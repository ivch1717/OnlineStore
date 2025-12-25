import { manualRoutes } from './manualRoutes'
import { applyParams, discoverRoutes } from './openapiDiscovery'

const API_BASE = import.meta.env.VITE_API_BASE ?? '' 

async function http<T>(method: string, path: string, body?: unknown): Promise<T> {
  const url = `${API_BASE}${path}`
  const res = await fetch(url, {
    method,
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    },
    body: body ? JSON.stringify(body) : undefined,
  })

  const text = await res.text()
  let data: any = null
  try { data = text ? JSON.parse(text) : null } catch { data = text }

  if (!res.ok) {
    const message = typeof data === 'string' ? data : JSON.stringify(data, null, 2)
    throw new Error(`HTTP ${res.status} ${res.statusText}\n${message}`)
  }
  return data as T
}

type Discovered = Awaited<ReturnType<typeof discoverRoutes>>
let discovered: Discovered | null = null

async function ensureDiscovered() {
  if (discovered) return discovered
  discovered = await discoverRoutes().catch(() => null)
  return discovered
}

function pickRoute(kind: 'orders' | 'accounts', action: string) {
  if (discovered) {
    // @ts-ignore
    const op = discovered?.[kind]?.[action]
    if (op?.path && op?.method) return { method: op.method.toUpperCase(), path: op.path }
  }
  // @ts-ignore
  const fallback = manualRoutes[kind][action]
  return { method: fallback.method, path: fallback.path }
}

export const api = {
  async init() {
    await ensureDiscovered()
  },

  orders: {
    async create(input: { userId: string; amount: number; title?: string }) {
      await ensureDiscovered()
      const r = pickRoute('orders', 'create')
      return http<any>(r.method, r.path, input)
    },
    async status(orderId: string) {
      await ensureDiscovered()
      const r = pickRoute('orders', 'status')
      const path = applyParams(r.path, { id: orderId , orderId})
      return http<any>(r.method, path)
    },
    async byUser(userId: string) {
      await ensureDiscovered()
      const r = pickRoute('orders', 'byUser')
      const path = applyParams(r.path, { userId })
      return http<any>(r.method, path)
    },
  },

  accounts: {
    async create(input: { userId: string; name?: string }) {
      await ensureDiscovered()
      const r = pickRoute('accounts', 'create')
      const path = r.path.includes('{userId}') ? applyParams(r.path, { userId: input.userId }) : r.path
      const body = r.path.includes('{userId}') ? (input.name ? { name: input.name } : undefined) : input
      return http<any>(r.method, path, body)
    },
    async balance(userId: string) {
      await ensureDiscovered()
      const r = pickRoute('accounts', 'balance')
      const path = r.path.includes('{userId}') ? applyParams(r.path, { userId }) : r.path
      return http<any>(r.method, path)
    },
    async topUp(input: { userId: string; amount: number }) {
      await ensureDiscovered()
      const r = pickRoute('accounts', 'topUp')
      const path = r.path.includes('{userId}') ? applyParams(r.path, { userId: input.userId }) : r.path
      const body = r.path.includes('{userId}') ? { amount: input.amount } : input
      return http<any>(r.method, path, body)
    },
  },
}
