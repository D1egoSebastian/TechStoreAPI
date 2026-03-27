const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api';


const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${localStorage.getItem("token")}`
});

const headers = () => ({
    "Content-Type": "application/json"
})

//Authenticated API calls
export const login = (data) =>
    fetch(`${API_URL}/auth/login`, {
        method: "POST",
        headers: headers(),
        body: JSON.stringify(data)
    });

export const register = (data) =>
    fetch(`${API_URL}/auth/register`, {
        method: "POST",
        headers: headers(),
        body: JSON.stringify(data)
    });

//Cart
export const getCart = () => 
    fetch(`${API_URL}/cart`, {
        method: "GET",
        headers: authHeaders()
    });

export const updateCart = (productId, data) =>
    fetch(`${API_URL}/cart/${productId}`, {
        method: "PUT",
        headers: authHeaders(),
        body: JSON.stringify(data)
    });

export const AddToCart = (data) =>
    fetch(`${API_URL}/cart`, {
        method: "POST",
        headers: authHeaders(),
        body: JSON.stringify(data)
    });

export const deleteCart = (productId) =>
    fetch(`${API_URL}/cart/${productId}`, {
        method: "DELETE",
        headers: authHeaders()
    });

//Categories
export const getCategories = () =>
    fetch(`${API_URL}/categories`, {
        method: "GET",
        headers: headers()
    });

export const updateCategory = (id, data) =>
    fetch(`${API_URL}/categories/${id}`, {
        method: "PUT",
        headers: authHeaders(),
        body: JSON.stringify(data)
    });

export const deleteCategory = (id) =>
    fetch(`${API_URL}/categories/${id}`, {
        method: "DELETE",
        headers: authHeaders()
    });

export const createCategory = (data) =>
    fetch(`${API_URL}/categories`, {
        method: "POST",
        headers: authHeaders(),
        body: JSON.stringify(data)
    });

//Products
export const getProducts = () =>
    fetch(`${API_URL}/product`, {
        method: "GET",
        headers: headers()
    });

export const updateProduct = (id, data) =>
    fetch(`${API_URL}/product/${id}`, {
        method: "PUT",
        headers: authHeaders(),
        body: JSON.stringify(data)
    });

export const getProductById = (id) =>
    fetch(`${API_URL}/product/${id}`, {
        method: "GET",
        headers: headers()
    });

export const deleteProduct = (id) =>
    fetch(`${API_URL}/product/${id}`, {
        method: "DELETE",
        headers: authHeaders()
    });

export const createProduct = (data) =>
    fetch(`${API_URL}/product`, {
        method: "POST",
        headers: authHeaders(),
        body: JSON.stringify(data)
    });

//Orders
export const getOrders = () =>
    fetch(`${API_URL}/orders`, {
        method: "GET",
        headers: authHeaders()
    });

export const updateOrder = (id, data) =>
    fetch(`${API_URL}/orders/${id}`, {
        method: "PUT",
        headers: authHeaders(),
        body: JSON.stringify(data)
    });

export const deleteOrder = (id) =>
    fetch(`${API_URL}/orders/${id}`, {
        method: "DELETE",
        headers: authHeaders()
    });

export const createOrder = (data) =>
    fetch(`${API_URL}/orders`, {
        method: "POST",
        headers: authHeaders(),
        body: JSON.stringify(data)
    });

//UploadImage
export const uploadImage = (file) => {
        const formData = new FormData();
        formData.append("file", file);
        return fetch(`${API_URL}/upload`, {
            method: "POST",
            body: formData
        });
    }
