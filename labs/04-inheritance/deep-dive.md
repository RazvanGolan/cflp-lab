---
theme: ../../theme
routerMode: hash
themeConfig:
  primary: '#be123c'
title: "Optional 4: Requests, responses and rules"
info: |
  CFLP laboratory, week 4, optional part.
optional: true
layout: cover
class: optional-divider
---

# Optional: Requests, responses and rules

Optional and not graded. Feel free to leave.

---

# Where we left off

Last week's API books rooms and refuses overlaps. It also had four problems:

- It sends back the stored object, with every field in it. Other students should see display names only (R7).
- Errors are plain strings, with no structure a program can read.
- Nothing stops a booking at 10:07, at 3:00 in the night, or in 2030 (R6).
- Anyone can send `"userId": "ana"` and book in Ana's name.

Today we fix the first three, and add joining, leaving and cancelling (R8 to R14).

<AppLink tag="week-04" compare="week-03" />

<!--
The link shows every change since last week. It works once week-04 is tagged after the lab.
The fourth problem waits for accounts in week 6.
-->

---

# What we store, what we send

```text
Booking (stored)                  BookingResponse (sent)
  Id                                Id
  RoomId      1                     Room        "A101"
  OwnerId     "ana"                 Owner       "Ana P."
  Date, Start, End                  Date, Start, End
  Participants                      People      ["Ana P.", "Mihai I."]
    UserId, Status, CreatedAt       PlacesLeft  1
```

The class we store has what the rules need: user ids, every participant with their status, when they joined. The class we send has what a screen needs: names to show, and how many places are left. Ids and emails stay on the server.

<!--
Ask: which screen needs PlacesLeft? (The timetable, to show whether you can still join.) Which screen needs CreatedAt? (None, so it is not sent.)
PlacesLeft is computed when the response is built. It is never stored.
-->

---
layout: two-cols
---

# Records

```cs
public record RoomResponse(
    int Id, string Name, string Building,
    int Capacity, Equipment Equipment);
```

A record is a short way to write a class that only carries data. This one line gives a constructor and a read-only property for each value.

::right::

<div class="pl-6">

It is the same as writing this class:

```cs
public class RoomResponse
{
    public int Id { get; }
    public string Name { get; }
    // ... the other three

    public RoomResponse(int id, ...)
    {
        Id = id;
        Name = name;
        // ...
    }
}
```

</div>

<!--
Two records with the same values are also equal with ==, unlike two objects of a normal class. We do not need that today; mention it only if asked.
BookingRequest from last week was already a record.
-->

---

# From a booking to a response

Every endpoint that returns a booking calls this one method, so the rule "names only" lives in one place.

```cs
public static BookingResponse From(Booking booking, Timetable timetable)
{
    Room room = timetable.GetRoom(booking.RoomId);
    List<string> people = [];
    foreach (Participant participant in booking.Participants)
    {
        if (participant.Status == ParticipantStatus.Joined)
        {
            people.Add(timetable.GetUser(participant.UserId).DisplayName);
        }
    }
    return new BookingResponse(booking.Id, room.Id, room.Name, ...,
        Math.Max(0, room.Capacity - people.Count), ...);
}
```

<!--
The method sits inside the BookingResponse record (a record can have methods, like any class). It is static because it creates the response; there is no response object yet to call it on.
Math.Max(0, ...) matters later: an admin may lower a room's capacity below the number of people already in a booking (R3).
-->

---

# Users, for now

```cs
timetable.AddUser("ana", "Ana P.", "ana.popescu@example.com");
timetable.AddUser("mihai", "Mihai I.", "mihai.ionescu@example.com");
```

A `User` has an id, a display name and an email. The response uses only the display name, so you can check in the browser that the emails never leave the server.

In week 6 these four sample users are replaced by real accounts with passwords.

---
layout: two-cols
---

# Rules with numbers

```json
"Booking": {
  "StepMinutes": 30,
  "MinMinutes": 30,
  "MaxMinutes": 180,
  "Opens": "08:00",
  "Closes": "20:00",
  "DaysAhead": 7,
  "CancelCutoffHours": 2,
  "TimeZone": "Europe/Bucharest"
}
```

The numbers live in `appsettings.json`, so changing one does not need a new build.

::right::

<div class="pl-6 pt-22">

```cs
public class BookingOptions
{
    public int StepMinutes { get; set; }
    public TimeOnly Opens { get; set; }
    // ... one per setting
}
```

A class with one property for each setting, with the same names.

</div>

---

# Reading the settings

In `Program.cs`, copy the section into a `BookingOptions` object:

```cs
builder.Services.Configure<BookingOptions>(
    builder.Configuration.GetSection("Booking"));
```

Any class can then ask for it in its constructor, the same way controllers ask for the `Timetable`:

```cs
public BookingRules(IOptions<BookingOptions> options)
{
    _options = options.Value;
}
```

`IOptions<BookingOptions>` is a small wrapper, and `.Value` is the filled-in object.

<!--
Configure copies the section into a BookingOptions object, matching names. Any class can then ask for IOptions<BookingOptions> in its constructor, the same way controllers ask for the Timetable. .Value is the filled-in object.
Demo: change MaxMinutes to 60, restart, and a two hour booking is refused.
-->

---

# Checking the times

It collects every problem, so a student who got three things wrong sees all three at once.

```cs
public List<string> CheckTimes(DateOnly date, TimeOnly start, TimeOnly end)
{
    List<string> problems = [];
    if (!IsOnStep(start) || !IsOnStep(end))
    {
        problems.Add("Start and end must be on a 30 minute step, ...");
    }

    double minutes = (end - start).TotalMinutes;
    if (minutes < _options.MinMinutes || minutes > _options.MaxMinutes)
    {
        problems.Add("A booking lasts from 30 to 180 minutes.");
    }
    // ... opening hours, not in the past, at most 7 days ahead
    return problems;
}
```

<!--
The real messages are built from the options with string interpolation, so they always show the current numbers.
IsOnStep: minutes since midnight divided by 30 leaves no remainder. 10:30 is 630 minutes, 10:10 is 610.
-->

---

# Errors a program can read

Every error the API sends has the same shape: a short title and the list of reasons.

```cs
public record ErrorResponse(string Title, List<string> Problems);
```

```json
{
  "title": "The booking is not valid.",
  "problems": [
    "Start and end must be on a 30 minute step, like 10:00 or 10:30.",
    "Rooms can be booked between 08:00 and 20:00.",
    "There is no user zoe."
  ]
}
```

The interface in week 7 can show the title, then each problem on its own line.

---

# Sending an error

```cs
List<string> problems = _rules.CheckTimes(request.Date, request.Start, request.End);
if (_timetable.FindUser(request.UserId) is null)
{
    problems.Add($"There is no user {request.UserId}.");
}
if (problems.Count > 0)
{
    return BadRequest(new ErrorResponse("The booking is not valid.", problems));
}
```

The same record goes with every status code, through the methods from week 3:

```cs
return Conflict(new ErrorResponse("The booking was refused.", [error]));
```

`[error]` is a list with one item, written the same way as the empty list `[]`.

<!--
If the JSON itself cannot be read, for example "date": "tomorrow", ASP.NET Core answers 400 before our code runs, with its own standard shape called ProblemDetails (RFC 9457): "title", "status" and an "errors" object. Show it in Scalar. Many APIs use ProblemDetails for all their errors; others, like Stripe or GitHub, have their own shape, as we do.
-->

---

# Joining a booking

| Request | Who may send it |
|---|---|
| `POST /bookings/1/join` | anyone, if the booking is open |
| `POST /bookings/1/leave` | someone in the booking |
| `POST /bookings/1/participants/mihai/remove` | the owner |
| `POST /bookings/1/cancel` | the owner |

The body says who is sending it: `{ "userId": "mihai" }`. All four are `POST`. None of them creates, replaces or deletes the booking: they are actions that change it, and the booking stays, with its new state, so people can see what happened.

<!--
Until accounts exist, the body says who is sending the request.
Booking on request (the owner approves each person, R12) comes in week 6, when we know who the owner really is.
-->

---
layout: two-cols
---

# `TryJoin`

```cs
public bool TryJoin(string userId, int capacity,
    DateTime now, out string? error)
{
    if (!IsOpenForChanges(now, out error))
    {
        return false;
    }
    if (JoinPolicy == JoinPolicy.Private)
    {
        error = "This booking is private.";
        return false;
    }
    // ... already in it? removed earlier?
    if (CountJoined() >= capacity)
    {
        error = "The booking is full.";
        return false;
    }
    // add the participant
}
```

::right::

<div class="pl-6 pt-22">

The rules live in `Booking`, with the same shape as `TryBook` last week: `true`, or `false` with the reason.

`TryLeave`, `TryRemove` and `TryCancel` look the same. Each one only needs the booking itself.

"Not in two places at once" needs every booking, so that check stays in `Timetable.TryJoin`, which then calls this one.

</div>

<!--
The slide is shortened. In the real code the out parameter has [NotNullWhen(false)], and a student who left earlier can join again, while one the owner removed cannot.
IsOpenForChanges: the booking is not cancelled and has not started yet.
-->

---

# Who is allowed

```cs
if (request.UserId != booking.OwnerId)
{
    return StatusCode(StatusCodes.Status403Forbidden,
        new ErrorResponse("You cannot cancel this booking.",
                          ["Only the owner can cancel the booking."]));
}
```

The controller checks who is asking. `Booking` checks whether the action makes sense: is it cancelled already, is it too late.

For now the check is easy to fool, because the request says who is sending it. In week 6 the user comes from the login, and this `if` stays the same.

<!--
There is no Forbidden(...) method that takes a body. Forbid() exists, but it belongs to the login system and does not work until week 6. StatusCode(code, body) sends any status code.
-->

---

# What time is it?

The cutoff and "not in the past" need the current time at the faculty.

```cs
public DateTime Now()
{
    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
}
```

A server in a data center usually runs in UTC, which is 3 hours behind Romania in summer and 2 in winter. Without the conversion, every rule about time would be off by hours.

On Sunday 25 October Romania moves the clocks back an hour. `TimeZoneInfo` knows that, so the code does not change.

<!--
Ask the class: a booking starts at 10:00 on Monday 26 October. Until when can the owner cancel it? (8:00 on Monday, local time. In UTC that is 6:00 on Monday; before Sunday it would have been 5:00.)
Our bookings start at 8:00 at the earliest, so the hour that happens twice in the night (3:00 to 4:00) never touches them.
-->

---
layout: two-cols
---

# You already use inheritance

```cs
public class BookingsController : ControllerBase
{
    ...
    return Ok(response);
    return BadRequest(error);
    return NotFound(error);
    return Conflict(error);
    return StatusCode(403, error);
}
```

::right::

<div class="pl-6 pt-22">

`Ok`, `BadRequest`, `NotFound`, `Conflict` and `StatusCode` are not ours. They are methods of `ControllerBase`, and every controller inherits them, like `Movie` inherits from `Media` in today's lab.

</div>

<!--
If someone asks: there is also a Controller class (with views, for web pages that the server renders). ControllerBase is the lighter base class for APIs that only return data.
-->

---

# A menu for the API

Remember the restaurant. OpenAPI is the printed menu: a file that lists every path, what each request needs, and what each answer looks like. Scalar is a page that reads the menu and lets you order from it.

```cs
builder.Services.AddOpenApi();

app.MapOpenApi();              // the menu, at /openapi/v1.json
app.MapScalarApiReference();   // the page, at /scalar
```

Both come from NuGet packages, libraries that `dotnet add package` downloads: `Microsoft.AspNetCore.OpenApi` and `Scalar.AspNetCore`. ASP.NET Core writes the menu from our controllers and records, so it is never out of date.

<!--
Open http://localhost:5080/openapi/v1.json first: a long JSON file. Then /scalar: the same information as a page, with a "Test Request" button on each endpoint.
Point out that BookingResponse appears in the menu with every field, and that Booking does not appear at all.
-->

---

# Trying it out

```text
POST 10:10-23:00 for zoe                     400  four problems at once
POST A101 (capacity 3) for ana, open         201
POST join mihai, join ioana                  200  full now
POST join radu                               409  the booking is full
GET  /timetable                              200  names only, no emails
POST leave ioana                             200
POST radu removes mihai                      403  only the owner
POST ana removes mihai                       200
POST ana cancels                             200  status Cancelled
```

<!--
SHOWCASE: run the API, open StudyRooms.Api.http, set @date to tomorrow, and send the requests from top to bottom. Then do the same few in Scalar.
For the cutoff: book a room for today starting less than 2 hours from now (the last two requests in the file), then cancel it: 409.
-->

---

# What is still wrong

- Stop the API and start it again: every booking is gone.
- Two requests that arrive at the same moment can still book the same room.
- Anyone can send `"userId": "ana"` and act as Ana.
- An owner cannot approve join requests yet (R12).

Next week the bookings move to a database, which fixes the first two. Accounts in week 6 fix the last two.

<AppLink tag="week-04" compare="week-03" />

<!--
The second point is next week's main topic. If someone wants a preview: two requests both check "is the room free?" before either one saves.
-->
