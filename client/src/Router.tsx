import { Route, Routes } from "react-router-dom";
import { Register } from "./pages/Register";
import { Login } from "./pages/Login";
import { Bills } from "./pages/Bills";
import { Transactions } from "./pages/Transactions";

export function Router() {
  return (
    <Routes>
      <Route path="register" element={<Register />} />
      <Route path="login" element={<Login />} />
      <Route path="bills" element={<Bills />} />
      <Route path="bills/:id" element={<Bills />} />
      <Route path="transactions" element={<Transactions />} />
    </Routes>
  );
}
