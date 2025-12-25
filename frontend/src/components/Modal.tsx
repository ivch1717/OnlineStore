import { ReactNode, useEffect } from 'react'
import { X } from 'lucide-react'

export function Modal(props: {
  open: boolean
  title: string
  description?: string
  children: ReactNode
  onClose: () => void
}) {
  useEffect(() => {
    if (!props.open) return
    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') props.onClose()
    }
    window.addEventListener('keydown', onKeyDown)
    return () => window.removeEventListener('keydown', onKeyDown)
  }, [props.open, props.onClose])

  if (!props.open) return null

  return (
    <div className="modalOverlay" onMouseDown={props.onClose}>
      <div className="modal" onMouseDown={(e) => e.stopPropagation()}>
        <div className="modalHeader">
          <div>
            <div className="modalTitle">{props.title}</div>
            {props.description ? <div className="modalDesc">{props.description}</div> : null}
          </div>
          <button className="pillBtn" onClick={props.onClose} title="Закрыть">
            <X size={18} />
          </button>
        </div>
        {props.children}
      </div>
    </div>
  )
}
