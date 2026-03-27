"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { login } from "../../services/API";
import { useAuth } from "../providers";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  
  const { login: setAuth } = useAuth();
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const res = await login({ Email: email, PasswordHash: password });
      
      if (res.ok) {
        const data = await res.json();
        const token = data.token;
        
        // Decode JWT to get user info (simple base64 decode)
        const payload = JSON.parse(atob(token.split('.')[1]));
        const user = {
          id: parseInt(payload.nameid),
          email: payload.email,
          name: payload.unique_name,
          role: payload.role
        };
        
        setAuth(token, user);
        router.push("/");
      } else {
        const data = await res.json();
        setError(data.message || "Credenciales inválidas");
      }
    } catch (err) {
      setError("Error de conexión");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-[#0f0f0f] py-12 px-4">
      <div className="max-w-md w-full">
        {/* Logo */}
        <div className="text-center mb-8">
          <Link href="/" className="inline-flex items-center gap-2">
            <div className="w-12 h-12 bg-gradient-to-br from-orange-500 to-red-500 rounded-xl flex items-center justify-center">
              <span className="text-white font-bold text-lg">TS</span>
            </div>
            <span className="font-display font-semibold text-2xl text-white">TechStore</span>
          </Link>
        </div>

        {/* Card */}
        <div className="bg-[#1a1a1a] rounded-3xl shadow-xl p-8 border border-white/10">
          <h1 className="font-display text-2xl font-bold text-white text-center mb-2">
            Bienvenido de nuevo
          </h1>
          <p className="text-gray-400 text-center mb-8">
            Ingresa a tu cuenta para continuar
          </p>

          {error && (
            <div className="mb-6 p-4 bg-red-500/10 border border-red-500/20 rounded-xl text-red-400 text-sm">
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-2">
                Email
              </label>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="w-full px-4 py-3 bg-[#0f0f0f] border border-white/10 rounded-xl focus:outline-none focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20 transition-all text-white placeholder-gray-500"
                placeholder="tu@email.com"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-300 mb-2">
                Contraseña
              </label>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full px-4 py-3 bg-[#0f0f0f] border border-white/10 rounded-xl focus:outline-none focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20 transition-all text-white placeholder-gray-500"
                placeholder="••••••••"
                required
              />
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full py-3 bg-gradient-to-br from-orange-500 to-red-500 text-white font-medium rounded-xl hover:opacity-90 transition-opacity disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {loading ? (
                <span className="flex items-center justify-center gap-2">
                  <svg className="animate-spin h-5 w-5" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
                  </svg>
                  Iniciando...
                </span>
              ) : (
                "Iniciar sesión"
              )}
            </button>
          </form>

          <p className="text-center text-gray-400 mt-6">
            ¿No tienes cuenta?{" "}
            <Link href="/register" className="text-orange-500 font-medium hover:underline">
              Regístrate
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}