import { useEffect, useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { ArrowLeft, Home, Wallet, PlusCircle, Search, CreditCard } from 'lucide-react'
import { OvalButton } from '../components/OvalButton'
import { Modal } from '../components/Modal'
import { api } from '../api/api'
import { isGuid, toNumber } from '../validators'

type Mode = 'create' | 'balance' | 'topup' | null

export function AccountsPage() {
  const nav = useNavigate()
  const [mode, setMode] = useState<Mode>(null)

  const [userId, setUserId] = useState('')
  const [amount, setAmount] = useState('')

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
      case 'create': return 'Создать счёт'
      case 'balance': return 'Посмотреть баланс счёта'
      case 'topup': return 'Пополнить баланс'
      default: return ''
    }
  }, [mode])

  async function submit() {
    resetResult()
    try {
      if (!isGuid(userId)) throw new Error('userId должен быть GUID')

      if (mode === 'create') {
        if (!isGuid(userId)) throw new Error('userId должен быть GUID')
        const data = await api.accounts.create({ userId })
        setResult(JSON.stringify(data, null, 2))
        setResultKind('ok')
      }

      if (mode === 'balance') {
        const data = await api.accounts.balance(userId)
        setResult(JSON.stringify(data, null, 2))
        setResultKind('ok')
      }

      if (mode === 'topup') {
        const n = toNumber(amount)
        if (!Number.isFinite(n) || n <= 0) throw new Error('amount должен быть целым числом > 0')
        const data = await api.accounts.topUp({ userId, amount: n })
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
              <Wallet size={22} /> Аккаунт
            </div>
            <div className="sub">3 действия · каждая кнопка открывает форму</div>
          </div>

          <div className="badge">/accounts</div>
        </div>

        <div className="card">
          <div className="row">
            <OvalButton variant="primary" onClick={() => open('create')}>
              <PlusCircle size={18} /> Создать счёт
            </OvalButton>
            <OvalButton onClick={() => open('balance')}>
              <Search size={18} /> Посмотреть баланс
            </OvalButton>
            <OvalButton onClick={() => open('topup')}>
              <CreditCard size={18} /> Пополнить баланс
            </OvalButton>
          </div>
        </div>

        <Modal open={mode !== null} title={modalTitle} onClose={() => setMode(null)}>
          <div className="form">
            <div className="field">
              <label>Введите id пользователя (GUID)</label>
              <input className="input" value={userId} onChange={(e) => setUserId(e.target.value)} placeholder="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" />
            </div>

            {mode === 'topup' ? (
              <div className="field">
                <label>Введите сумму пополнения (amount)</label>
                <input className="input" value={amount} onChange={(e) => setAmount(e.target.value)} placeholder="например 500" />
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
