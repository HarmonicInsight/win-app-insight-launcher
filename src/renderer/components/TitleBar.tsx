import React from 'react'
import { Minus, Square, X, Rocket } from 'lucide-react'

export default function TitleBar() {
  const handleMinimize = () => window.electronAPI?.windowMinimize()
  const handleMaximize = () => window.electronAPI?.windowMaximize()
  const handleClose = () => window.electronAPI?.windowClose()

  return (
    <header className="titlebar-drag flex items-center justify-between h-10 px-4 bg-slate-900/80 border-b border-slate-700/50">
      {/* アプリロゴ */}
      <div className="flex items-center gap-2 text-slate-300">
        <Rocket className="w-5 h-5 text-primary-400" />
        <span className="font-semibold">Insight Launcher</span>
      </div>

      {/* ウィンドウコントロール */}
      <div className="titlebar-no-drag flex items-center">
        <button
          onClick={handleMinimize}
          className="w-10 h-10 flex items-center justify-center text-slate-400 hover:bg-slate-700 transition-colors"
          title="最小化"
        >
          <Minus className="w-4 h-4" />
        </button>
        <button
          onClick={handleMaximize}
          className="w-10 h-10 flex items-center justify-center text-slate-400 hover:bg-slate-700 transition-colors"
          title="最大化"
        >
          <Square className="w-3.5 h-3.5" />
        </button>
        <button
          onClick={handleClose}
          className="w-10 h-10 flex items-center justify-center text-slate-400 hover:bg-red-600 hover:text-white transition-colors"
          title="閉じる"
        >
          <X className="w-4 h-4" />
        </button>
      </div>
    </header>
  )
}
