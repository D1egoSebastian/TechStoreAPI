"use client";

import { useState, useEffect } from "react";
import { ProductCard } from "./ProductCard";
import { getProducts } from "../../services/API";

interface Product {
  id: number;
  name: string;
  description?: string;
  price: number;
  image_url?: string;
  category?: {
    id: number;
    name: string;
    description?: string;
  };
}

export function ProductGrid() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const res = await getProducts();
        if (res.ok) {
          const data = await res.json();
          setProducts(data);
        } else {
          setError("Error al cargar productos");
        }
      } catch (err) {
        setError("Error de conexión");
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, []);

  if (loading) {
    return (
      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {[...Array(8)].map((_, i) => (
          <div key={i} className="bg-[#1a1a1a] rounded-2xl border border-white/10 overflow-hidden animate-pulse">
            <div className="aspect-square bg-white/5" />
            <div className="p-4 space-y-3">
              <div className="h-4 bg-white/10 rounded w-3/4" />
              <div className="h-4 bg-white/10 rounded w-1/2" />
              <div className="flex justify-between">
                <div className="h-6 bg-white/10 rounded w-24" />
                <div className="h-8 bg-white/10 rounded w-20" />
              </div>
            </div>
          </div>
        ))}
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center py-12">
        <p className="text-gray-500">{error}</p>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
      {products.map((product, index) => (
        <div
          key={product.id}
          className="animate-fade-in"
          style={{ animationDelay: `${index * 0.05}s` }}
        >
          <ProductCard product={product} />
        </div>
      ))}
    </div>
  );
}