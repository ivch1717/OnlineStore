export const manualRoutes = {
  orders: {
    create: { method: 'POST', path: '/orders' },
    status: { method: 'GET', path: '/orders/{id}' },
    byUser: { method: 'GET', path: '/users/{userId}/orders' },
  },
  accounts: {
    create: { method: 'POST', path: '/accounts' },
    balance: { method: 'GET', path: '/accounts/{userId}/balance' },
    topUp: { method: 'POST', path: '/accounts/{userId}/topup' },
  },
} as const

export type HttpMethod = 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE'
