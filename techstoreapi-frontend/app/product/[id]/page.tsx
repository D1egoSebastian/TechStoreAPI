"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import Link from "next/link";
import { Navbar } from "../../components/Navbar";
import { CartSidebar } from "../../components/CartSidebar";
import { useCart } from "../../providers";

interface Product {
  id: number;
  name: string;
  description?: string;
  price: number;
  stock: number;
  image_url?: string;
  category?: {
    id: number;
    name: string;
    description?: string;
  };
}

export default function ProductDetail() {
  const params = useParams();
  const { addItem } = useCart();
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);
  const [quantity, setQuantity] = useState(1);

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const url = `http://localhost:5000/api/product/${params.id}`;
        console.log("Fetching:", url);
        const res = await fetch(url);
        console.log("Response:", res.status, res.ok);
        if (res.ok) {
          const data = await res.json();
          console.log("Product data:", data);
          setProduct(data);
        } else {
          console.error("Error fetching product:", await res.text());
        }
      } catch (err) {
        console.error("Fetch error:", err);
      } finally {
        setLoading(false);
      }
    };

    if (params.id) {
      fetchProduct();
    }
  }, [params.id]);

  const handleAddToCart = () => {
    if (product) {
      addItem({
        productId: product.id,
        productName: product.name,
        productPrice: product.price,
        quantity,
      });
    }
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <main className="min-h-screen bg-[#0f0f0f] flex items-center justify-center">
          <div className="animate-spin w-8 h-8 border-2 border-orange-500 border-t-transparent rounded-full" />
        </main>
      </>
    );
  }

  if (!product) {
    return (
      <>
        <Navbar />
        <main className="min-h-screen bg-[#0f0f0f] flex items-center justify-center">
          <div className="text-center">
            <p className="text-gray-400 mb-4">Producto no encontrado</p>
            <Link href="/" className="text-orange-500 hover:underline">
              Volver al inicio
            </Link>
          </div>
        </main>
      </>
    );
  }

  return (
    <>
      <Navbar />
      <CartSidebar />
      <main className="min-h-screen bg-[#0f0f0f] py-12">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          {/* Breadcrumb */}
          <nav className="flex items-center gap-2 text-sm text-gray-400 mb-8">
            <Link href="/" className="hover:text-white">Inicio</Link>
            <span>/</span>
            <Link href="#products" className="hover:text-white">Productos</Link>
            <span>/</span>
            <span className="text-white">{product.name}</span>
          </nav>

          {/* Product Details */}
          <div className="grid lg:grid-cols-2 gap-12">
            {/* Image */}
            <div className="relative aspect-square bg-[#1a1a1a] rounded-3xl overflow-hidden border border-white/10">
              {product.image_url ? (
                <img
                  src={product.image_url}
                  alt={product.name}
                  className="w-full h-full object-cover"
                />
              ) : (
                <div className="w-full h-full flex items-center justify-center">
                  <svg className="w-24 h-24 text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                  </svg>
                </div>
              )}
            </div>

            {/* Info */}
            <div className="flex flex-col">
              {/* Category */}
              {product.category && (
                <span className="inline-block px-3 py-1 bg-orange-500/10 text-orange-400 text-sm font-medium rounded-full w-fit mb-4">
                  {product.category.name}
                </span>
              )}

              {/* Name */}
              <h1 className="font-display text-4xl font-bold text-white mb-4">
                {product.name}
              </h1>

              {/* Price */}
              <div className="flex items-baseline gap-4 mb-6">
                <span className="font-display text-4xl font-bold text-white">
                  ${product.price.toFixed(2)}
                </span>
                <span className="text-gray-400">
                  {product.stock > 0 ? (
                    <span className="text-green-400">✓ En stock ({product.stock} disponibles)</span>
                  ) : (
                    <span className="text-red-400">Sin stock</span>
                  )}
                </span>
              </div>

              {/* Description */}
              <div className="mb-8">
                <h2 className="font-semibold text-white text-lg mb-3">Descripción</h2>
                <p className="text-gray-400 leading-relaxed">
                  {product.description || "No hay descripción disponible para este producto."}
                </p>
              </div>

              {/* Quantity & Add to Cart */}
              <div className="mt-auto">
                <div className="flex items-center gap-4 mb-6">
                  <div className="flex items-center bg-[#1a1a1a] border border-white/10 rounded-xl">
                    <button
                      onClick={() => setQuantity(Math.max(1, quantity - 1))}
                      className="px-4 py-3 text-white hover:text-orange-500 transition-colors"
                    >
                      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 12H4" />
                      </svg>
                    </button>
                    <span className="px-4 py-3 text-white font-medium">{quantity}</span>
                    <button
                      onClick={() => setQuantity(quantity + 1)}
                      className="px-4 py-3 text-white hover:text-orange-500 transition-colors"
                    >
                      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
                      </svg>
                    </button>
                  </div>
                </div>

                <button
                  onClick={handleAddToCart}
                  disabled={product.stock === 0}
                  className="w-full py-4 bg-gradient-to-br from-orange-500 to-red-500 text-white font-semibold rounded-xl hover:opacity-90 transition-opacity disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  {product.stock === 0 ? "Sin stock" : "Agregar al carrito"}
                </button>
              </div>
            </div>
          </div>
        </div>
      </main>
    </>
  );
}