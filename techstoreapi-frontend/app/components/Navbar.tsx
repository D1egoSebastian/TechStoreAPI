"use client";

import { useState } from "react";
import Link from "next/link";
import { useCart, useAuth } from "../providers";

export function Navbar() {
  const { items, total, isOpen, setIsOpen } = useCart();
  const { user, isAuthenticated, logout } = useAuth();
  const [menuOpen, setMenuOpen] = useState(false);

  const cartCount = items.reduce((sum, item) => sum + item.quantity, 0);

  return (
    <nav className="sticky top-0 z-50 bg-[#0f0f0f]/80 backdrop-blur-xl border-b border-white/10">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <Link href="/" className="flex items-center gap-2">
            <div className="w-8 h-8 bg-gradient-to-br from-orange-500 to-red-500 rounded-lg flex items-center justify-center">
              <span className="text-white font-bold text-sm">TS</span>
            </div>
            <span className="font-display font-semibold text-xl text-white">TechStore</span>
          </Link>

          {/* Desktop Navigation */}
          <div className="hidden md:flex items-center gap-8">
            <Link href="/" className="text-sm font-medium text-gray-300 hover:text-white">
              Inicio
            </Link>
            <Link href="#products" className="text-sm font-medium text-gray-300 hover:text-white">
              Productos
            </Link>
            <Link href="#categories" className="text-sm font-medium text-gray-300 hover:text-white">
              Categorías
            </Link>
          </div>

          {/* Right side */}
          <div className="flex items-center gap-4">
            {/* Cart button */}
            <button
              onClick={() => setIsOpen(!isOpen)}
              className="relative p-2 text-gray-300 hover:text-white transition-colors"
            >
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
              </svg>
              {cartCount > 0 && (
                <span className="absolute -top-1 -right-1 w-5 h-5 bg-gradient-to-br from-orange-500 to-red-500 text-white text-xs font-medium rounded-full flex items-center justify-center">
                  {cartCount}
                </span>
              )}
            </button>

            {/* Auth buttons */}
            {isAuthenticated ? (
              <div className="flex items-center gap-4">
                <span className="hidden sm:block text-sm text-gray-300">Hola, {user?.name}</span>
                <button
                  onClick={logout}
                  className="text-sm font-medium text-gray-300 hover:text-white"
                >
                  Cerrar sesión
                </button>
              </div>
            ) : (
              <div className="flex items-center gap-3">
                <Link
                  href="/login"
                  className="text-sm font-medium text-gray-300 hover:text-white"
                >
                  Iniciar sesión
                </Link>
                <Link
                  href="/register"
                  className="text-sm font-medium px-4 py-2 bg-gradient-to-br from-orange-500 to-red-500 text-white rounded-lg hover:opacity-90 transition-opacity"
                >
                  Registrarse
                </Link>
              </div>
            )}

            {/* Mobile menu button */}
            <button
              onClick={() => setMenuOpen(!menuOpen)}
              className="md:hidden p-2 text-gray-300"
            >
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                {menuOpen ? (
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                ) : (
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
                )}
              </svg>
            </button>
          </div>
        </div>

        {/* Mobile menu */}
        {menuOpen && (
          <div className="md:hidden py-4 border-t border-white/10">
            <div className="flex flex-col gap-4">
              <Link href="/" className="text-sm font-medium text-gray-300">
                Inicio
              </Link>
              <Link href="#products" className="text-sm font-medium text-gray-300">
                Productos
              </Link>
              <Link href="#categories" className="text-sm font-medium text-gray-300">
                Categorías
              </Link>
            </div>
          </div>
        )}
      </div>
    </nav>
  );
}