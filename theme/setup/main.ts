import { defineAppSetup } from '@slidev/types'

// Each deploy renames the slide files. A tab opened before the deploy then asks
// for files that no longer exist, and the slide shows "failed to fetch".
// Reload once to get the new version. The timestamp stops a reload loop if a
// file is really missing.
export default defineAppSetup(() => {
  if (typeof window === 'undefined') return

  window.addEventListener('vite:preloadError', (event) => {
    const key = 'cflp-reloaded-at'
    let last = 0
    try {
      last = Number(sessionStorage.getItem(key) ?? 0)
      if (Date.now() - last < 10_000) return
      sessionStorage.setItem(key, String(Date.now()))
    } catch {
      // Storage can be blocked; reloading once is still better than a broken slide.
    }
    event.preventDefault()
    window.location.reload()
  })
})
