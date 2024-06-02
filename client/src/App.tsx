import { ThemeProvider } from "./components/theme/theme-provider";
import { Bills } from "./pages/app/bills";

export function App() {

  return (
    <ThemeProvider defaultTheme="light" storageKey="bitfinance-ui-theme">
      <Bills />
    </ThemeProvider>
  )
}