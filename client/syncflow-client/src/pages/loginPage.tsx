import React, { useState } from "react";
import loginApi from "../api/loginApi";

const LoginPage: React.FC = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");

        try {
            debugger;
            const response = await loginApi.login(email, password);

            const { token } = response;
            localStorage.setItem("token", token);

            // Redirigir a la página principal
            window.location.href = "/projects";
        } catch (err: any) {
            setError(err.response?.data?.message || "Error al iniciar sesión");
        }
    };

    return (
        <div className="flex h-screen items-center justify-center bg-gray-100">
            <div className="bg-white shadow-lg rounded-lg p-8 w-96">
                <h1 className="text-2xl font-bold mb-4 text-center">Iniciar Sesión</h1>
                <form onSubmit={handleLogin} className="space-y-4">
                    <div>
                        <label className="block mb-1">Correo electrónico</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="w-full border rounded px-3 py-2"
                            required
                        />
                    </div>

                    <div>
                        <label className="block mb-1">Contraseña</label>
                        <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="w-full border rounded px-3 py-2"
                            required
                        />
                    </div>

                    {error && <p className="text-red-500 text-sm">{error}</p>}

                    <button
                        type="submit"
                        className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
                    >
                        Ingresar
                    </button>
                </form>
            </div>
        </div>
    );
};

export default LoginPage;
