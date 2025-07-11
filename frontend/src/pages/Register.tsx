import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { useNavigate, Link } from "react-router-dom";
import { useAppDispatch } from "../hooks/useAuth";
import { register as registerUser } from "../features/auth/authSlice";
import type { AuthRequest } from "../types/AuthDto";
import { useState } from "react";

const schema = yup.object().shape({
    email: yup.string().required("Email requerido").email("Email inválido"),
    password: yup.string().required("Contraseña requerida").min(6, "Mínimo 6 caracteres"),
});

export default function Register() {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const [error, setError] = useState<string | null>(null);

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<AuthRequest>({
        resolver: yupResolver(schema),
    });

    const onSubmit = async (data: AuthRequest) => {
        setError(null);
        const result = await dispatch(registerUser(data));
        if (registerUser.fulfilled.match(result)) {
            navigate("/login");
        } else {
            setError("No se pudo registrar. El correo ya está en uso o hubo un error.");
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <form
                onSubmit={handleSubmit(onSubmit)}
                className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md"
            >
                <h2 className="text-3xl font-bold text-center text-gray-800 mb-6">Registro</h2>

                {error && (
                    <div className="mb-4 bg-red-100 text-red-700 px-4 py-2 rounded border border-red-300 text-sm">
                        {error}
                    </div>
                )}

                <div className="mb-5">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
                    <input
                        type="email"
                        {...register("email")}
                        className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
                        placeholder="tucorreo@ejemplo.com"
                    />
                    {errors.email && <p className="text-red-500 text-sm mt-1">{errors.email.message}</p>}
                </div>

                <div className="mb-6">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Contraseña</label>
                    <input
                        type="password"
                        {...register("password")}
                        className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
                        placeholder="••••••••"
                    />
                    {errors.password && <p className="text-red-500 text-sm mt-1">{errors.password.message}</p>}
                </div>

                <button
                    type="submit"
                    className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition-colors font-semibold"
                >
                    Registrarse
                </button>

                <p className="mt-4 text-center text-sm text-gray-600">
                    ¿Ya tienes una cuenta?{" "}
                    <Link to="/login" className="text-blue-600 hover:underline">
                        Inicia sesión aquí
                    </Link>
                </p>
            </form>
        </div>
    );
}
