import { useEffect } from "react";
import { useSelector } from "react-redux";
import { useAppDispatch } from "../hooks/useAuth";
import { fetchProperties, deleteProperty } from "../features/properties/propertySlice";
import { Link } from "react-router-dom";
import type { RootState } from "../app/store";
import Navbar from "../components/Navbar";

export default function Properties() {
    const dispatch = useAppDispatch();
    const { list, loading } = useSelector((state: RootState) => state.properties);

    useEffect(() => {
        dispatch(fetchProperties({ pageNumber: 1, pageSize: 10 }));
    }, [dispatch]);

    const handleDelete = async (id: string) => {
        const confirmDelete = window.confirm("¿Estás seguro de que deseas eliminar esta propiedad?");
        if (!confirmDelete) return;

        await dispatch(deleteProperty(id));
        dispatch(fetchProperties({ pageNumber: 1, pageSize: 10 }));
    };

    return (
        <>
            <Navbar />
            <div className="p-6 max-w-6xl mx-auto">
                <div className="flex justify-between items-center mb-6">
                    <h2 className="text-2xl font-semibold text-gray-800">Propiedades</h2>
                    <Link
                        to="/properties/create"
                        className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 transition-colors"
                    >
                        Nueva
                    </Link>
                </div>

                {loading ? (
                    <p className="text-center text-gray-500">Cargando...</p>
                ) : (
                    <div className="overflow-x-auto rounded-lg shadow">
                        <table className="min-w-full bg-white border border-gray-200 text-sm text-left">
                            <thead className="bg-gray-100 text-gray-700 uppercase text-xs tracking-wider">
                                <tr>
                                    <th className="px-6 py-3 border-b">Nombre</th>
                                    <th className="px-6 py-3 border-b">Dirección</th>
                                    <th className="px-6 py-3 border-b">Precio</th>
                                    <th className="px-6 py-3 border-b">Propietario</th>
                                    <th className="px-6 py-3 border-b">Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                {list.map((p) => (
                                    <tr key={p.id} className="hover:bg-gray-50">
                                        <td className="px-6 py-4 border-b">{p.name}</td>
                                        <td className="px-6 py-4 border-b">{p.address}</td>
                                        <td className="px-6 py-4 border-b">${p.price}</td>
                                        <td className="px-6 py-4 border-b">{p.owner?.name}</td>
                                        <td className="px-6 py-4 border-b">
                                            <div className="flex gap-2">
                                                <Link
                                                    to={`/properties/edit/${p.id}`}
                                                    className="px-3 py-1 text-sm text-white bg-green-600 rounded-md hover:bg-green-700 transition-colors"
                                                >
                                                    Editar
                                                </Link>
                                                <button
                                                    onClick={() => handleDelete(p.id!)}
                                                    className="px-3 py-1 text-sm text-white bg-red-600 rounded-md hover:bg-red-700 transition-colors"
                                                >
                                                    Eliminar
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>
        </>
    );
}
