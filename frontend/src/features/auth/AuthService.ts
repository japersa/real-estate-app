import axios from "../../api/axiosInstance";
import type { AuthRequest, AuthResponse } from "../../types/AuthDto";

const login = async (credentials: AuthRequest): Promise<AuthResponse> => {
    const res = await axios.post("/Auth/login", credentials);
    return res.data;
};

const register = async (credentials: AuthRequest): Promise<AuthResponse> => {
    const res = await axios.post("/Auth/register", credentials);
    return res.data;
};

export default { login, register };
