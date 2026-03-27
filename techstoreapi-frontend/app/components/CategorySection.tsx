"use client";

import { useState, useEffect } from "react";
import { getCategories } from "../../services/API";

interface Category {
  id: number;
  name: string;
  description?: string;
}

const categoryIcons: Record<string, string> = {
  "Smartphones": "M17.5 19H9a7 7 0 110-14h8a7 7 0 010 14z",
  "Laptops": "M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z",
  "Audio": "M9 19V6l12-3v13M9 19c0 1.105-1.343 2-3 2s-3-.895-3-2 1.343-2 3-2 3 .895 3 2zm12-3c0 1.105-1.343 2-3 2s-3-.895-3-2 1.343-2 3-2 3 .895 3 2zM9 10l12-3",
  "Gaming": "M11 4a2 2 0 114 0v1a1 1 0 001 1h3a1 1 0 011 1v3a1 1 0 01-1 1h-1a2 2 0 100 4h1a1 1 0 011 1v3a1 1 0 01-1 1h-3a1 1 0 01-1-1v-1a2 2 0 10-4 0v1a1 1 0 01-1 1H7a1 1 0 01-1-1v-3a1 1 0 00-1-1H4a2 2 0 110-4h1a1 1 0 001-1V7a1 1 0 011-1h3a1 1 0 001-1V4z",
  "default": "M4 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2V6zM14 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2V6zM4 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2v-2zM14 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2v-2z"
};

export function CategorySection() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const res = await getCategories();
        if (res.ok) {
          const data = await res.json();
          setCategories(data);
        }
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchCategories();
  }, []);

  if (loading) {
    return (
      <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
        {[...Array(4)].map((_, i) => (
          <div key={i} className="bg-[#1a1a1a] rounded-2xl border border-white/10 p-6 animate-pulse">
            <div className="w-12 h-12 bg-white/10 rounded-xl mb-4" />
            <div className="h-4 bg-white/10 rounded w-3/4" />
          </div>
        ))}
      </div>
    );
  }

  return (
    <section id="categories" className="py-16 bg-[#0f0f0f]">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-12">
          <h2 className="font-display text-3xl font-bold text-white mb-4">Categorías</h2>
          <p className="text-gray-400 max-w-2xl mx-auto">
            Explora nuestras categorías y encuentra exactamente lo que buscas
          </p>
        </div>

        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
          {categories.map((category, index) => (
            <div
              key={category.id}
              className="group bg-[#1a1a1a] rounded-2xl border border-white/10 p-6 hover:shadow-lg hover:border-orange-500/30 transition-all duration-300 cursor-pointer animate-fade-in"
              style={{ animationDelay: `${index * 0.1}s` }}
            >
              <div className="w-12 h-12 bg-orange-500/10 rounded-xl flex items-center justify-center mb-4 group-hover:bg-orange-500 transition-colors">
                <svg className="w-6 h-6 text-orange-500 group-hover:text-white transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d={categoryIcons[category.name] || categoryIcons["default"]} />
                </svg>
              </div>
              <h3 className="font-medium text-white">{category.name}</h3>
              {category.description && (
                <p className="text-sm text-gray-400 mt-1 line-clamp-1">{category.description}</p>
              )}
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}