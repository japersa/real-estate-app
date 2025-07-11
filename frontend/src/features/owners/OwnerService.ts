import axios from "../../api/axiosInstance";
import type { OwnerDto } from "../../types/OwnerDto";

const getAll = async (pageNumber = 1, pageSize = 10) => {
    const res = await axios.get("/Owner", {
        params: { pageNumber, pageSize },
    });
    return res.data;
};

const getById = async (id: string) => {
    const res = await axios.get(`/Owner/${id}`);
    return res.data;
};

const create = async (owner: OwnerDto) => {
    const res = await axios.post("/Owner", owner);
    return res.data;
};

const update = async (id: string, owner: OwnerDto) => {
    const res = await axios.put(`/Owner/${id}`, owner);
    return res.data;
};

const remove = async (id: string) => {
    const res = await axios.delete(`/Owner/${id}`);
    return res.data;
};

export default { getAll, getById, create, update, remove };
