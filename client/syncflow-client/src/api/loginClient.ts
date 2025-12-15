// src/api/loginClient.ts
import axios from "axios";

const loginClient = axios.create({
  baseURL: "https://localhost:44318", // Sin /api/
  headers: {
    "Content-Type": "application/json",
  },
});

// Puedes manejar errores globales si quieres, pero sin token
loginClient.interceptors.response.use(
  (response) => response,
  (error) => {
    // Ejemplo: manejo de errores de login
    if (error.response?.status === 400) {
      console.error("Credenciales inválidas");
    }
    return Promise.reject(error);
  }
);

export default loginClient;
