import React, { useState } from 'react'
import { ClipboardList, CheckCircle, ChevronRight } from 'lucide-react'

interface SurveyOption {
  id: string
  label: string
}

interface SurveyItem {
  id: string
  question: string
  type: 'single' | 'multiple' | 'text'
  options?: SurveyOption[]
  deadline?: string
  isCompleted?: boolean
}

// デモ用のアンケートデータ
const demoSurveys: SurveyItem[] = [
  {
    id: '1',
    question: '在宅勤務に関するアンケート',
    type: 'single',
    options: [
      { id: 'a', label: '週5日出社希望' },
      { id: 'b', label: '週3日出社希望' },
      { id: 'c', label: '週1日出社希望' },
      { id: 'd', label: 'フルリモート希望' }
    ],
    deadline: '2026-02-10'
  },
  {
    id: '2',
    question: '社内イベントの満足度調査',
    type: 'single',
    deadline: '2026-02-15',
    isCompleted: true
  },
  {
    id: '3',
    question: '新オフィスレイアウトへのご意見',
    type: 'text',
    deadline: '2026-02-20'
  }
]

export default function Survey() {
  const [surveys] = useState<SurveyItem[]>(demoSurveys)
  const [selectedSurvey, setSelectedSurvey] = useState<SurveyItem | null>(null)
  const [selectedOption, setSelectedOption] = useState<string | null>(null)
  const [submitted, setSubmitted] = useState(false)

  const handleSubmit = () => {
    if (selectedOption) {
      setSubmitted(true)
      setTimeout(() => {
        setSelectedSurvey(null)
        setSelectedOption(null)
        setSubmitted(false)
      }, 2000)
    }
  }

  const pendingSurveys = surveys.filter(s => !s.isCompleted)
  const completedCount = surveys.filter(s => s.isCompleted).length

  return (
    <div className="card h-full flex flex-col animate-slide-up">
      {/* ヘッダー */}
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center gap-2">
          <ClipboardList className="w-5 h-5 text-primary-400" />
          <h2 className="text-lg font-semibold text-white">アンケート</h2>
        </div>
        <span className="text-xs text-slate-500">
          {completedCount}/{surveys.length} 回答済
        </span>
      </div>

      {/* アンケートリストまたは回答フォーム */}
      <div className="flex-1 overflow-y-auto">
        {selectedSurvey ? (
          // 回答フォーム
          <div className="space-y-4">
            <button
              onClick={() => setSelectedSurvey(null)}
              className="text-sm text-slate-400 hover:text-white"
            >
              ← 一覧に戻る
            </button>

            <h3 className="font-medium text-white">{selectedSurvey.question}</h3>

            {submitted ? (
              <div className="flex flex-col items-center justify-center py-8 text-green-400">
                <CheckCircle className="w-12 h-12 mb-2" />
                <p>回答を送信しました</p>
              </div>
            ) : (
              <>
                {selectedSurvey.options && (
                  <div className="space-y-2">
                    {selectedSurvey.options.map((option) => (
                      <button
                        key={option.id}
                        onClick={() => setSelectedOption(option.id)}
                        className={`w-full p-3 text-left rounded-lg border transition-colors ${
                          selectedOption === option.id
                            ? 'border-primary-500 bg-primary-500/20 text-white'
                            : 'border-slate-600 bg-slate-700/30 text-slate-300 hover:border-slate-500'
                        }`}
                      >
                        <div className="flex items-center gap-3">
                          <div
                            className={`w-4 h-4 rounded-full border-2 ${
                              selectedOption === option.id
                                ? 'border-primary-500 bg-primary-500'
                                : 'border-slate-500'
                            }`}
                          />
                          {option.label}
                        </div>
                      </button>
                    ))}
                  </div>
                )}

                <button
                  onClick={handleSubmit}
                  disabled={!selectedOption}
                  className={`w-full py-2 rounded-lg font-medium transition-colors ${
                    selectedOption
                      ? 'bg-primary-600 text-white hover:bg-primary-700'
                      : 'bg-slate-700 text-slate-500 cursor-not-allowed'
                  }`}
                >
                  回答を送信
                </button>
              </>
            )}
          </div>
        ) : (
          // アンケート一覧
          <div className="space-y-2">
            {pendingSurveys.length === 0 ? (
              <div className="text-center py-8 text-slate-500">
                <CheckCircle className="w-8 h-8 mx-auto mb-2 text-green-500" />
                <p>すべてのアンケートに回答済みです</p>
              </div>
            ) : (
              pendingSurveys.map((survey) => (
                <button
                  key={survey.id}
                  onClick={() => setSelectedSurvey(survey)}
                  className="w-full p-3 bg-slate-700/30 rounded-lg hover:bg-slate-700/50 transition-colors text-left group"
                >
                  <div className="flex items-center justify-between">
                    <div className="flex-1 min-w-0">
                      <p className="font-medium text-white truncate group-hover:text-primary-300">
                        {survey.question}
                      </p>
                      {survey.deadline && (
                        <p className="text-xs text-slate-500 mt-1">
                          締切: {survey.deadline}
                        </p>
                      )}
                    </div>
                    <ChevronRight className="w-4 h-4 text-slate-500 group-hover:text-white" />
                  </div>
                </button>
              ))
            )}
          </div>
        )}
      </div>
    </div>
  )
}
