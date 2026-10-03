---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#0f766e'
title: "Optional 2: Choosing the application"
info: |
  CFLP laboratory, week 2, optional part.
optional: true
layout: cover
class: optional-divider
---

# Optional: Choosing the application

Optional and not graded. Feel free to leave.

---

# Today

We pick the application we build this semester and design it on the board.

1. Vote on one of the five options.
2. Write down who uses it and what they can do.
3. Find the entities, their fields and the rules.
4. Sketch two or three screens.
5. Write the entities as C# classes, with what you learned in the lab today.

Next week these classes go behind a web API.

---

# The vote

| Application | What it does |
|---|---|
| Conference tickets | Buy tickets for events and their sessions |
| Study room booking | Book a faculty room for a time slot, with no overlaps |
| Shared expenses | Roommates track who paid what and who owes whom |
| Library | Borrow books and get reminders when a loan is late |
| Canteen orders | Order from the menu and follow the order until it is ready |

Pick the one you would actually use.

<!--
Show of hands, one vote each. If the top two are within two or three votes, vote again between those two.
If it is still a tie, we build the study room booking application.
After the vote, open the cheat sheet page for the winner.
-->

---

# Who uses it, and what can they do?

Write each thing a user can do as one short sentence. An example from an application we are not building, an animal shelter:

- A visitor sees the animals that are up for adoption.
- A visitor sends an adoption request for one animal.
- An employee approves or rejects a request.
- An employee adds a new animal.

Ten sentences or fewer. Anything we leave out today can come later.

<!--
Write the class's sentences on the board. There are two roles, user and admin, so sentences may start with either. Push back on real payments and on any third role.
-->

---

# Entities

The nouns in those sentences are the candidates: animal, visitor, request, employee.

For each one, ask:

- What does it need to store?
- Does it belong to another one? A request is for one animal, and an animal can have many requests.
- Does a field have a fixed list of values? That is an enum, such as the status of a request: submitted, approved or rejected.

```text
Animal  1 ──── * AdoptionRequest * ──── 1 Visitor
  Name             Status                   Name
  Species          SentAt                   Age
  Age
```

<!--
Aim for three or four entities. Employee is not an entity here: it is a user with the admin role, and login comes later in the semester.
-->

---

# Rules

A rule is something that must never happen. For the shelter:

- An animal can have only one approved request.
- A rejected request cannot be approved later.
- An adopter must be at least 18 years old.

Each rule has a place in the code. A rule about one object goes in its constructor or in one of its methods, as in `TaskItem` today. A rule that looks at several objects, such as "only one approved request per animal", needs a class that sees all of them, like `Board`.

---

# Screens

Sketch two or three screens on paper. For each one, write:

- what it shows
- what the user can do there

Every action on a screen becomes a request to the API. "Send an adoption request" turns into one request that creates an `AdoptionRequest`.

The screens get built in week 7. Today they only help us check that the entities have everything the screens need.

---

# From the board to C#

The entities become classes and enums, with the same ideas as in the lab.

```cs
public class AdoptionRequest
{
    public int Id { get; }
    public int AnimalId { get; }
    public RequestStatus Status { get; private set; }

    public bool Approve()
    {
        if (Status != RequestStatus.Submitted)
        {
            return false;
        }
        Status = RequestStatus.Approved;
        return true;
    }
}
```

<!--
LIVE: create a console project for the chosen application and write its entities, enums and one or two rules.
Keep everything in a List<T> in Program.cs. No web and no database yet.
-->

---

# Next week: a web API

A web API is a program that answers requests over HTTP, usually with JSON. Your browser sends requests like these all the time.

Open a site, press `F12`, and choose the Network tab. Every line is a request. Click one to see what was sent and what came back.

Next week the classes we write today get their own requests: one to list the items, one to see one item, and one to create a new one.

<!--
Demo 1: open https://openlibrary.org/isbn/9780140449136.json in the browser to show raw JSON.
Demo 2: search on https://openlibrary.org with the Network tab open, filter by Fetch/XHR, and click a search.json request.
-->
