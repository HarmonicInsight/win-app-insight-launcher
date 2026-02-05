import React, { useState, useEffect } from 'react'
import { FileSpreadsheet, FileText, Presentation, File, Clock, FolderOpen, RefreshCw } from 'lucide-react'
import type { RecentFile } from '../App'

// デモ用のファイルデータ
const demoFiles: RecentFile[] = [
  { name: '2026年度予算計画.xlsx', path: 'C:\\Users\\Documents\\2026年度予算計画.xlsx', type: 'excel', lastModified: '2026-02-05T09:30:00' },
  { name: '週次報告書_Week5.docx', path: 'C:\\Users\\Documents\\週次報告書_Week5.docx', type: 'word', lastModified: '2026-02-05T08:15:00' },
  { name: 'プロジェクト進捗報告.pptx', path: 'C:\\Users\\Documents\\プロジェクト進捗報告.pptx', type: 'powerpoint', lastModified: '2026-02-04T17:45:00' },
  { name: '売上データ_1月.xlsx', path: 'C:\\Users\\Documents\\売上データ_1月.xlsx', type: 'excel', lastModified: '2026-02-04T14:20:00' },
  { name: '会議議事録_20260204.docx', path: 'C:\\Users\\Documents\\会議議事録_20260204.docx', type: 'word', lastModified: '2026-02-04T11:00:00' },
  { name: '新製品企画書.pptx', path: 'C:\\Users\\Documents\\新製品企画書.pptx', type: 'powerpoint', lastModified: '2026-02-03T16:30:00' },
  { name: '顧客リスト_2026.xlsx', path: 'C:\\Users\\Documents\\顧客リスト_2026.xlsx', type: 'excel', lastModified: '2026-02-03T10:00:00' },
  { name: '業務マニュアル.docx', path: 'C:\\Users\\Documents\\業務マニュアル.docx', type: 'word', lastModified: '2026-02-02T15:00:00' },
]

type FilterType = 'all' | 'excel' | 'word' | 'powerpoint'

export default function RecentFiles() {
  const [files, setFiles] = useState<RecentFile[]>(demoFiles)
  const [filter, setFilter] = useState<FilterType>('all')
  const [isLoading, setIsLoading] = useState(false)

  const loadRecentFiles = async () => {
    setIsLoading(true)
    try {
      if (window.electronAPI) {
        const recentFiles = await window.electronAPI.getRecentFiles()
        if (recentFiles.length > 0) {
          setFiles(recentFiles)
        }
      }
    } catch (err) {
      console.error('Failed to load recent files:', err)
    }
    setIsLoading(false)
  }

  useEffect(() => {
    loadRecentFiles()
  }, [])

  const getFileIcon = (type: RecentFile['type']) => {
    switch (type) {
      case 'excel':
        return <FileSpreadsheet className="w-8 h-8 text-green-500" />
      case 'word':
        return <FileText className="w-8 h-8 text-blue-500" />
      case 'powerpoint':
        return <Presentation className="w-8 h-8 text-orange-500" />
      default:
        return <File className="w-8 h-8 text-slate-400" />
    }
  }

  const formatTime = (dateStr: string) => {
    const date = new Date(dateStr)
    const now = new Date()
    const diffMs = now.getTime() - date.getTime()
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60))
    const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24))

    if (diffHours < 1) return '数分前'
    if (diffHours < 24) return `${diffHours}時間前`
    if (diffDays < 7) return `${diffDays}日前`
    return date.toLocaleDateString('ja-JP')
  }

  const handleOpenFile = async (filePath: string) => {
    if (window.electronAPI) {
      await window.electronAPI.openFile(filePath)
    }
  }

  const filteredFiles = filter === 'all'
    ? files
    : files.filter(f => f.type === filter)

  const filterButtons: { value: FilterType; label: string; color: string }[] = [
    { value: 'all', label: 'すべて', color: 'bg-slate-600' },
    { value: 'excel', label: 'Excel', color: 'bg-green-600' },
    { value: 'word', label: 'Word', color: 'bg-blue-600' },
    { value: 'powerpoint', label: 'PowerPoint', color: 'bg-orange-600' },
  ]

  return (
    <div className="card h-full flex flex-col animate-slide-up">
      {/* ヘッダー */}
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center gap-2">
          <Clock className="w-5 h-5 text-primary-400" />
          <h2 className="text-lg font-semibold text-white">最近使ったファイル</h2>
        </div>
        <button
          onClick={loadRecentFiles}
          className="p-2 text-slate-400 hover:text-white hover:bg-slate-700 rounded-lg transition-colors"
          title="更新"
        >
          <RefreshCw className={`w-4 h-4 ${isLoading ? 'animate-spin' : ''}`} />
        </button>
      </div>

      {/* フィルター */}
      <div className="flex gap-2 mb-4">
        {filterButtons.map((btn) => (
          <button
            key={btn.value}
            onClick={() => setFilter(btn.value)}
            className={`px-3 py-1.5 text-sm rounded-lg transition-colors ${
              filter === btn.value
                ? `${btn.color} text-white`
                : 'bg-slate-700/50 text-slate-400 hover:bg-slate-700'
            }`}
          >
            {btn.label}
          </button>
        ))}
      </div>

      {/* ファイルリスト */}
      <div className="flex-1 overflow-y-auto">
        <div className="grid grid-cols-2 gap-3">
          {filteredFiles.map((file, index) => (
            <button
              key={index}
              onClick={() => handleOpenFile(file.path)}
              className="flex items-center gap-3 p-3 bg-slate-700/30 rounded-lg hover:bg-slate-700/60 transition-all group text-left"
            >
              <div className="flex-shrink-0 p-2 bg-slate-800/50 rounded-lg group-hover:scale-110 transition-transform">
                {getFileIcon(file.type)}
              </div>
              <div className="flex-1 min-w-0">
                <p className="font-medium text-white truncate group-hover:text-primary-300 transition-colors">
                  {file.name}
                </p>
                <p className="text-xs text-slate-500 mt-1">
                  {formatTime(file.lastModified)}
                </p>
              </div>
            </button>
          ))}
        </div>
      </div>

      {/* フッター */}
      <div className="mt-4 pt-4 border-t border-slate-700/50 flex gap-2">
        <button
          onClick={() => window.electronAPI?.openExplorer()}
          className="flex-1 flex items-center justify-center gap-2 py-2 text-sm text-slate-400 hover:text-white hover:bg-slate-700/50 rounded-lg transition-colors"
        >
          <FolderOpen className="w-4 h-4" />
          エクスプローラーを開く
        </button>
      </div>
    </div>
  )
}
