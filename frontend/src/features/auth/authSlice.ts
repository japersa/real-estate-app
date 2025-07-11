import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import AuthService from "./AuthService";
import type { AuthRequest, AuthResponse } from "../../types/AuthDto";

export const login = createAsyncThunk(
    "auth/login",
    async (credentials: AuthRequest): Promise<AuthResponse> => {
        return await AuthService.login(credentials);
    }
);

export const register = createAsyncThunk(
    "auth/register",
    async (credentials: AuthRequest): Promise<AuthResponse> => {
        return await AuthService.register(credentials);
    }
);

const authSlice = createSlice({
    name: "auth",
    initialState: {
        user: null as AuthResponse | null,
        loading: false,
        error: null as string | null,
    },
    reducers: {
        logout: (state) => {
            state.user = null;
            localStorage.removeItem("token");
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(login.pending, (state) => {
                state.loading = true;
            })
            .addCase(login.fulfilled, (state, action) => {
                state.user = action.payload;
                state.loading = false;
                localStorage.setItem("token", action.payload.token);
            })
            .addCase(login.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message ?? "Login failed";
            })
            .addCase(register.fulfilled, (state, action) => {
                state.user = action.payload;
                localStorage.setItem("token", action.payload.token);
            });
    },
});

export const { logout } = authSlice.actions;
export default authSlice.reducer;
