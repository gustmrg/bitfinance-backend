import { Route, Routes } from 'react-router-dom';
import { DashboardLayout } from './layouts/DashboardLayout';
import { Bills } from './pages/Bills/Bills';

export function Router() {
    return (
        <Routes>
            <Route path="/" element={<DashboardLayout />}>
                <Route path="/" element={<Bills />}></Route>
            </Route>
        </Routes>
    )
}