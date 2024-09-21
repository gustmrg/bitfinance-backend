import { Header } from "@/components/ui/header";
import { Sidebar } from "@/components/ui/sidebar";
import { useAuth0 } from "@auth0/auth0-react";
import { Outlet } from "react-router-dom";

export default function DashboardLayout() {
  const { isAuthenticated } = useAuth0();

  return (
    <div className="flex h-screen overflow-hidden">
      <Sidebar />
      <div className="flex-1 flex flex-col overflow-hidden overflow-y-auto">
        <Header isAuthenticated={isAuthenticated} />
        <main className="container mx-auto p-4">
          <Outlet />
        </main>
      </div>
    </div>
  );
}