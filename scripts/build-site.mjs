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

    entry.decks.push({ ...deck, slug, title: readTitle(source) })
  }

  if (entry.decks.length > 0) entries.push(entry)
}

const template = readFileSync(join(root, 'site', 'index.html'), 'utf8')
writeFileSync(join(dist, 'index.html'), template.replace('<!-- LABS -->', entries.map(renderLab).join('\n')))
writeFileSync(join(dist, '.nojekyll'), '')
console.log(`\n✔ Built ${entries.length} lab(s) into dist/`)

function readTitle(file) {
  const frontmatter = readFileSync(file, 'utf8').match(/^---\n([\s\S]*?)\n---/)
  const title = frontmatter?.[1].match(/^title:\s*"?(.*?)"?\s*$/m)
  return title?.[1] ?? file
}

function renderLab({ lab, week, decks }) {
  const main = decks.find((d) => d.kind === 'lab')
  const deepDive = decks.find((d) => d.kind === 'deep-dive')
  const title = escapeHtml(main?.title.replace(/^Lab \d+\s*[:—-]\s*/, '') ?? lab)
  const links = [
    main && `<a href="${lab}/${main.slug}/">Slides</a>`,
    deepDive && `<a class="optional" href="${lab}/${deepDive.slug}/" title="${escapeHtml(deepDive.title)}">Deep dive</a>`,
    `<a href="${REPO_URL}/tree/main/labs/${lab}">Code</a>`,
  ].filter(Boolean)

  return `      <li>
        <span class="week">Week ${week}</span>
        <span class="title">${title}</span>
        <span class="links">${links.join('')}</span>
      </li>`
}

function escapeHtml(text) {
  return text.replace(/[&<>"]/g, (c) => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' })[c])
}
