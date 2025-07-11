import { useForm } from "react-hook-form";
import { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { useAppDispatch } from "../hooks/useAuth";
import { createOwner, updateOwner } from "../features/owners/ownerSlice";
import OwnerService from "../features/owners/OwnerService";
import Navbar from "../components/Navbar";

type OwnerFormData = {
    name: string;
    email: string;
};

const schema = yup.object().shape({
    name: yup.string().required("Nombre requerido"),
    email: yup.string().required("Email requerido").email("Email inválido"),
});

export default function OwnerForm() {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const { id } = useParams();

    const {
        register,
        handleSubmit,
        setValue,
        formState: { errors },
    } = useForm<OwnerFormData>({
        resolver: yupResolver(schema),
    });

    useEffect(() => {
        if (id) {
            OwnerService.getById(id).then((data) => {
                setValue("name", data.name);
                setValue("email", data.email);
            });
        }
    }, [id, setValue]);

    const onSubmit = async (data: OwnerFormData) => {
        if (id) {
            await dispatch(updateOwner({ id, owner: data }));
        } else {
            await dispatch(createOwner(data));
        }
        navigate("/owners");
    };

    return (
        <>
            <Navbar />
            <div className="p-6 max-w-xl mx-auto">
                <h2 className="text-2xl font-semibold mb-6 text-gray-800">
                    {id ? "Editar" : "Crear"} Propietario
                </h2>
                <form
                    onSubmit={handleSubmit(onSubmit)}
                    className="space-y-5 bg-white shadow rounded-lg p-6"
                >
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Nombre</label>
                        <input
                            {...register("name")}
                            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
                            placeholder="Nombre del propietario"
                        />
                        {errors.name && (
                            <p className="text-red-500 text-sm mt-1">{errors.name.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
                        <input
                            {...register("email")}
                            type="email"
                            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
                            placeholder="Correo electrónico"
                        />
                        {errors.email && (
                            <p className="text-red-500 text-sm mt-1">{errors.email.message}</p>
                        )}
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
