import { createBrowserRouter } from 'react-router-dom'

import { AppLayout } from './pages/app/_layouts/app'
import { Bills } from './pages/app/bills'

export const router = createBrowserRouter([
    {
        path: '/',
        element: <AppLayout />,
        children: [
            { path: '/bills', element: <Bills /> }
        ]
    }
])