import { RouterProvider } from "react-router-dom"
import { ThemeProvider } from "./components/theme/theme-provider";
import { router } from "./routes";

export function App() {

  return (
    <ThemeProvider defaultTheme="light" storageKey="bitfinance-ui-theme">
      <RouterProvider router={router}/>
    </ThemeProvider>
  )
}