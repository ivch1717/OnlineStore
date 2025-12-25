const guidRe =
  /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/

export function isGuid(value: string) {
  return guidRe.test(value.trim())
}

export function toNumber(value: string) {
  const s = value.trim()
  if (!/^[0-9]+$/.test(s)) return NaN
  const n = Number.parseInt(s, 10)
  return Number.isFinite(n) ? n : NaN
}
