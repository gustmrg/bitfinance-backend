import { createBrowserRouter } from 'react-router-dom'

import { AppLayout } from './pages/app/_layouts/app'
import { Bills } from './pages/app/bills'
import { Dashboard } from './pages/app/dashboard'

export const router = createBrowserRouter([
    {
        path: '/',
        element: <AppLayout />,
        children: [
            { path: '/', element: <Dashboard /> },
            { path: '/bills', element: <Bills /> },
        ]
    }
])