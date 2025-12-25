import { useNavigate } from 'react-router-dom'
import { ChevronRight, Package, Wallet } from 'lucide-react'

export function HomePage() {
  const nav = useNavigate()

  return (
    <div className="container">
      <div className="shell">
        <div className="header">
          <div>
            <div className="title">OnlineStore</div>
</div>
</div>

        <div className="grid2">
          <button className="bigChoice" onClick={() => nav('/orders')}>
            <div>
              <div className="label" style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                <Package size={22} /> Заказы
              </div>
              <div className="hint">Сделать заказ · статус · заказы пользователя</div>
            </div>
            <ChevronRight />
          </button>

          <button className="bigChoice" onClick={() => nav('/accounts')}>
            <div>
              <div className="label" style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                <Wallet size={22} /> Аккаунт
              </div>
              <div className="hint">Создать счёт · баланс · пополнить</div>
            </div>
            <ChevronRight />
          </button>
        </div>
</div>
    </div>
  )
}
