---
theme: ../../theme
routerMode: hash
title: "Deep dive 1: What is .NET?"
info: |
  CFLP laboratory, week 1, optional part.
optional: true
layout: cover
class: optional-divider
---

# Deep dive: What is .NET?

Optional and not graded. Feel free to leave.

---

# What is installed on your machine

```bash
dotnet --list-sdks
dotnet --list-runtimes
```

The SDK is what you need to write programs: the C# compiler, the `dotnet` commands and the project templates. It also contains a runtime.

The runtime is what you need to run them. A machine that only runs .NET programs, such as a server, can have the runtime without the SDK.

`net10.0` in the `.csproj` means the program needs runtime 10 to run.

---

# From source code to a running program

```mermaid {scale: 0.9}
flowchart LR
  A[Program.cs] --> B[C# compiler] --> C[.dll with IL] --> D[runtime and JIT] --> E[machine code]
```

Your code is compiled twice: once by the C# compiler into IL (intermediate language), and again by the runtime into machine code while the program runs.

---

# What the build produces

```bash
cd labs/01-intro/code/UnderTheHood
dotnet build -c Release
ls -l bin/Release/net10.0
```

```text
124712  UnderTheHood                    native launcher for this OS
  5632  UnderTheHood.dll                your code, as IL
 11928  UnderTheHood.pdb                debug information
   330  UnderTheHood.runtimeconfig.json which runtime to load
   408  UnderTheHood.deps.json          dependencies
```

Your program is the 5 KB `.dll`. You can run it with `dotnet bin/Release/net10.0/UnderTheHood.dll`, or through the launcher next to it.

---

# Which runtime to load

`UnderTheHood.runtimeconfig.json`:

```json
{
  "runtimeOptions": {
    "tfm": "net10.0",
    "framework": {
      "name": "Microsoft.NETCore.App",
      "version": "10.0.0"
    }
  }
}
```

When you start the program, `dotnet` reads this file and looks for runtime 10.0.0 or a newer patch of it, such as 10.0.2.

---

# Where `Main` went

<<< @/code/UnderTheHood/Program.cs#entry cs

```text
Entry point: Program.<Main>$
```

With top-level statements the compiler still creates a class and a `Main` method. It names the method `<Main>$`, which is not a valid C# name, so it can never clash with a method you write.

---

# What is inside the `.dll`

The `.dll` does not contain machine code. It contains IL, a list of simple instructions for an imaginary processor that works with a stack: push two values, add them, return the result.

IL is the same on every computer. The method `Add` from the demo is four bytes of it.

<!--
LIVE: open UnderTheHood.dll in the tool of your choice and show the IL of Calculator.Add
(ldarg.0, ldarg.1, add, ret). The program itself prints the raw bytes: 0203582A.
-->

---

# The JIT

When a method is called for the first time, the runtime's JIT (just-in-time) compiler turns its IL into machine code for the processor it is running on.

It first compiles quickly, without optimizations, so the program starts fast. If a method turns out to be called often, the JIT compiles it again, this time optimized.

<!--
Optional demo, run the program directly (not `dotnet run`, which prints its own methods too):

  DOTNET_JitDisasmSummary=1 ./bin/Release/net10.0/UnderTheHood | grep Calculator

Add appears three times: Tier0 (36 bytes), Instrumented Tier0, Tier1 (20 bytes).
To see the machine code: DOTNET_JitDisasm='Calculator:Add' ./bin/Release/net10.0/UnderTheHood
-->

---

# Why the same program runs everywhere

The `.dll` you build on Windows is the same file on Linux or macOS. Only the machine code the JIT produces is different, because each processor has its own instructions.

That is why a .NET program runs on any computer that has the runtime installed.

---

# In the browser

[sharplab.io](https://sharplab.io) shows the same steps for any code you type: the C# the compiler really sees, the IL, and the JIT output. Nothing to install.

Next deep dive: value types, reference types and what happens in memory when you pass them around.
