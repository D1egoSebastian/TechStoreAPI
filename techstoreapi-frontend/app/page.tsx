import { Navbar } from "./components/Navbar";
import { Hero } from "./components/Hero";
import { ProductGrid } from "./components/ProductGrid";
import { CategorySection } from "./components/CategorySection";
import { CartSidebar } from "./components/CartSidebar";

export default function Home() {
  return (
    <>
      <Navbar />
      <CartSidebar />
      <main>
        <Hero />
        
        {/* Products Section */}
        <section id="products" className="py-16">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="text-center mb-12">
              <h2 className="font-display text-4xl font-bold text-white mb-4">Productos Destacados</h2>
              <p className="text-gray-400">Los productos más populares de nuestra tienda</p>
            </div>
            <ProductGrid />
          </div>
        </section>

        {/* Categories Section */}
        <CategorySection />

        {/* Footer */}
        <footer className="bg-[#0a0a0a] text-white py-12 border-t border-white/10">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="grid md:grid-cols-4 gap-8">
              <div>
                <div className="flex items-center gap-2 mb-4">
                  <div className="w-8 h-8 bg-gradient-to-br from-orange-500 to-red-500 rounded-lg flex items-center justify-center">
                    <span className="text-white font-bold text-sm">TS</span>
                  </div>
                  <span className="font-display font-semibold text-xl">TechStore</span>
                </div>
                <p className="text-gray-400 text-sm">
                  Tu tienda de tecnología de confianza. Los mejores productos al mejor precio.
                </p>
              </div>
              <div>
                <h4 className="font-medium mb-4">Comprar</h4>
                <ul className="space-y-2 text-gray-400 text-sm">
                  <li><a href="#" className="hover:text-white transition-colors">Smartphones</a></li>
                  <li><a href="#" className="hover:text-white transition-colors">Laptops</a></li>
                  <li><a href="#" className="hover:text-white transition-colors">Audio</a></li>
                  <li><a href="#" className="hover:text-white transition-colors">Accesorios</a></li>
                </ul>
              </div>
              <div>
                <h4 className="font-medium mb-4">Empresa</h4>
                <ul className="space-y-2 text-gray-400 text-sm">
                  <li><a href="#" className="hover:text-white transition-colors">Sobre nosotros</a></li>
                  <li><a href="#" className="hover:text-white transition-colors">Contacto</a></li>
                  <li><a href="#" className="hover:text-white transition-colors">Términos</a></li>
                  <li><a href="#" className="hover:text-white transition-colors">Privacidad</a></li>
                </ul>
              </div>
              <div>
                <h4 className="font-medium mb-4">Newsletter</h4>
                <p className="text-gray-400 text-sm mb-4">Recibe ofertas exclusivas en tu correo</p>
                <div className="flex">
                  <input
                    type="email"
                    placeholder="Tu email"
                    className="flex-1 px-4 py-2 bg-[#1a1a1a] border border-white/10 rounded-l-lg text-white text-sm focus:outline-none focus:border-orange-500"
                  />
                  <button className="px-4 py-2 bg-gradient-to-br from-orange-500 to-red-500 rounded-r-lg hover:opacity-90 transition-opacity">
                    <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M14 5l7 7m0 0l-7 7m7-7H3" />
                    </svg>
                  </button>
                </div>
              </div>
            </div>
            <div className="border-t border-white/10 mt-8 pt-8 text-center text-gray-400 text-sm">
              © 2025 TechStore. Todos los derechos reservados.
            </div>
          </div>
        </footer>
      </main>
    </>
  );
}