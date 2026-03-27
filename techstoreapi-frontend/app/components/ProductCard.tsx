"use client";

import { useCart } from "../providers";
import Link from "next/link";

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

interface ProductCardProps {
  product: Product;
}

export function ProductCard({ product }: ProductCardProps) {
  const { addItem } = useCart();

  const handleAddToCart = (e: React.MouseEvent) => {
    e.preventDefault();
    addItem({
      productId: product.id,
      productName: product.name,
      productPrice: product.price,
      quantity: 1,
    });
  };

  return (
    <Link href={`/product/${product.id}`} className="block group">
      <div className="bg-[#1a1a1a] rounded-2xl border border-white/10 overflow-hidden hover:shadow-xl transition-all duration-300 hover:-translate-y-1 hover:border-orange-500/30">
        {/* Image */}
        <div className="aspect-square bg-[#2a2a2a] relative overflow-hidden">
          {product.image_url ? (
            <img
              src={product.image_url}
              alt={product.name}
              className="w-full h-full object-cover transition-transform duration-500 group-hover:scale-105"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center">
              <svg className="w-16 h-16 text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
              </svg>
            </div>
          )}
          
          {/* Category badge */}
          {product.category && (
            <span className="absolute top-3 left-3 px-3 py-1 bg-black/60 backdrop-blur-sm text-xs font-medium text-white/90 rounded-full">
              {product.category.name}
            </span>
          )}

          {/* Quick add button */}
          <button
            onClick={handleAddToCart}
            className="absolute bottom-3 right-3 w-10 h-10 bg-gradient-to-br from-orange-500 to-red-500 text-white rounded-full flex items-center justify-center opacity-0 group-hover:opacity-100 transition-all translate-y-2 group-hover:translate-y-0 shadow-lg hover:opacity-90"
          >
            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
          </button>
        </div>

        {/* Content */}
        <div className="p-4">
          <h3 className="font-medium text-white mb-1 line-clamp-1">{product.name}</h3>
          {product.description && (
            <p className="text-sm text-gray-400 mb-3 line-clamp-2">{product.description}</p>
          )}
          <div className="flex items-center justify-between">
            <span className="font-display font-bold text-xl text-white">
              ${product.price.toFixed(2)}
            </span>
            <button
              onClick={handleAddToCart}
              className="px-4 py-2 bg-white/10 text-white text-sm font-medium rounded-lg hover:bg-orange-500 transition-colors border border-white/10"
            >
              Agregar
            </button>
          </div>
        </div>
      </div>
    </Link>
  );
}