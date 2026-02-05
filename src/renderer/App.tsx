import React from 'react'
import TitleBar from './components/TitleBar'
import DateTimeWeather from './components/DateTimeWeather'
import CalendarWidget from './components/CalendarWidget'
import Announcements from './components/Announcements'
import RecentFiles from './components/RecentFiles'
import QuickActions from './components/QuickActions'
import Survey from './components/Survey'

declare global {
  interface Window {
    electronAPI: {
      getRecentFiles: () => Promise<RecentFile[]>
      openFile: (filePath: string) => Promise<{ success: boolean; error?: string }>
      openExternal: (url: string) => Promise<{ success: boolean; error?: string }>
      openExplorer: (dirPath?: string) => Promise<{ success: boolean; error?: string }>
      setAutoLaunch: (enable: boolean) => Promise<{ success: boolean; error?: string }>
      windowMinimize: () => void
      windowMaximize: () => void
      windowClose: () => void
    }
  }
}

export interface RecentFile {
  name: string
  path: string
  type: 'excel' | 'word' | 'powerpoint' | 'other'
  lastModified: string
}

export default function App() {
  return (
    <div className="h-screen flex flex-col bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900">
      {/* タイトルバー */}
      <TitleBar />

      {/* メインコンテンツ */}
      <main className="flex-1 p-6 overflow-hidden">
        <div className="h-full grid grid-cols-12 gap-6">
          {/* 左カラム - 日付/天気 + カレンダー */}
          <div className="col-span-3 flex flex-col gap-6">
            <DateTimeWeather />
            <div className="flex-1 overflow-hidden">
              <CalendarWidget />
            </div>
          </div>

          {/* 中央カラム - 最近使ったファイル */}
          <div className="col-span-5 flex flex-col gap-6">
            <RecentFiles />
          </div>

          {/* 右カラム - お知らせ + クイックアクション + アンケート */}
          <div className="col-span-4 flex flex-col gap-6">
            <QuickActions />
            <div className="flex-1 overflow-hidden">
              <Announcements />
            </div>
            <Survey />
          </div>
        </div>
      </main>

      {/* フッター */}
      <footer className="px-6 py-3 text-center text-slate-500 text-xs border-t border-slate-700/50">
        Insight Launcher v1.0.0 - Your Work Portal
      </footer>
    </div>
  )
}
