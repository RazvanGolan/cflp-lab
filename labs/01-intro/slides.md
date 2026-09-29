---
theme: ../../theme
routerMode: hash
title: "Lab 1: Introduction to C#"
info: |
  CFLP laboratory, week 1.
week: 1
layout: cover
---

# Lab 1: Introduction to C#

Program structure, top-level statements, console I/O

<!--
Speaker notes live in HTML comments like this one.
With a single screen, export them to paper with `npx slidev export-notes`.
-->

---

# Importing code from the repository

The code on these slides comes from the projects in the repository, which are compiled on every change.

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

The statement is on Moodle. Upload your solution before the deadline shown there.
