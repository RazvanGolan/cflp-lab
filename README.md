# CFLP Lab

Slides and demo code for the C# laboratory of Fundamental Concepts of Programming Languages (CFLP).

Slides: https://razvangolan.github.io/cflp-lab/

Each lab is published after it takes place. Assignments and deadlines are on Moodle.

## Running the code

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download). Then:

```bash
git clone https://github.com/RazvanGolan/cflp-lab.git
cd cflp-lab
dotnet run --project labs/01-intro/code/HelloWorld
```

Every folder under `labs/NN-topic/code/` is an independent console project, so you can open just the one you need.

## Where to find things

```
labs/
  01-intro/
    README.md        summary, how to run, further reading
    code/            one console project per concept
    slides.md        source of the lab slides
    deep-dive.md     source of the optional deep-dive slides
```

The code here is the cleaned-up version of what we write live in the lab, so it may differ slightly from what you saw.
