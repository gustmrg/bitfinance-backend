import { Outlet } from "react-router-dom";

export default function MainLayout() {
  return (
    <div className="min-h-screen bg-background text-foreground">
      <main className="container mx-auto p-4">
        <Outlet />
      </main>
    </div>
  );
}
