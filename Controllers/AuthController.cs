using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TechStoreAPI.Data;
using TechStoreAPI.DTOs;
using TechStoreAPI.Models;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase 
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public  AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterDto dto)
    {
        try
        {
            //check if the email to enter already exist in db
            var emailexist = _context.Users.Any(e => e.Email == dto.Email);

            if (emailexist)
            {
                return BadRequest("that email already exist");
            }

            //bycrypt password
            var cryptedpassword = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = cryptedpassword,
                CreatedAt = DateTime.UtcNow
                
            };

            _context.Users.Add(newUser);
            await  _context.SaveChangesAsync();

            return Created(string.Empty,new
            {
                id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email

            });

            
            
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);  
        };

        
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);

        if(user == null)
        {
            return Unauthorized();
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash))
        {
            return Unauthorized();
        }

        //jwt implementation
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
            
        };

        //iconfiguration
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expiresminutos = int.Parse(_configuration["Jwt:ExpiresMinutos"]?? "60");

        var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

        //Token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.UtcNow.AddMinutes(expiresminutos),
            claims: claims,
            signingCredentials: credentials
        );

        var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = tokenstring
        });
    }
}