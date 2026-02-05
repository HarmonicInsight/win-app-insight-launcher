import React from 'react'
import {
  FolderOpen,
  Mail,
  Calendar,
  Globe,
  Calculator,
  StickyNote,
  Settings,
  HelpCircle
} from 'lucide-react'

interface QuickAction {
  id: string
  label: string
  icon: React.ReactNode
  color: string
  action: () => void
}

export default function QuickActions() {
  const actions: QuickAction[] = [
    {
      id: 'explorer',
      label: 'エクスプローラー',
      icon: <FolderOpen className="w-5 h-5" />,
      color: 'bg-amber-500/20 text-amber-400 hover:bg-amber-500/30',
      action: () => window.electronAPI?.openExplorer()
    },
    {
      id: 'outlook',
      label: 'メール',
      icon: <Mail className="w-5 h-5" />,
      color: 'bg-blue-500/20 text-blue-400 hover:bg-blue-500/30',
      action: () => window.electronAPI?.openExternal('mailto:')
    },
    {
      id: 'calendar',
      label: 'カレンダー',
      icon: <Calendar className="w-5 h-5" />,
      color: 'bg-green-500/20 text-green-400 hover:bg-green-500/30',
      action: () => window.electronAPI?.openExternal('outlookcal:')
    },
    {
      id: 'browser',
      label: 'ブラウザ',
      icon: <Globe className="w-5 h-5" />,
      color: 'bg-purple-500/20 text-purple-400 hover:bg-purple-500/30',
      action: () => window.electronAPI?.openExternal('https://www.google.com')
    },
    {
      id: 'calc',
      label: '電卓',
      icon: <Calculator className="w-5 h-5" />,
      color: 'bg-cyan-500/20 text-cyan-400 hover:bg-cyan-500/30',
      action: () => window.electronAPI?.openExternal('calculator:')
    },
    {
      id: 'notes',
      label: 'メモ帳',
      icon: <StickyNote className="w-5 h-5" />,
      color: 'bg-yellow-500/20 text-yellow-400 hover:bg-yellow-500/30',
      action: () => window.electronAPI?.openExternal('notepad:')
    },
    {
      id: 'settings',
      label: '設定',
      icon: <Settings className="w-5 h-5" />,
      color: 'bg-slate-500/20 text-slate-400 hover:bg-slate-500/30',
      action: () => console.log('Open settings')
    },
    {
      id: 'help',
      label: 'ヘルプ',
      icon: <HelpCircle className="w-5 h-5" />,
      color: 'bg-pink-500/20 text-pink-400 hover:bg-pink-500/30',
      action: () => console.log('Open help')
    }
  ]

  return (
    <div className="card animate-slide-up">
      <h2 className="text-lg font-semibold text-white mb-4">クイックアクション</h2>
      <div className="grid grid-cols-4 gap-2">
        {actions.map((action) => (
          <button
            key={action.id}
            onClick={action.action}
            className={`flex flex-col items-center justify-center p-3 rounded-lg transition-all ${action.color}`}
            title={action.label}
          >
            {action.icon}
            <span className="text-[10px] mt-1 opacity-80">{action.label}</span>
          </button>
        ))}
      </div>
    </div>
  )
}
