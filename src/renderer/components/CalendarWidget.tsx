import React, { useState } from 'react'
import { Calendar, ChevronLeft, ChevronRight, ExternalLink, RefreshCw } from 'lucide-react'

interface CalendarEvent {
  id: string
  title: string
  startTime: string
  endTime: string
  source: 'outlook' | 'google'
  location?: string
  isAllDay?: boolean
}

// デモ用の予定データ
const demoEvents: CalendarEvent[] = [
  {
    id: '1',
    title: '週次チームミーティング',
    startTime: '09:00',
    endTime: '10:00',
    source: 'outlook',
    location: '会議室A'
  },
  {
    id: '2',
    title: 'プロジェクト進捗報告',
    startTime: '11:00',
    endTime: '12:00',
    source: 'outlook',
    location: 'Teams'
  },
  {
    id: '3',
    title: '昼食会（新人歓迎）',
    startTime: '12:30',
    endTime: '13:30',
    source: 'google',
    location: '社員食堂'
  },
  {
    id: '4',
    title: 'クライアント打ち合わせ',
    startTime: '14:00',
    endTime: '15:30',
    source: 'outlook',
    location: '外出先'
  },
  {
    id: '5',
    title: '歯医者の予約',
    startTime: '18:00',
    endTime: '19:00',
    source: 'google',
    location: '〇〇歯科'
  }
]

interface CalendarConnection {
  type: 'outlook' | 'google'
  name: string
  isConnected: boolean
  color: string
}

export default function CalendarWidget() {
  const [events] = useState<CalendarEvent[]>(demoEvents)
  const [isLoading, setIsLoading] = useState(false)
  const [connections, setConnections] = useState<CalendarConnection[]>([
    { type: 'outlook', name: 'Outlook', isConnected: true, color: 'bg-blue-500' },
    { type: 'google', name: 'Google', isConnected: true, color: 'bg-red-500' }
  ])
  const [selectedDate, setSelectedDate] = useState(new Date())

  const formatDate = (date: Date) => {
    return date.toLocaleDateString('ja-JP', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      weekday: 'short'
    })
  }

  const changeDate = (days: number) => {
    const newDate = new Date(selectedDate)
    newDate.setDate(newDate.getDate() + days)
    setSelectedDate(newDate)
  }

  const handleConnect = async (type: 'outlook' | 'google') => {
    // 実際の実装ではOAuth認証フローを開始
    if (type === 'outlook') {
      // Microsoft Graph API OAuth
      window.electronAPI?.openExternal('https://login.microsoftonline.com/common/oauth2/v2.0/authorize')
    } else {
      // Google Calendar API OAuth
      window.electronAPI?.openExternal('https://accounts.google.com/o/oauth2/v2/auth')
    }
  }

  const handleRefresh = async () => {
    setIsLoading(true)
    // 実際の実装ではAPIから予定を再取得
    setTimeout(() => setIsLoading(false), 1000)
  }

  const getSourceBadge = (source: CalendarEvent['source']) => {
    const conn = connections.find(c => c.type === source)
    return (
      <span className={`px-1.5 py-0.5 text-[10px] rounded ${conn?.color} text-white`}>
        {conn?.name}
      </span>
    )
  }

  const isToday = selectedDate.toDateString() === new Date().toDateString()

  return (
    <div className="card h-full flex flex-col animate-slide-up">
      {/* ヘッダー */}
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center gap-2">
          <Calendar className="w-5 h-5 text-primary-400" />
          <h2 className="text-lg font-semibold text-white">カレンダー</h2>
        </div>
        <button
          onClick={handleRefresh}
          className="p-2 text-slate-400 hover:text-white hover:bg-slate-700 rounded-lg transition-colors"
          title="更新"
        >
          <RefreshCw className={`w-4 h-4 ${isLoading ? 'animate-spin' : ''}`} />
        </button>
      </div>

      {/* 連携状態 */}
      <div className="flex gap-2 mb-4">
        {connections.map((conn) => (
          <div
            key={conn.type}
            className={`flex items-center gap-1.5 px-2 py-1 rounded text-xs ${
              conn.isConnected
                ? 'bg-slate-700/50 text-slate-300'
                : 'bg-slate-800 text-slate-500'
            }`}
          >
            <span className={`w-2 h-2 rounded-full ${conn.isConnected ? conn.color : 'bg-slate-600'}`} />
            {conn.name}
            {!conn.isConnected && (
              <button
                onClick={() => handleConnect(conn.type)}
                className="text-primary-400 hover:text-primary-300"
              >
                連携
              </button>
            )}
          </div>
        ))}
      </div>

      {/* 日付ナビゲーション */}
      <div className="flex items-center justify-between mb-4 pb-3 border-b border-slate-700/50">
        <button
          onClick={() => changeDate(-1)}
          className="p-1 text-slate-400 hover:text-white hover:bg-slate-700 rounded transition-colors"
        >
          <ChevronLeft className="w-5 h-5" />
        </button>
        <div className="text-center">
          <p className="text-white font-medium">{formatDate(selectedDate)}</p>
          {isToday && (
            <p className="text-xs text-primary-400">今日</p>
          )}
        </div>
        <button
          onClick={() => changeDate(1)}
          className="p-1 text-slate-400 hover:text-white hover:bg-slate-700 rounded transition-colors"
        >
          <ChevronRight className="w-5 h-5" />
        </button>
      </div>

      {/* 予定リスト */}
      <div className="flex-1 overflow-y-auto space-y-2">
        {events.length === 0 ? (
          <div className="text-center py-8 text-slate-500">
            <Calendar className="w-8 h-8 mx-auto mb-2 opacity-50" />
            <p>予定はありません</p>
          </div>
        ) : (
          events.map((event) => (
            <div
              key={event.id}
              className="p-3 bg-slate-700/30 rounded-lg hover:bg-slate-700/50 transition-colors"
            >
              <div className="flex items-start justify-between gap-2">
                <div className="flex-1 min-w-0">
                  <div className="flex items-center gap-2 mb-1">
                    <span className="text-xs text-slate-400">
                      {event.startTime} - {event.endTime}
                    </span>
                    {getSourceBadge(event.source)}
                  </div>
                  <p className="font-medium text-white truncate">{event.title}</p>
                  {event.location && (
                    <p className="text-xs text-slate-500 mt-1">{event.location}</p>
                  )}
                </div>
              </div>
            </div>
          ))
        )}
      </div>

      {/* フッター */}
      <div className="mt-4 pt-3 border-t border-slate-700/50 flex gap-2">
        <button
          onClick={() => window.electronAPI?.openExternal('https://outlook.office.com/calendar')}
          className="flex-1 flex items-center justify-center gap-1 py-1.5 text-xs text-blue-400 hover:bg-blue-500/10 rounded transition-colors"
        >
          <ExternalLink className="w-3 h-3" />
          Outlook
        </button>
        <button
          onClick={() => window.electronAPI?.openExternal('https://calendar.google.com')}
          className="flex-1 flex items-center justify-center gap-1 py-1.5 text-xs text-red-400 hover:bg-red-500/10 rounded transition-colors"
        >
          <ExternalLink className="w-3 h-3" />
          Google
        </button>
      </div>
    </div>
  )
}
