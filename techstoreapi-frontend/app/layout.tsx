import type { Metadata } from "next";
import { Sora, Inter } from "next/font/google";
import "./globals.css";
import { Providers } from "./providers";

const sora = Sora({
  variable: "--font-display",
  subsets: ["latin"],
  display: "swap",
});

const inter = Inter({
  variable: "--font-body",
  subsets: ["latin"],
  display: "swap",
});

export const metadata: Metadata = {
  title: "TechStore - Tu tienda de tecnología",
  description: "Encuentra los mejores productos tecnológicos en TechStore",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="es" className={`${sora.variable} ${inter.variable} dark`}>
      <body className="min-h-screen bg-[#0f0f0f] text-white antialiased">
        <Providers>{children}</Providers>
      </body>
    </html>
  );
}