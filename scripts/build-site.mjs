// Builds every deck under labs/*/ into dist/<lab>/<deck>/ and generates the landing page.
//
//   node scripts/build-site.mjs              -> base "/cflp-lab/" (GitHub Pages)
//   node scripts/build-site.mjs --base /     -> for local preview
//   node scripts/build-site.mjs --only 01    -> build only labs whose folder starts with "01"

import { execFileSync } from 'node:child_process'
import { existsSync, mkdirSync, readdirSync, readFileSync, rmSync, writeFileSync } from 'node:fs'
import { join, resolve } from 'node:path'
import { parseArgs } from 'node:util'

const REPO_URL = 'https://github.com/RazvanGolan/cflp-lab'
const DEFAULT_ACCENT = '#512bd4'
const DECKS = [
  { file: 'slides.md', kind: 'lab' },
  { file: 'deep-dive.md', kind: 'deep-dive' },
]

const { values: args } = parseArgs({
  options: {
    base: { type: 'string', default: '/cflp-lab/' },
    only: { type: 'string' },
  },
})

const root = resolve(import.meta.dirname, '..')
const labsDir = join(root, 'labs')
const dist = join(root, 'dist')
const slidevBin = join(root, 'node_modules', '.bin', 'slidev')

rmSync(dist, { recursive: true, force: true })
mkdirSync(dist, { recursive: true })

const labs = readdirSync(labsDir, { withFileTypes: true })
  .filter((d) => d.isDirectory() && /^\d{2}-/.test(d.name))
  .map((d) => d.name)
  .sort()
  .filter((name) => !args.only || name.startsWith(args.only))

const entries = []

for (const lab of labs) {
  const entry = { lab, week: Number(lab.slice(0, 2)), decks: [] }

  for (const deck of DECKS) {
    const source = join(labsDir, lab, deck.file)
    if (!existsSync(source)) continue

    const slug = deck.file.replace(/\.md$/, '')
    const out = join(dist, lab, slug)
    console.log(`\n▶ ${lab}/${deck.file}`)
    execFileSync(slidevBin, ['build', source, '--base', `${args.base}${lab}/${slug}/`, '--out', out], {
      stdio: 'inherit',
      cwd: root,
    })

    // Point the deck's tab icon at the lab icon written below, instead of Slidev's default.
    for (const page of ['index.html', '404.html']) {
      const file = join(out, page)
      if (!existsSync(file)) continue
      const html = readFileSync(file, 'utf8').replace(/<link rel="icon"[^>]*>/, '<link rel="icon" type="image/svg+xml" href="../icon.svg">')
      writeFileSync(file, html)
    }

    entry.decks.push({ ...deck, slug, ...readHeadmatter(source) })
  }

  if (entry.decks.length > 0) {
    // One icon per lab: its number on its accent colour, shared by both decks.
    const colour = entry.decks.find((d) => d.primary)?.primary ?? DEFAULT_ACCENT
    writeFileSync(join(dist, lab, 'icon.svg'), iconSvg(String(entry.week), colour))
    entries.push(entry)
  }
}

writeFileSync(join(dist, 'icon.svg'), iconSvg('C#', DEFAULT_ACCENT))

const template = readFileSync(join(root, 'site', 'index.html'), 'utf8')
writeFileSync(join(dist, 'index.html'), template.replace('<!-- LABS -->', entries.map(renderLab).join('\n')))
writeFileSync(join(dist, '.nojekyll'), '')
console.log(`\n✔ Built ${entries.length} lab(s) into dist/`)

// Reads the deck title and its accent colour (themeConfig.primary) from the headmatter.
function readHeadmatter(file) {
  const headmatter = readFileSync(file, 'utf8').match(/^---\n([\s\S]*?)\n---/)?.[1] ?? ''
  const title = headmatter.match(/^title:\s*"?(.*?)"?\s*$/m)?.[1] ?? file
  const primary = headmatter.match(/^\s+primary:\s*['"]?(#[0-9a-fA-F]{3,8})['"]?\s*$/m)?.[1]
  return { title, primary }
}

function renderLab({ lab, week, decks }) {
  const main = decks.find((d) => d.kind === 'lab')
  const deepDive = decks.find((d) => d.kind === 'deep-dive')
  const title = escapeHtml(main?.title.replace(/^Lab \d+\s*[:—-]\s*/, '') ?? lab)
  const links = [
    main && `<a href="${lab}/${main.slug}/">Slides</a>`,
    deepDive && `<a class="optional" href="${lab}/${deepDive.slug}/" title="${escapeHtml(deepDive.title)}">Optional part</a>`,
    `<a href="${REPO_URL}/tree/main/labs/${lab}">Code</a>`,
  ].filter(Boolean)

  const accent = main?.primary ?? deepDive?.primary
  const style = accent ? ` style="--lab: ${accent}"` : ''

  return `      <li${style}>
        <span class="week">Week ${week}</span>
        <span class="title">${title}</span>
        <span class="links">${links.join('')}</span>
      </li>`
}

function iconSvg(text, colour) {
  const size = text.length === 1 ? 42 : 32
  return `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 64 64">
  <rect width="64" height="64" rx="14" fill="${colour}"/>
  <text x="32" y="34" text-anchor="middle" dominant-baseline="central" fill="#fff"
    font-family="system-ui, -apple-system, 'Segoe UI', Roboto, sans-serif" font-size="${size}" font-weight="700">${escapeHtml(text)}</text>
</svg>
`
}

function escapeHtml(text) {
  return text.replace(/[&<>"]/g, (c) => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' })[c])
}
