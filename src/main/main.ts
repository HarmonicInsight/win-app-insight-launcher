import { app, BrowserWindow, ipcMain, shell } from 'electron'
import * as path from 'path'
import * as fs from 'fs'

let mainWindow: BrowserWindow | null = null

const isDev = process.env.NODE_ENV === 'development' || !app.isPackaged

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    minWidth: 900,
    minHeight: 600,
    frame: false,
    transparent: false,
    backgroundColor: '#1e293b',
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true,
      preload: path.join(__dirname, 'preload.js')
    },
    show: false
  })

  // 起動時に全面表示
  mainWindow.once('ready-to-show', () => {
    mainWindow?.show()
    mainWindow?.focus()
  })

  if (isDev) {
    mainWindow.loadURL('http://localhost:5173')
    mainWindow.webContents.openDevTools()
  } else {
    mainWindow.loadFile(path.join(__dirname, '../renderer/index.html'))
  }

  mainWindow.on('closed', () => {
    mainWindow = null
  })
}

// 最近使ったファイルを取得（Windows）
function getRecentFiles(): RecentFile[] {
  const recentFiles: RecentFile[] = []

  if (process.platform === 'win32') {
    const recentPath = path.join(
      process.env.APPDATA || '',
      'Microsoft/Windows/Recent'
    )

    try {
      if (fs.existsSync(recentPath)) {
        const files = fs.readdirSync(recentPath)
          .filter(f => f.endsWith('.lnk'))
          .slice(0, 20)

        files.forEach(file => {
          const fullPath = path.join(recentPath, file)
          const stats = fs.statSync(fullPath)
          const name = file.replace('.lnk', '')

          // ファイル種別を判定
          let type: 'excel' | 'word' | 'powerpoint' | 'other' = 'other'
          if (name.match(/\.xlsx?$/i)) type = 'excel'
          else if (name.match(/\.docx?$/i)) type = 'word'
          else if (name.match(/\.pptx?$/i)) type = 'powerpoint'

          recentFiles.push({
            name,
            path: fullPath,
            type,
            lastModified: stats.mtime.toISOString()
          })
        })
      }
    } catch (err) {
      console.error('Error reading recent files:', err)
    }
  }

  return recentFiles.sort((a, b) =>
    new Date(b.lastModified).getTime() - new Date(a.lastModified).getTime()
  )
}

interface RecentFile {
  name: string
  path: string
  type: 'excel' | 'word' | 'powerpoint' | 'other'
  lastModified: string
}

// IPC ハンドラー
ipcMain.handle('get-recent-files', () => {
  return getRecentFiles()
})

ipcMain.handle('open-file', async (_, filePath: string) => {
  try {
    await shell.openPath(filePath)
    return { success: true }
  } catch (err) {
    return { success: false, error: String(err) }
  }
})

ipcMain.handle('open-external', async (_, url: string) => {
  try {
    await shell.openExternal(url)
    return { success: true }
  } catch (err) {
    return { success: false, error: String(err) }
  }
})

ipcMain.handle('open-explorer', async (_, dirPath?: string) => {
  try {
    const targetPath = dirPath || process.env.USERPROFILE || 'C:\\'
    await shell.openPath(targetPath)
    return { success: true }
  } catch (err) {
    return { success: false, error: String(err) }
  }
})

// ウィンドウ操作
ipcMain.on('window-minimize', () => mainWindow?.minimize())
ipcMain.on('window-maximize', () => {
  if (mainWindow?.isMaximized()) {
    mainWindow.unmaximize()
  } else {
    mainWindow?.maximize()
  }
})
ipcMain.on('window-close', () => mainWindow?.close())

// スタートアップ登録
ipcMain.handle('set-auto-launch', async (_, enable: boolean) => {
  if (process.platform === 'win32') {
    const appPath = app.getPath('exe')
    const AutoLaunch = require('auto-launch')
    const autoLauncher = new AutoLaunch({
      name: 'Insight Launcher',
      path: appPath
    })

    if (enable) {
      await autoLauncher.enable()
    } else {
      await autoLauncher.disable()
    }
    return { success: true }
  }
  return { success: false, error: 'Not Windows' }
})

app.whenReady().then(createWindow)

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit()
  }
})

app.on('activate', () => {
  if (mainWindow === null) {
    createWindow()
  }
})
