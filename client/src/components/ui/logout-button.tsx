import { useAuth0 } from "@auth0/auth0-react";
import { DropdownMenuItem } from "./dropdown-menu";

const LogoutButton = () => {
  const { logout } = useAuth0();

  return (
    <DropdownMenuItem
      onClick={() =>
        logout({ logoutParams: { returnTo: window.location.origin } })
      }
    >
      Log Out
    </DropdownMenuItem>
  );
};

export default LogoutButton;
