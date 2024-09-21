import { Route, Routes } from "react-router-dom";
import { Register } from "./pages/Register";
import { Login } from "./pages/Login";
import { Bills } from "./pages/Bills/page";
import { Transactions } from "./pages/Transactions/page";
import DashboardLayout from "./layouts/dashboard-layout";
import { Dashboard } from "./pages/Dashboard/page";
import Profile from "./pages/Profile/page";

export function Router() {
  return (
    <Routes>
      <Route path="/" element={<DashboardLayout />}>
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/bills" element={<Bills />} />
        <Route path="/transactions" element={<Transactions />} />
        <Route path="/profile" element={<Profile />} />
      </Route>
      <Route path="/register" element={<Register />} />
      <Route path="/login" element={<Login />} />
    </Routes>
  );
}
