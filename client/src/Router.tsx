import { Route, Routes } from "react-router-dom";
import { Register } from "./pages/Register";
import { Login } from "./pages/Login";
import { Bills } from "./pages/Bills";
import { Transactions } from "./pages/Transactions";
import MainLayout from "./layouts/MainLayout";

export function Router() {
  return (
    <Routes>
      <Route path="/" element={<MainLayout />}>
        <Route path="/bills" element={<Bills />} />
        <Route path="/transactions" element={<Transactions />} />
      </Route>
      <Route path="/register" element={<Register />} />
      <Route path="/login" element={<Login />} />
    </Routes>
  );
}
