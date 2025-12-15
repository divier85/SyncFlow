import axios from "axios";

const axiosClient = axios.create({
  baseURL: "https://localhost:44318/api",
  headers: {
    "Content-Type": "application/json",
  },
});

// Interceptor para agregar token a cada request
axiosClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Interceptor para manejar errores globales (401, etc.)
axiosClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // El token no es válido o ha expirado
      localStorage.removeItem("token");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

export default axiosClient;
