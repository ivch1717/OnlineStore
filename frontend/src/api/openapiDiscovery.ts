type HttpMethod = 'get' | 'post' | 'put' | 'patch' | 'delete'
type OpenApiSpec = any

type DiscoveredOp = {
  method: HttpMethod
  path: string
  operation: any
}

const cache: Record<string, OpenApiSpec | null> = {
  orders: null,
  accounts: null,
}

async function fetchJson(url: string) {
  const res = await fetch(url, { headers: { Accept: 'application/json' } })
  if (!res.ok) throw new Error(`Не получилось загрузить ${url}: ${res.status}`)
  return res.json()
}

export async function loadOpenApi() {
  if (cache.orders && cache.accounts) return cache
  const [orders, accounts] = await Promise.all([
    fetchJson('/orders/openapi.json').catch(() => null),
    fetchJson('/accounts/openapi.json').catch(() => null),
  ])
  cache.orders = orders
  cache.accounts = accounts
  return cache
}

function listOperations(spec: OpenApiSpec): DiscoveredOp[] {
  if (!spec?.paths) return []
  const out: DiscoveredOp[] = []
  for (const [path, methods] of Object.entries(spec.paths)) {
    if (!methods) continue
    for (const [m, op] of Object.entries(methods as Record<string, any>)) {
      const method = m.toLowerCase() as HttpMethod
      if (!['get', 'post', 'put', 'patch', 'delete'].includes(method)) continue
      out.push({ method, path, operation: op })
    }
  }
  return out
}

function normalize(s: string) {
  return (s ?? '').toString().toLowerCase()
}

function scoreOrderCreate(op: DiscoveredOp) {
  let score = 0
  const p = normalize(op.path)
  if (op.method === 'post') score += 3
  if (p.startsWith('/orders')) score += 3
  if (p.includes('create')) score += 1
  if (p === '/orders') score += 2
  return score
}

function scoreOrderStatus(op: DiscoveredOp) {
  let score = 0
  const p = normalize(op.path)
  if (op.method === 'get') score += 3
  if (p.startsWith('/orders/')) score += 3
  if (p.includes('{')) score += 2
  return score
}

function scoreOrdersByUser(op: DiscoveredOp) {
  let score = 0
  const p = normalize(op.path)
  if (op.method === 'get') score += 3
  if (p.includes('/users') && p.includes('orders')) score += 4
  if (p.includes('{userid}') || p.includes('{userId}'.toLowerCase())) score += 2
  return score
}

function scoreAccountCreate(op: DiscoveredOp) {
  let score = 0
  const p = normalize(op.path)
  if (op.method === 'post') score += 3
  if (p.startsWith('/accounts')) score += 3
  if (p === '/accounts') score += 2
  return score
}

function scoreAccountBalance(op: DiscoveredOp) {
  let score = 0
  const p = normalize(op.path)
  if (op.method === 'get') score += 3
  if (p.startsWith('/accounts')) score += 2
  if (p.includes('balance')) score += 3
  if (p.includes('{')) score += 1
  return score
}

function scoreAccountTopUp(op: DiscoveredOp) {
  let score = 0
  const p = normalize(op.path)
  if (['post', 'put', 'patch'].includes(op.method)) score += 3
  if (p.startsWith('/accounts')) score += 2
  if (p.includes('top') || p.includes('deposit') || p.includes('replenish') || p.includes('refill')) score += 4
  return score
}

function pickBest(ops: DiscoveredOp[], scorer: (op: DiscoveredOp) => number): DiscoveredOp | null {
  let best: DiscoveredOp | null = null
  let bestScore = -1
  for (const op of ops) {
    const s = scorer(op)
    if (s > bestScore) {
      best = op
      bestScore = s
    }
  }
  return bestScore <= 0 ? null : best
}

export async function discoverRoutes() {
  const { orders, accounts } = await loadOpenApi()
  const orderOps = listOperations(orders)
  const accountOps = listOperations(accounts)

  return {
    orders: {
      create: pickBest(orderOps, scoreOrderCreate),
      status: pickBest(orderOps, scoreOrderStatus),
      byUser: pickBest(orderOps, scoreOrdersByUser),
    },
    accounts: {
      create: pickBest(accountOps, scoreAccountCreate),
      balance: pickBest(accountOps, scoreAccountBalance),
      topUp: pickBest(accountOps, scoreAccountTopUp),
    },
  }
}

export function applyParams(path: string, params: Record<string, string>) {
  let out = path
  for (const [k, v] of Object.entries(params)) {
    out = out.replaceAll(`{${k}}`, encodeURIComponent(v))
  }
  return out
}
