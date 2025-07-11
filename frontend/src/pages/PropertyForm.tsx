import { useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { useAppDispatch } from "../hooks/useAuth";
import { createProperty, updateProperty } from "../features/properties/propertySlice";
import propertyService from "../features/properties/PropertyService";
import ownerService from "../features/owners/OwnerService";
import type { OwnerDto } from "../types/OwnerDto";
import Navbar from "../components/Navbar";

type PropertyFormData = {
    name: string;
    address: string;
    price: number;
    imageUrl: string;
    idOwner: string;
};

const schema = yup.object().shape({
    name: yup.string().required("Nombre requerido"),
    address: yup.string().required("Dirección requerida"),
    price: yup.number().required("Precio requerido").positive("Debe ser positivo"),
    imageUrl: yup.string().required("Imagen requerida").url("Debe ser una URL válida"),
    idOwner: yup.string().required("Propietario requerido"),
});

export default function PropertyForm() {
    const { id } = useParams();
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const [owners, setOwners] = useState<OwnerDto[]>([]);

    const { register, handleSubmit, setValue, formState: { errors } } = useForm<PropertyFormData>({
        resolver: yupResolver(schema),
    });

    useEffect(() => {
        ownerService.getAll().then(data => setOwners(data.items || data));
        if (id) {
            propertyService.getById(id).then(data => {
                Object.keys(data).forEach((key) => {
                    setValue(key as keyof PropertyFormData, data[key as keyof PropertyFormData]);
                });
            });
        }
    }, [id, setValue]);

    const onSubmit = async (data: PropertyFormData) => {
        if (id) {
            await dispatch(updateProperty({ id, property: data }));
        } else {
            await dispatch(createProperty(data));
        }
        navigate("/properties");
    };

    return (
        <>
            <Navbar />
            <div className="p-6 max-w-xl mx-auto">
                <h2 className="text-2xl font-semibold mb-6 text-gray-800">{id ? "Editar" : "Crear"} Propiedad</h2>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-5 bg-white shadow rounded-lg p-6">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Nombre</label>
                        <input
                            {...register("name")}
                            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
                            placeholder="Nombre"
                        />
                        {errors.name && <p className="text-sm text-red-500">{errors.name.message}</p>}
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Dirección</label>
                        <input
                            {...register("address")}
                            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
                            placeholder="Dirección"
                        />
                        {errors.address && <p className="text-sm text-red-500">{errors.address.message}</p>}
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Precio</label>
                        <input
                            type="number"
                            {...register("price")}
                            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
                            placeholder="Precio"
                        />
                        {errors.price && <p className="text-sm text-red-500">{errors.price.message}</p>}
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">URL de Imagen</label>
                        <input
                            {...register("imageUrl")}
                            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
                            placeholder="https://..."
                        />
                        {errors.imageUrl && <p className="text-sm text-red-500">{errors.imageUrl.message}</p>}
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Propietario</label>
                        <select
                            {...register("idOwner")}
                            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
                        >
                            <option value="">Selecciona un propietario</option>
                            {owners.map((o) => (
                                <option key={o.id} value={o.id}>
                                    {o.name}
                                </option>
                            ))}
                        </select>
                        {errors.idOwner && <p className="text-sm text-red-500">{errors.idOwner.message}</p>}
                    </div>

                    <button
                        type="submit"
                        className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition-colors"
                    >
                        {id ? "Actualizar" : "Crear"}
                    </button>
                </form>
            </div>
        </>
    );
}
