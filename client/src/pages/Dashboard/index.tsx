import LoginButton from "@/components/ui/login-button";
import { useAuth0 } from "@auth0/auth0-react";

export function Dashboard() {
  const { user, isAuthenticated } = useAuth0();
  console.log("user", user);
  console.log("isAuthenticated", isAuthenticated);

  return (
    <div>
      <h1 className="text-2xl">Dashboard</h1>
      {!isAuthenticated ? <LoginButton /> : null}
    </div>
  );
}
