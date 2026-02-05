import React from 'react'
import { Bell, AlertTriangle, Info, Calendar, ChevronRight } from 'lucide-react'

interface Announcement {
  id: string
  type: 'important' | 'info' | 'schedule'
  title: string
  content: string
  date: string
  isNew?: boolean
}

// デモ用のお知らせデータ
const demoAnnouncements: Announcement[] = [
  {
    id: '1',
    type: 'important',
    title: '全社セキュリティ研修のお知らせ',
    content: '2月10日(月)に全社セキュリティ研修があります。必ず参加してください。',
    date: '2026-02-05',
    isNew: true
  },
  {
    id: '2',
    type: 'schedule',
    title: '月例会議の日程変更',
    content: '今月の月例会議は2月15日(土)から2月18日(火)に変更になりました。',
    date: '2026-02-04',
    isNew: true
  },
  {
    id: '3',
    type: 'info',
    title: 'システムメンテナンスのお知らせ',
    content: '2月8日(土) 深夜1時〜5時にシステムメンテナンスを実施します。',
    date: '2026-02-03'
  },
  {
    id: '4',
    type: 'info',
    title: '新入社員歓迎会について',
    content: '4月入社の新入社員歓迎会を企画中です。参加希望の方は人事部まで。',
    date: '2026-02-01'
  }
]

export default function Announcements() {
  const getIcon = (type: Announcement['type']) => {
    switch (type) {
      case 'important':
        return <AlertTriangle className="w-5 h-5 text-amber-400" />
      case 'schedule':
        return <Calendar className="w-5 h-5 text-blue-400" />
      default:
        return <Info className="w-5 h-5 text-slate-400" />
    }
  }

  const getBorderColor = (type: Announcement['type']) => {
    switch (type) {
      case 'important':
        return 'border-l-amber-400'
      case 'schedule':
        return 'border-l-blue-400'
      default:
        return 'border-l-slate-500'
    }
  }

  return (
    <div className="card h-full flex flex-col animate-slide-up">
      {/* ヘッダー */}
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center gap-2">
          <Bell className="w-5 h-5 text-primary-400" />
          <h2 className="text-lg font-semibold text-white">お知らせ</h2>
        </div>
        <span className="text-xs text-slate-500">管理者からのメッセージ</span>
      </div>

      {/* お知らせリスト */}
      <div className="flex-1 overflow-y-auto space-y-3">
        {demoAnnouncements.map((announcement) => (
          <div
            key={announcement.id}
            className={`p-3 bg-slate-700/30 rounded-lg border-l-4 ${getBorderColor(announcement.type)} hover:bg-slate-700/50 transition-colors cursor-pointer`}
          >
            <div className="flex items-start gap-3">
              <div className="mt-0.5">{getIcon(announcement.type)}</div>
              <div className="flex-1 min-w-0">
                <div className="flex items-center gap-2">
                  <h3 className="font-medium text-white truncate">
                    {announcement.title}
                  </h3>
                  {announcement.isNew && (
                    <span className="px-1.5 py-0.5 text-[10px] font-bold bg-red-500 text-white rounded">
                      NEW
                    </span>
                  )}
                </div>
                <p className="text-sm text-slate-400 mt-1 line-clamp-2">
                  {announcement.content}
                </p>
                <p className="text-xs text-slate-500 mt-2">{announcement.date}</p>
              </div>
              <ChevronRight className="w-4 h-4 text-slate-500 flex-shrink-0" />
            </div>
          </div>
        ))}
      </div>

      {/* もっと見る */}
      <button className="mt-4 w-full py-2 text-sm text-primary-400 hover:text-primary-300 hover:bg-slate-700/30 rounded-lg transition-colors">
        すべてのお知らせを見る
      </button>
    </div>
  )
}
