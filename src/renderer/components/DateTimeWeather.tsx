import React, { useState, useEffect } from 'react'
import { Cloud, Sun, CloudRain, CloudSnow, Wind, Droplets } from 'lucide-react'

interface WeatherData {
  temp: number
  description: string
  icon: string
  humidity: number
  windSpeed: number
}

const DAYS = ['日', '月', '火', '水', '木', '金', '土']

export default function DateTimeWeather() {
  const [currentTime, setCurrentTime] = useState(new Date())
  const [weather, setWeather] = useState<WeatherData | null>(null)

  useEffect(() => {
    const timer = setInterval(() => {
      setCurrentTime(new Date())
    }, 1000)

    // デモ用の天気データ（実際はAPIから取得）
    setWeather({
      temp: 12,
      description: '晴れ',
      icon: '01d',
      humidity: 45,
      windSpeed: 3.2
    })

    return () => clearInterval(timer)
  }, [])

  const formatDate = (date: Date) => {
    const year = date.getFullYear()
    const month = date.getMonth() + 1
    const day = date.getDate()
    const dayOfWeek = DAYS[date.getDay()]
    return `${year}年${month}月${day}日 (${dayOfWeek})`
  }

  const formatTime = (date: Date) => {
    return date.toLocaleTimeString('ja-JP', {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    })
  }

  const getWeatherIcon = (icon: string) => {
    if (icon.includes('01') || icon.includes('02')) return <Sun className="w-12 h-12 text-yellow-400" />
    if (icon.includes('09') || icon.includes('10')) return <CloudRain className="w-12 h-12 text-blue-400" />
    if (icon.includes('13')) return <CloudSnow className="w-12 h-12 text-blue-200" />
    return <Cloud className="w-12 h-12 text-slate-400" />
  }

  return (
    <div className="card animate-fade-in">
      {/* 日付・時刻 */}
      <div className="text-center mb-4">
        <p className="text-slate-400 text-sm">{formatDate(currentTime)}</p>
        <p className="text-4xl font-light text-white tracking-wider mt-1">
          {formatTime(currentTime)}
        </p>
      </div>

      {/* 天気情報 */}
      {weather && (
        <div className="flex items-center justify-between pt-4 border-t border-slate-700/50">
          <div className="flex items-center gap-3">
            {getWeatherIcon(weather.icon)}
            <div>
              <p className="text-3xl font-semibold text-white">{weather.temp}°C</p>
              <p className="text-slate-400">{weather.description}</p>
            </div>
          </div>
          <div className="text-right text-sm text-slate-400 space-y-1">
            <div className="flex items-center gap-1 justify-end">
              <Droplets className="w-4 h-4" />
              <span>{weather.humidity}%</span>
            </div>
            <div className="flex items-center gap-1 justify-end">
              <Wind className="w-4 h-4" />
              <span>{weather.windSpeed}m/s</span>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}
