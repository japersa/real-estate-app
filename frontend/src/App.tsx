import { Route, Routes } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Owners from "./pages/Owners";
import OwnerForm from "./pages/OwnerForm";
import Properties from "./pages/Properties";
import PropertyForm from "./pages/PropertyForm";
import ProtectedRoute from "./components/ProtectedRoute";

export default function App() {
  return (
    <>
    <Routes>
      <Route path="/" element={<Login />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />

      <Route
        path="/owners"
        element={
          <ProtectedRoute>
            <Owners />
          </ProtectedRoute>
        }
      />
      <Route
        path="/owners/create"
        element={
          <ProtectedRoute>
            <OwnerForm />
          </ProtectedRoute>
        }
      />
      <Route
        path="/owners/edit/:id"
        element={
          <ProtectedRoute>
            <OwnerForm />
          </ProtectedRoute>
        }
      />

      <Route
        path="/properties"
        element={
          <ProtectedRoute>
            <Properties />
          </ProtectedRoute>
        }
      />
      <Route
        path="/properties/create"
        element={
          <ProtectedRoute>
            <PropertyForm />
          </ProtectedRoute>
        }
      />
      <Route
        path="/properties/edit/:id"
        element={
          <ProtectedRoute>
            <PropertyForm />
          </ProtectedRoute>
        }
      />
    </Routes>
    </>
  );
}
