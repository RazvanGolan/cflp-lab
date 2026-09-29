---
theme: ../../theme
routerMode: hash
title: "Lab 1 — Introduction to C#"
info: |
  CFLP laboratory, week 1.
week: 1
layout: cover
---

# Lab 1 — Introduction to C#

Program structure, top-level statements, console I/O

<!--
Speaker notes live in HTML comments like this one.
With a single screen, export them to paper with `npx slidev export-notes`.
-->

---

# Importing code from the repository

Code on slides comes from real projects that compile in CI, not copy-pasted text.

<<< @/code/HelloWorld/Program.cs#greeting cs {1-2|4|all}

<CodeLink path="labs/01-intro/code/HelloWorld" />

---

# Second region, same file

<<< @/code/HelloWorld/Program.cs#sum cs {1|2-6|8}

Run it:

```bash
dotnet run --project labs/01-intro/code/HelloWorld
```

---
layout: center
---

# Assignment

Statement on Moodle — upload by the deadline shown there.
