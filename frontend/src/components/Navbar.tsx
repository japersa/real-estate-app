import { Link, useNavigate } from "react-router-dom";

export default function Navbar() {
    const navigate = useNavigate();

    const logout = () => {
        const confirmed = window.confirm("¿Estás seguro de que deseas cerrar sesión?");
        if (!confirmed) return;

        localStorage.removeItem("token");
        navigate("/login");
    };

    return (
        <nav className="bg-white text-gray-800 px-6 py-4 flex items-center justify-between shadow-md">
            <div className="flex items-center gap-6">
                <Link
                    to="/owners"
                    className="hover:text-blue-600 transition-colors font-medium"
                >
                    Propietarios
                </Link>
                <Link
                    to="/properties"
                    className="hover:text-blue-600 transition-colors font-medium"
                >
                    Propiedades
                </Link>
            </div>
            <button
                onClick={logout}
                className="bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded-md text-sm font-semibold transition-colors"
            >
                Cerrar sesión
            </button>
        </nav>
    );
}
