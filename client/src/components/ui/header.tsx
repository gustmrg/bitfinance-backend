import { Bell, ChevronDown, Menu } from "lucide-react";
import { Avatar, AvatarFallback, AvatarImage } from "./avatar";
import { Button } from "./button";

export function Header() {
  return (
    <header className="bg-white border-b border-gray-200 p-4">
      <div className="flex items-center justify-between">
        <div className="flex items-center">
          <Button variant="ghost" size="icon" className="md:hidden mr-2">
            <Menu className="h-6 w-6" />
          </Button>
        </div>
        <div className="flex items-center space-x-4">
          <Button variant="ghost" size="icon">
            <Bell className="h-5 w-5" />
          </Button>
          <Button variant="ghost" className="flex flex-row gap-2">
            <Avatar className="h-8 w-8">
              <AvatarImage src="/avatars/02.png" alt="@gustmrg" />
              <AvatarFallback>TC</AvatarFallback>
            </Avatar>
            Tom Cook
            <span>
              <ChevronDown />
            </span>
          </Button>
        </div>
      </div>
    </header>
  );
}
