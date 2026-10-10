---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#1d4ed8'
title: "Optional 3: A first web API"
info: |
  CFLP laboratory, week 3, optional part.
optional: true
layout: cover
class: optional-divider
---

# Optional: A first web API

Optional and not graded. Feel free to leave.

---

# Today

Last week we picked the study room booking application and wrote down its rules. Today it starts running.

1. Write the classes for rooms and bookings.
2. Work out when two bookings overlap.
3. Put the rules in one method, shaped like `TryParse` from the lab.
4. Put all of it behind a web API and send it requests.

<AppLink tag="week-03" />

<!--
The requirements we agreed on are in REQUIREMENTS.md in the study-rooms repository. Open it in the browser and scroll through it once, so everyone sees the R numbers. Today we only need R5 and R13.
The link points to the week-03 tag, which exists only after the lab (tag it at the end).
-->

---

# The rules for today

From the list we agreed on last week:

- R5. Two active bookings of the same room never overlap.
- R13. A student cannot be in two places at once: the bookings they own or joined never overlap.

Both rules need to see every booking, so neither fits inside the `Booking` class. They go in a class that holds all the bookings.

<!--
Remind them of the week 2 slide: a rule about one object goes in its constructor or methods, a rule about many objects goes in a class that sees all of them.
-->

---

# The classes

```text
Room  1 ──── * Booking  1 ──── * Participant
  Name           RoomId            UserId
  Building       OwnerId           Status
  Capacity       Date
  Equipment      Start, End
                 JoinPolicy
                 Status
```

The owner is also a participant. That way "everyone in the booking" always includes the owner, and nobody has to remember to add them.

<!--
LIVE: create the project with `dotnet new web -n StudyRooms.Api`, then write Room, Booking and Participant in a Domain folder, with the enums.
Use `{ get; }` and `{ get; private set; }` as in lab 2. Booking's constructor throws when End is not after Start, and adds the owner to Participants.
Equipment is a [Flags] enum: mention that Projector | Whiteboard means both, and leave the details for later.
-->

---

# When do two bookings overlap?

Ana has A101 from 10:00 to 12:00. Which of these clash with her?

```text
Ana's booking         ████████
starts inside             ████████
ends inside       ████████
contains it       ████████████████
inside it               ████
touches it                    ████
before it     ██████
              8   9   10  11  12  13  14
```

<div v-click>

The first four overlap. The last two do not, because 12:00 to 13:00 starts exactly when Ana leaves. One condition covers all six:

```cs
Start < end && start < End
```

</div>

<!--
Ask the class to write the condition before revealing it. Most first attempts check only "start is inside", or use <= and refuse the touching case.
Test each answer against all six rows on the board.
The condition reads: each booking starts before the other one ends.
-->

---

# Into the `Booking` class

```cs
public bool Overlaps(DateOnly date, TimeOnly start, TimeOnly end)
{
    return Date == date && Start < end && start < End;
}
```

`DateOnly` and `TimeOnly` are .NET types for a day without a time, and a time without a day. A booking always fits in one day, so we keep them apart.

---
layout: two-cols
---

# `TryBook`

```cs
public bool TryBook(int roomId, DateOnly date,
    TimeOnly start, TimeOnly end, string userId,
    JoinPolicy joinPolicy,
    [NotNullWhen(true)] out Booking? booking,
    [NotNullWhen(false)] out string? error)
```

It has the same shape as `TryParse` from the lab. It returns `true` with the new booking, or `false` with the reason it was refused.

`[NotNullWhen(true)]` is a note for the compiler: when the method returns `true`, `booking` is not null. Without it, the caller gets a warning when it uses `booking`.

::right::

<div class="pl-6">

```cs
foreach (Booking other in _bookings)
{
    if (other.Status != BookingStatus.Active
        || !other.Overlaps(date, start, end))
    {
        continue;
    }

    // The room is already booked
    if (other.RoomId == roomId)
    {
        booking = null;
        error = "The room is taken ...";
        return false;
    }

    // Same for the student: HasJoined(userId)
}
```

</div>

<!--
LIVE: write the Timetable class with a List<Room>, a List<Booking>, AddRoom, GetRooms, FindRoom and TryBook.
Try it from Program.cs as a console program first: book 10:00 to 12:00, then 11:00 to 13:00, and print the error.
Write TryBook without the attributes first, use it in Program.cs, and show the warning on booking.Id. Then add the attributes and watch the warning go away. They need `using System.Diagnostics.CodeAnalysis;`.
-->

---

# A web API

A web API is a program that answers requests over HTTP. Think of a restaurant: you never walk into the kitchen. You tell the waiter what you want, and the waiter comes back with a dish, or with a reason you cannot have it.

```text
POST /bookings                                       your order
{ "roomId": 1, "date": "2026-10-20", "start": "10:00", ... }

201 Created                                          the answer
{ "id": 1, "roomId": 1, "ownerId": "ana", ... }
```

A request has a method, a path and sometimes a body. The answer has a status code and usually a body in JSON.

<!--
The browser is the customer, the API is the waiter, and the Timetable class is the kitchen. The customer only sees the menu (the list of paths) and what comes back.
-->

---

# Paths and methods

The path is a noun: which thing we are talking about.

```text
/rooms                         all rooms
/rooms/2                       room 2
/bookings/1                    booking 1
/timetable?date=2026-10-20     the bookings of one day
```

The method is the verb: what to do with that thing. `GET /rooms/2` reads room 2, and `DELETE /rooms/2` removes it. Same thing, different action.

<!--
A common beginner mistake is putting the verb in the path: /getRooms, /createBooking, /deleteRoom?id=2. HTTP already has the verbs, so the path only names the thing.
-->

---

# The methods

Picture a paper sheet on the door of each study room, where people write their bookings.

| Method | What it does | On the paper sheet |
|---|---|---|
| `GET` | reads, changes nothing | look at the sheet |
| `POST` | creates something new | write a new line |
| `PUT` | replaces the whole thing | rewrite a line from scratch |
| `PATCH` | changes one part | fix only the end time |
| `DELETE` | removes it | erase a line |

<!--
In our API: GET /rooms and GET /timetable, POST /bookings today. PUT /rooms/2 and DELETE /rooms/2 come in week 6 for admins. We do not use PATCH.
The browser address bar can only send GET. That is why we test POST with the .http file.
Cancelling a booking will be POST /bookings/1/cancel, not DELETE: the booking stays, with the status Cancelled, so the owner can see what happened to it.
-->

---

# What if you send it twice?

The network is not perfect. Sometimes a request is sent again because the answer got lost.

- `GET` twice: nothing changes. Refreshing a page is always fine.
- `PUT` twice: the line is rewritten with the same text. Same result as once.
- `DELETE` twice: the line is already gone. The second time answers 404, but the sheet looks the same.
- `POST` twice: two new lines, so two bookings.

That is why a browser asks "Confirm form resubmission?" when you refresh a page after sending a form. Methods that are safe to repeat are called idempotent.

<!--
Ask the class: what should the API do if Ana's POST is sent twice by accident? Ours answers 409 to the second one, because the room is already taken, by Ana herself. A lucky side effect of our rules.
-->

---

# Status codes: it worked

The first digit tells you who is to blame. 2xx means it worked, 4xx means the request was wrong, and 5xx means the server broke.

| Code | Meaning | In a controller |
|---|---|---|
| 200 OK | here it is | `return Ok(room);` |
| 201 Created | created, and here is where | `return Created(url, booking);` |
| 204 No Content | done, nothing to send back | `return NoContent();` |

These methods come from `ControllerBase`, the class every controller inherits from. Returning the object itself, `return room;`, also gives 200.

---

# Status codes: it did not work

| Code | Meaning | In a controller |
|---|---|---|
| 400 Bad Request | makes no sense | `return BadRequest("...");` |
| 401 Unauthorized | log in first | `return Unauthorized();` |
| 403 Forbidden | logged in, but not allowed | `return Forbid();` |
| 404 Not Found | no such thing | `return NotFound();` |
| 409 Conflict | clashes with what exists | `return Conflict("...");` |

500 Internal Server Error means the server broke. You never return it yourself: ASP.NET Core sends it when your code throws an exception that nobody catches.

<!--
Today we use 200, 201, 400, 404 and 409. 401 and 403 arrive with accounts in week 6: 401 is a stranger at the door, 403 is a student trying to open the admin office.
Demo for 500: comment out the End <= Start check in BookingsController.Create and send a booking from 12:00 to 11:00. The Booking constructor throws, and the answer is 500 with the exception in the terminal. Put the check back: now it is a 400, because we caught the mistake ourselves.
-->

---
layout: center
---

# Every code has a cat

<img src="https://http.cat/409.jpg" class="h-90 mx-auto rounded" alt="HTTP 409 Conflict, as a cat" />

https://http.cat

<!--
Needs internet. Try https://http.cat/404 and https://http.cat/418 with the class.
There is also https://http.dog for dog people.
-->

---

# `Program.cs`

```cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<Timetable>();

var app = builder.Build();
app.MapControllers();
app.Run();
```

`AddControllers` finds every controller class in the project, and `MapControllers` sends each request to the right one. `AddSingleton` registers the `Timetable`: one object for the whole application, so every request sees the same bookings.

<!--
The real Program.cs has two more things: JsonStringEnumConverter, so enums travel as text ("Open") instead of numbers, and AddSampleRooms, which adds three rooms at startup.
LIVE: create the project with `dotnet new web -n StudyRooms.Api`, move the classes from the console version in, and write this Program.cs.
-->

---
layout: two-cols
---

# A controller

```cs
[ApiController]
[Route("rooms")]
public class RoomsController : ControllerBase
{
    private readonly Timetable _timetable;

    public RoomsController(Timetable timetable)
    {
        _timetable = timetable;
    }

    [HttpGet]
    public List<Room> GetAll()
    {
        return _timetable.GetRooms();
    }
}
```

::right::

<div class="pl-6 pt-22">

A controller is a class that answers requests. Its methods are called actions.

`[Route("rooms")]` and `[HttpGet]` say which requests reach `GetAll`: `GET /rooms`.

The list it returns goes back to the browser as JSON.

</div>

<!--
LIVE: write RoomsController, run the API, and open http://localhost:5080/rooms in the browser.
Then add Get(int id) with [HttpGet("{id}")] and show /rooms/2 and /rooms/99 (404).
Then TimetableController: Get(DateOnly date) reads ?date=2026-10-20 from the query string. Show /timetable?date=2026-10-20 (an empty list for now).
-->

---

# Where `timetable` comes from

Nobody writes `new RoomsController(...)`. ASP.NET Core creates a controller for each request, sees that the constructor asks for a `Timetable`, and passes the one registered in `Program.cs`.

A logger arrives the same way. Ask for it in the constructor:

```cs
public BookingsController(Timetable timetable,
                          ILogger<BookingsController> logger)
{
    _timetable = timetable;
    _logger = logger;
}
```

This is called dependency injection. In week 5 we meet the other lifetimes besides singleton.

<!--
Show the log lines in the terminal when bookings are created or refused.
Point out the {UserId} placeholders in LogInformation: they are not string interpolation. The logger keeps the values separately, which matters in week 10 when we search logs by booking id.
-->

---

# Why not a `static` class?

A static class would also give us one timetable for the whole application:

```cs
Timetable.TryBook(...);       // static: called by its class name, from anywhere
_timetable.TryBook(...);      // singleton: an object passed to the controller
```

With a singleton, `Timetable` stays an ordinary class, and `Program.cs` decides how many objects exist. In week 5 the timetable moves to a database and each request gets its own timetable: we change `AddSingleton` to `AddScoped` in one line. With a static class we would rewrite every call.

The constructor also shows what a controller needs. A static call can hide anywhere in the code.

<!--
They will ask this. Short answer: both mean "one for the whole app". Static fixes that forever and lets any code reach it; a singleton is a normal object whose lifetime is a setting in Program.cs, handed only to the classes that ask for it.
More background in student-questions.md (Lab 3).
-->

---

# Creating a booking

```cs
[HttpPost]
public IActionResult Create(BookingRequest request)
{
    if (_timetable.FindRoom(request.RoomId) is null)
    {
        return NotFound($"Room {request.RoomId} does not exist.");
    }

    if (!_timetable.TryBook(request.RoomId, request.Date, request.Start,
            request.End, request.UserId, request.JoinPolicy,
            out Booking? booking, out string? error))
    {
        return Conflict(error);
    }

    return Created($"/bookings/{booking.Id}", booking);
}
```

<!--
The body arrives as a BookingRequest record (BookingRequest.cs). We have no accounts yet, so the request says who is booking.
NotFound, Conflict and Created come from ControllerBase and set the status code. Created also sends a Location header with the address of the new booking, which GET /bookings/{id} answers.
The real code also writes a log line for every booking created or refused, and returns 400 (BadRequest) when End is not after Start.
If someone asks what happens when two requests book at the same moment: that is a real problem, and it is the topic of week 5, when the bookings move to a database.
-->

---

# Trying it out

The `.http` file in the project holds the requests. Click Send Request above each one.

```text
POST A101 10:00-12:00 for ana       201 Created
POST A101 11:00-13:00 for mihai     409 Conflict    room taken (R5)
POST A101 12:00-13:00 for mihai     201 Created     only touches
POST A102 11:00-12:00 for ana       409 Conflict    ana is busy (R13)
POST room 99                        404 Not Found
GET  /timetable?date=2026-10-20     200 OK
```

<!--
SHOWCASE: run `dotnet run --project src/StudyRooms.Api` and send the requests in StudyRooms.Api.http from top to bottom. Show the log line for each one in the terminal.
On machines without VS Code or Rider, use curl (the README has an example).
Then stop the API and start it again: all bookings are gone. That is the hook for week 5.
-->

---

# What is still wrong

Look at what `GET /timetable` sends back:

```json
{ "id": 1, "ownerId": "ana",
  "participants": [ { "userId": "ana", "createdAt": "2026-10-10T06:32:03Z" } ], ... }
```

- The answer is our stored object, with every field in it. R7 says other students see display names only.
- Errors are plain strings, with no structure a program can read.
- Nothing stops a booking at 10:07, at 3:00 in the night, or in 2030 (R6).
- Anyone can send `"userId": "ana"` and book in Ana's name.

Next week fixes the first three. The last one waits for accounts in week 6.

<AppLink tag="week-03" />

<!--
Ask the class which of these bothers them most. The privacy one leads into next week: the API should send a different object from the one it stores.
-->
