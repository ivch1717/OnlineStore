import { useEffect, useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { ArrowLeft, Home, Package, PlusCircle, Search, List } from 'lucide-react'
import { OvalButton } from '../components/OvalButton'
import { Modal } from '../components/Modal'
import { api } from '../api/api'
import { isGuid, toNumber } from '../validators'

type Mode = 'create' | 'status' | 'byUser' | null

export function OrdersPage() {
  const nav = useNavigate()
  const [mode, setMode] = useState<Mode>(null)

  const [userId, setUserId] = useState('')
  const [orderId, setOrderId] = useState('')
  const [amount, setAmount] = useState('')
  const [description, setDescription] = useState('')

  const [result, setResult] = useState<string>('')
  const [resultKind, setResultKind] = useState<'ok' | 'error' | ''>('')

  useEffect(() => {
    api.init().catch(() => {})
  }, [])

  function resetResult() {
    setResult('')
    setResultKind('')
  }

  function open(m: Mode) {
    resetResult()
    setMode(m)
  }

  const modalTitle = useMemo(() => {
    switch (mode) {
      case 'create': return 'Сделать заказ'
      case 'status': return 'Посмотреть статус заказа'
      case 'byUser': return 'Посмотреть все заказы пользователя'
      default: return ''
    }
  }, [mode])

  async function submit() {
    resetResult()
    try {
      if (mode === 'create') {
        if (!isGuid(userId)) throw new Error('userId должен быть GUID')
        const n = toNumber(amount)
        if (!Number.isFinite(n) || n <= 0) throw new Error('amount должен быть целым числом > 0')
        if (!description.trim()) throw new Error('Введите описание заказа')

        const data = await api.orders.create({ userId, amount: n, description: description.trim() })
        setResult(JSON.stringify(data, null, 2))
        setResultKind('ok')
      }

      if (mode === 'status') {
        if (!isGuid(orderId)) throw new Error('orderId должен быть GUID')
        const data = await api.orders.status(orderId)
        setResult(JSON.stringify(data, null, 2))
        setResultKind('ok')
      }

      if (mode === 'byUser') {
        if (!isGuid(userId)) throw new Error('userId должен быть GUID')
        const data = await api.orders.byUser(userId)
        setResult(JSON.stringify(data, null, 2))
        setResultKind('ok')
      }
    } catch (e: any) {
      setResult(e?.message ?? String(e))
      setResultKind('error')
    }
  }

  return (
    <div className="container">
      <div className="shell">
        <div className="header">
          <div className="topLeft">
            <button className="pillBtn" onClick={() => nav(-1)} title="Назад">
              <ArrowLeft size={18} /> Назад
            </button>
            <button className="pillBtn" onClick={() => nav('/')} title="На главную">
              <Home size={18} />
            </button>
          </div>

          <div>
            <div className="title" style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
              <Package size={22} /> Заказы
            </div>
            <div className="sub">3 действия · каждая кнопка открывает форму</div>
          </div>

          <div className="badge">/orders · /users</div>
        </div>

        <div className="card">
          <div className="row">
            <OvalButton variant="primary" onClick={() => open('create')}>
              <PlusCircle size={18} /> Сделать заказ
            </OvalButton>
            <OvalButton onClick={() => open('status')}>
              <Search size={18} /> Посмотреть статус заказа
            </OvalButton>
            <OvalButton onClick={() => open('byUser')}>
              <List size={18} /> Посмотреть все заказы
            </OvalButton>
          </div>
        </div>

        <Modal open={mode !== null} title={modalTitle} onClose={() => setMode(null)}>
          <div className="form">
            {mode === 'create' ? (
              <>
                <div className="twoCols">
                  <div className="field">
                    <label>Введите id пользователя (GUID)</label>
                    <input className="input" value={userId} onChange={(e) => setUserId(e.target.value)} placeholder="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" />
                  </div>
                  <div className="field">
                    <label>Введите цену / сумму (amount)</label>
                    <input className="input" value={amount} onChange={(e) => setAmount(e.target.value)} placeholder="например 199" />
                  </div>
                </div>
                <div className="field">
                  <label>Введите описание заказа</label>
                  <input className="input" value={description} onChange={(e) => setDescription(e.target.value)} placeholder="например: Заказ плитки" />
                </div>
              </>
            ) : null}

            {mode === 'status' ? (
              <div className="field">
                <label>Введите id заказа (GUID)</label>
                <input className="input" value={orderId} onChange={(e) => setOrderId(e.target.value)} placeholder="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" />
              </div>
            ) : null}

            {mode === 'byUser' ? (
              <div className="field">
                <label>Введите id пользователя (GUID)</label>
                <input className="input" value={userId} onChange={(e) => setUserId(e.target.value)} placeholder="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" />
              </div>
            ) : null}

            <div className="footerRow">
              <OvalButton onClick={() => setMode(null)}>Закрыть</OvalButton>
              <OvalButton variant="primary" onClick={submit}>Отправить</OvalButton>
            </div>

            {result ? <div className={`alert ${resultKind === 'error' ? 'error' : resultKind === 'ok' ? 'ok' : ''}`}>{result}</div> : null}
          </div>
        </Modal>
      </div>
    </div>
  )
}
