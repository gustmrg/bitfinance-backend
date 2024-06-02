import { createBrowserRouter } from 'react-router-dom'
import { AppLayout } from './pages/app/_layouts/app'

export const router = createBrowserRouter([
    {
        path: '/',
        element: <AppLayout />
    }
])