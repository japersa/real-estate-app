import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import OwnerService from "./OwnerService";
import type { OwnerDto } from "../../types/OwnerDto";

export const fetchOwners = createAsyncThunk(
    "owners/fetchAll",
    async ({ pageNumber = 1, pageSize = 10 }: { pageNumber?: number; pageSize?: number }) =>
        await OwnerService.getAll(pageNumber, pageSize)
);

export const createOwner = createAsyncThunk(
    "owners/create",
    async (owner: OwnerDto) => await OwnerService.create(owner)
);

export const updateOwner = createAsyncThunk(
    "owners/update",
    async ({ id, owner }: { id: string; owner: OwnerDto }) => await OwnerService.update(id, owner)
);

export const deleteOwner = createAsyncThunk(
    "owners/delete",
    async (id: string) => await OwnerService.remove(id)
);

const ownerSlice = createSlice({
    name: "owners",
    initialState: {
        list: [] as OwnerDto[],
        loading: false,
        error: null as string | null,
    },
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchOwners.pending, (state) => {
                state.loading = true;
            })
            .addCase(fetchOwners.fulfilled, (state, action) => {
                state.list = action.payload.items ?? action.payload;
                state.loading = false;
            })
            .addCase(fetchOwners.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message ?? "Error al obtener propietarios";
            });
    },
});

export default ownerSlice.reducer;
