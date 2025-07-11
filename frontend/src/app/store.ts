import { configureStore } from "@reduxjs/toolkit";
import authReducer from "../features/auth/authSlice";
import ownerReducer from "../features/owners/ownerSlice";
import propertyReducer from "../features/properties/propertySlice";

export const store = configureStore({
    reducer: {
        auth: authReducer,
        owners: ownerReducer,
        properties: propertyReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
