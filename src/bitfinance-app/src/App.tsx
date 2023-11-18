import { BrowserRouter } from 'react-router-dom';
import { Router } from './Router';
import { MantineProvider } from '@mantine/core'

import '@mantine/core/styles.css'

export default function App() {
  return (
    <MantineProvider>
      <BrowserRouter>
        <Router />
      </BrowserRouter>
    </MantineProvider>
  )
}
