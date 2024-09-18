import { Bell, Menu } from "lucide-react";
import { Button } from "./button";
import { Avatar, AvatarFallback, AvatarImage } from "./avatar";

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
          <Button variant="ghost" size="icon">
            <Avatar>
              <AvatarImage
                src="https://github.com/gustmrg.png"
                alt="@gustmrg"
              />
              <AvatarFallback>GM</AvatarFallback>
            </Avatar>
          </Button>
        </div>
      </div>
    </header>
  );
}
