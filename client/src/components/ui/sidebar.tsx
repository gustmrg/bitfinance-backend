import { Link } from "react-router-dom";
import { Button } from "./button";
import { Barcode, CreditCard, Home, Settings } from "lucide-react";

export function Sidebar() {
  return (
    <aside
      className={`bg-gray-800 text-white w-64 min-h-screen p-4 block md:block`}
    >
      <nav className="space-y-2">
        <Button variant="ghost" className="w-full justify-start">
          <Home className="mr-2 h-4 w-4" />
          Dashboard
        </Button>
        <Link to="/transactions">
          <Button variant="ghost" className="w-full justify-start">
            <CreditCard className="mr-2 h-4 w-4" />
            Transactions
          </Button>
        </Link>
        <Link to="/bills">
          <Button variant="ghost" className="w-full justify-start">
            <Barcode className="mr-2 h-4 w-4" />
            Bills
          </Button>
        </Link>
        <Button variant="ghost" className="w-full justify-start">
          <Settings className="mr-2 h-4 w-4" />
          Settings
        </Button>
      </nav>
    </aside>
  );
}
