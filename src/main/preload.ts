import { contextBridge, ipcRenderer } from 'electron'

export interface RecentFile {
  name: string
  path: string
  type: 'excel' | 'word' | 'powerpoint' | 'other'
  lastModified: string
}

export interface ElectronAPI {
  getRecentFiles: () => Promise<RecentFile[]>
  openFile: (filePath: string) => Promise<{ success: boolean; error?: string }>
  openExternal: (url: string) => Promise<{ success: boolean; error?: string }>
  openExplorer: (dirPath?: string) => Promise<{ success: boolean; error?: string }>
  setAutoLaunch: (enable: boolean) => Promise<{ success: boolean; error?: string }>
  windowMinimize: () => void
  windowMaximize: () => void
  windowClose: () => void
}

const api: ElectronAPI = {
  getRecentFiles: () => ipcRenderer.invoke('get-recent-files'),
  openFile: (filePath) => ipcRenderer.invoke('open-file', filePath),
  openExternal: (url) => ipcRenderer.invoke('open-external', url),
  openExplorer: (dirPath) => ipcRenderer.invoke('open-explorer', dirPath),
  setAutoLaunch: (enable) => ipcRenderer.invoke('set-auto-launch', enable),
  windowMinimize: () => ipcRenderer.send('window-minimize'),
  windowMaximize: () => ipcRenderer.send('window-maximize'),
  windowClose: () => ipcRenderer.send('window-close')
}

contextBridge.exposeInMainWorld('electronAPI', api)
