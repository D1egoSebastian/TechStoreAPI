"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

export function Hero() {
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    setIsVisible(true);
  }, []);

  return (
    <section className="relative overflow-hidden bg-gradient-to-br from-[#0f0f0f] via-[#1a1a1a] to-[#0f0f0f] py-20 lg:py-32">
      {/* Background pattern */}
      <div className="absolute inset-0 opacity-30">
        <div className="absolute top-20 left-10 w-72 h-72 bg-orange-500/10 rounded-full blur-3xl" />
        <div className="absolute bottom-20 right-10 w-96 h-96 bg-blue-500/10 rounded-full blur-3xl" />
      </div>

      <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid lg:grid-cols-2 gap-12 items-center">
          {/* Content */}
          <div className={`transition-all duration-700 ${isVisible ? "opacity-100 translate-y-0" : "opacity-0 translate-y-8"}`}>
            <span className="inline-block px-4 py-1.5 bg-orange-500/10 text-orange-400 text-sm font-medium rounded-full mb-6">
              ✨ Nueva colección 2025
            </span>
            <h1 className="font-display text-4xl sm:text-5xl lg:text-6xl font-bold text-white leading-tight mb-6">
              Tecnología de
              <span className="text-orange-500"> última generación</span>
            </h1>
            <p className="text-lg text-gray-400 mb-8 max-w-lg">
              Descubre los productos tecnológicos más innovadores del mercado. 
              Desde smartphones hasta accesorios inteligentes, todo en un solo lugar.
            </p>
            <div className="flex flex-wrap gap-4">
              <Link
                href="#products"
                className="inline-flex items-center gap-2 px-6 py-3 bg-gradient-to-br from-orange-500 to-red-500 text-white font-medium rounded-xl hover:opacity-90 transition-all hover:-translate-y-1"
              >
                Ver Productos
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8l4 4m0 0l-4 4m4-4H3" />
                </svg>
              </Link>
              <Link
                href="#categories"
                className="inline-flex items-center gap-2 px-6 py-3 bg-white/10 text-white font-medium rounded-xl border border-white/20 hover:bg-white/20 transition-all"
              >
                Explorar Categorías
              </Link>
            </div>

            {/* Stats */}
            <div className="flex gap-8 mt-12 pt-8 border-t border-white/10">
              <div>
                <p className="font-display text-3xl font-bold text-white">500+</p>
                <p className="text-sm text-gray-500">Productos</p>
              </div>
              <div>
                <p className="font-display text-3xl font-bold text-white">10k+</p>
                <p className="text-sm text-gray-500">Clientes</p>
              </div>
              <div>
                <p className="font-display text-3xl font-bold text-white">4.9</p>
                <p className="text-sm text-gray-500">Valoración</p>
              </div>
            </div>
          </div>

          {/* Image */}
          <div className={`relative transition-all duration-700 delay-200 ${isVisible ? "opacity-100 translate-x-0" : "opacity-0 translate-x-8"}`}>
            <div className="relative aspect-square max-w-lg mx-auto pb-8">
              {/* Decorative elements */}
              <div className="absolute top-0 right-0 w-32 h-32 bg-orange-500/20 rounded-2xl rotate-12" />
              <div className="absolute bottom-10 left-10 w-24 h-24 bg-blue-500/20 rounded-2xl -rotate-12" />
              
              {/* Main card */}
              <div className="relative bg-gradient-to-br from-[#1a1a1a] to-[#2a2a2a] rounded-3xl shadow-2xl p-8 border border-white/10">
                <div className="aspect-video bg-gradient-to-br from-[#1a1a1a] to-[#2a2a2a] rounded-2xl mb-6 flex items-center justify-center border border-white/5">
                  <svg className="w-24 h-24 text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                  </svg>
                </div>
                <div className="space-y-3">
                  <div className="h-4 bg-white/10 rounded w-3/4" />
                  <div className="h-4 bg-white/10 rounded w-1/2" />
                  <div className="flex gap-2">
                    <div className="h-8 w-24 bg-orange-500/20 rounded-lg" />
                    <div className="h-8 w-24 bg-white/10 rounded-lg" />
                  </div>
                </div>
              </div>

              {/* Floating badge - centered below card */}
              <div className="absolute -bottom-2 left-1/2 -translate-x-1/2 bg-gradient-to-r from-emerald-500 to-teal-600 rounded-2xl shadow-xl p-4 animate-bounce z-10" style={{ animationDuration: "3s" }}>
                <div className="flex items-center gap-3">
                  <div className="w-12 h-12 bg-white/20 backdrop-blur-sm rounded-full flex items-center justify-center">
                    <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                    </svg>
                  </div>
                  <div>
                    <p className="text-sm font-bold text-white">Envío Gratis</p>
                    <p className="text-xs text-white/80">En pedidos superiores a $50</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}