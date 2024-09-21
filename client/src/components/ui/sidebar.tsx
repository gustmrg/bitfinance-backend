import { Link, matchPath, useLocation } from "react-router-dom";
import { Button } from "./button";
import { Barcode, Home, Receipt, Settings } from "lucide-react";

export function Sidebar() {
  const location = useLocation();

  return (
    <aside
      className={`bg-gray-800 text-white w-64 min-h-screen p-4 block md:block`}
    >
      <nav className="flex flex-col space-y-2">
        <Link to="/">
          <Button
            variant="ghost"
            className={`w-full justify-start ${matchPath({ path: "/", end: true }, location.pathname) ? "bg-accent text-accent-foreground" : ""}`}
          >
            <Home className="mr-2 h-4 w-4" />
            Dashboard
          </Button>
        </Link>
        {/* <Link to="/transactions">
          <Button
            variant="ghost"
            className={`w-full justify-start ${matchPath({ path: "/transactions", end: true }, location.pathname) ? "bg-accent text-accent-foreground" : ""}`}
          >
            <CreditCard className="mr-2 h-4 w-4" />
            Transactions
          </Button>
        </Link> */}
        <Link to="/bills">
          <Button
            variant="ghost"
            className={`w-full justify-start ${matchPath({ path: "/bills", end: true }, location.pathname) ? "bg-accent text-accent-foreground" : ""}`}
          >
            <Barcode className="mr-2 h-4 w-4" />
            Bills
          </Button>
        </Link>
        <Link to="/">
          <Button
            variant="ghost"
            className={`w-full justify-start ${matchPath({ path: "/expenses", end: true }, location.pathname) ? "bg-accent text-accent-foreground" : ""}`}
          >
            <Receipt className="mr-2 h-4 w-4" />
            Expenses
          </Button>
        </Link>
        <Link to="/">
          <Button
            variant="ghost"
            className={`w-full justify-start ${matchPath({ path: "/settings", end: true }, location.pathname) ? "bg-accent text-accent-foreground" : ""}`}
          >
            <Settings className="mr-2 h-4 w-4" />
            Settings
          </Button>
        </Link>
      </nav>
    </aside>
  );
}
