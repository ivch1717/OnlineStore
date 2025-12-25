import { ReactNode } from 'react'

export function OvalButton(props: {
  children: ReactNode
  onClick?: () => void
  variant?: 'default' | 'primary'
  title?: string
}) {
  const cls = props.variant === 'primary' ? 'pillBtn primary' : 'pillBtn'
  return (
    <button className={cls} onClick={props.onClick} title={props.title}>
      {props.children}
    </button>
  )
}
