import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom'
import { HomePage } from './pages/HomePage'
import { OrdersPage } from './pages/OrdersPage'
import { AccountsPage } from './pages/AccountsPage'

export function App() {
  return (
    <BrowserRouter basename="/app">
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/orders" element={<OrdersPage />} />
        <Route path="/accounts" element={<AccountsPage />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}
