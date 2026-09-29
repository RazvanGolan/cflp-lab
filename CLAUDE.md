# CLAUDE.md

This repository holds the slides and demo code for the C# laboratory of Fundamental Concepts of Programming Languages (CFLP), a course at the Politehnica University of Timișoara. The site is published at https://razvangolan.github.io/cflp-lab/.

Each weekly session has two parts. The first is the graded lab: a short presentation, code written live, and an assignment that students upload to Moodle. The second is an optional deep dive into how C# and .NET work underneath (IL, the JIT, the garbage collector and so on). Students may leave before it.

## Layout

```
labs/NN-topic/
  slides.md        Slidev deck for the graded lab
  deep-dive.md     Slidev deck for the optional part (not every week has one)
  README.md        what the lab covers, how to run the code, further reading
  code/<Project>/  one console project per concept
theme/             shared Slidev theme, tuned for a classroom projector
site/index.html    landing page template; the build fills in one row per lab
scripts/build-site.mjs   builds every deck into dist/ and writes the landing page
CFLP.slnx          lists every demo project
```

## Commands

```bash
dotnet build CFLP.slnx                          # build all demo code
dotnet run --project labs/01-intro/code/Hello

npm install                                     # once
npm run dev -- labs/01-intro/slides.md          # edit one deck with live reload
npm run build                                   # whole site into dist/, base /cflp-lab/
npm run build -- --only 01                      # only lab 01
npm run preview                                 # build with base / and serve locally
```

CI (`.github/workflows/pages.yml`) builds the code with `-warnaserror`, checks that every `.csproj` is listed in `CFLP.slnx`, builds the site, and deploys it from `main`.

## Conventions

### Code

- Target `net10.0` with nullable reference types on. Each `.csproj` holds its own settings, with no shared `Directory.Build.props`, so a student can copy one project folder and build it on its own.
- Add every new project to the solution: `dotnet sln CFLP.slnx add <path> --solution-folder labs/NN-topic`.
- The code must build and run with nothing but the .NET SDK. The lab machines run Linux and students cannot install extra software on them, so demos cannot depend on databases, Docker or anything similar.
- Pick demo domains that differ from the assignments. The code here must never contain a solution to a graded assignment or a test, because those are shared with other lab groups and reused from year to year.

### Slides

- Every deck starts with this headmatter:
  ```yaml
  ---
  theme: ../../theme
  routerMode: hash
  title: "Lab N: Topic"
  layout: cover
  ---
  ```
  `routerMode: hash` is required because GitHub Pages cannot serve deep links such as `/slides/3`. The landing page takes the lab name from `title` and strips the `Lab N:` prefix.
- Each lab has its own accent colour, set in the headmatter of both its decks. The theme derives the dark-mode shade and the other tints from it, and the landing page uses it for that lab's row:
  ```yaml
  themeConfig:
    primary: '#512bd4'
  ```
  Use the next unused colour from this list, so that neighbouring labs look different. Each one has enough contrast on a white background.

  | Lab | Colour | |
  |---|---|---|
  | 1 | `#512bd4` | .NET purple |
  | 2 | `#0f766e` | teal |
  | 3 | `#1d4ed8` | blue |
  | 4 | `#be123c` | rose |
  | 5 | `#b45309` | amber |
  | 6 | `#15803d` | green |
  | 7 | `#a21caf` | magenta |
  | 9 | `#0369a1` | sky |
  | 10 | `#c2410c` | orange |
  | 11 | `#4d7c0f` | olive |
  | 12 | `#334155` | slate |
- Slides and the landing page follow the viewer's light or dark setting. Colours in the theme are CSS variables with a dark variant, so do not hard-code colours on slides.
- Slides show code by importing a C# region from the lab's projects, so the code on a slide always compiles:
  ```md
  <<< @/code/Library/Program.cs#oldest cs {1|2-8|all}
  ```
  Mark the region in the C# file with `#region oldest` and `#endregion`. `@` is the lab folder.
- The slides are shown on one laptop through a projector, with no presenter view. Keep text large and short, and put the instructor's script in HTML comments at the end of a slide.
- A slide cannot run C#. When the output matters, show it in a `text` block revealed with `v-click`, and keep it in sync with the code by hand.

### Writing

Students read the slides, the READMEs and the landing page, so that text has to read like a person wrote it. Use the humanizer skill (`blader/humanizer`) when it is installed. Either way, follow these rules:

- No em or en dashes in prose. Titles use a colon: "Lab 3: Parameter passing".
- State the point directly. Avoid "not X but Y" contrasts, one-line closers that repeat the paragraph, and lists that come in threes for rhythm.
- No bold labels in front of list items, and no decorative emphasis.
- Use plain verbs ("is", "has") rather than "serves as" or "features".
- All material is in English.

READMEs are for students. Instructor notes, such as the publishing workflow, do not belong in this repository.

## Branches

Work happens on `next`. After each lab, `next` is merged into `main`, which publishes that week's slides and code. Anything that reveals the content of a future lab stays on `next` until then.
