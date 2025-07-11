import axios from "../../api/axiosInstance";
import type { PropertyDto } from "../../types/PropertyDto";

const getAll = async (pageNumber = 1, pageSize = 10) => {
    const res = await axios.get("/Properties", {
        params: { pageNumber, pageSize },
    });
    return res.data;
};

const getById = async (id: string) => {
    const res = await axios.get(`/Properties/${id}`);
    return res.data;
};

const create = async (property: PropertyDto) => {
    const res = await axios.post("/Properties", property);
    return res.data;
};

const update = async (id: string, property: PropertyDto) => {
    const res = await axios.put(`/Properties/${id}`, property);
    return res.data;
};

const remove = async (id: string) => {
    const res = await axios.delete(`/Properties/${id}`);
    return res.data;
};

export default { getAll, getById, create, update, remove };
