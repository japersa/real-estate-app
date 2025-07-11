import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import PropertyService from "./PropertyService";
import type { PropertyDto } from "../../types/PropertyDto";

export const fetchProperties = createAsyncThunk(
    "properties/fetchAll",
    async ({ pageNumber = 1, pageSize = 10 }: { pageNumber?: number; pageSize?: number }) =>
        await PropertyService.getAll(pageNumber, pageSize)
);

export const createProperty = createAsyncThunk(
    "properties/create",
    async (property: PropertyDto) => await PropertyService.create(property)
);

export const updateProperty = createAsyncThunk(
    "properties/update",
    async ({ id, property }: { id: string; property: PropertyDto }) =>
        await PropertyService.update(id, property)
);

export const deleteProperty = createAsyncThunk(
    "properties/delete",
    async (id: string) => await PropertyService.remove(id)
);

const propertySlice = createSlice({
    name: "properties",
    initialState: {
        list: [] as PropertyDto[],
        loading: false,
        error: null as string | null,
    },
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchProperties.pending, (state) => {
                state.loading = true;
            })
            .addCase(fetchProperties.fulfilled, (state, action) => {
                state.list = action.payload.items ?? action.payload;
                state.loading = false;
            })
            .addCase(fetchProperties.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message ?? "Error al obtener propiedades";
            });
    },
});

export default propertySlice.reducer;
