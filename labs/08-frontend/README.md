# Week 8: The timetable in the browser

There is no graded lab this week, only the optional part. The study rooms application gets a web page: the timetable for a day, booking a room, joining someone else's booking, your own bookings, and the admin pages. We use the page to watch the API we built in weeks 3 to 6 at work.

## The code

The page is in the `frontend` folder of the [study-rooms repository](https://github.com/RazvanGolan/study-rooms/tree/main/frontend). The code at the end of this week has the tag `week-08`, and [this comparison](https://github.com/RazvanGolan/study-rooms/compare/week-06...week-08) shows everything that changed since week 6.

You do not need to read the page's code for the lab. It is written in React with TypeScript, and every screen calls endpoints you already know.

## Running it

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download) and [Node.js](https://nodejs.org/) 20 or newer. From the `study-rooms` folder, start the API in one terminal:

```bash
dotnet run --project src/StudyRooms.Api
```

and the page in a second one:

```bash
cd frontend
npm install
npm run dev
```

Open http://localhost:5173 and click one of the sample accounts to log in. To be two students at once, open the page again in a private window and log in as someone else.

## Watching the requests

Open the developer tools (F12, or Cmd+Option+I on a Mac), go to the Network tab and pick Fetch/XHR. Every click that needs data shows up there as a request to the API. Click one to see its status code, the JSON the page sent and the JSON the API answered.

Some things worth trying:

- Book a time when you are already in another booking. The API answers 409 and the page shows its message.
- Look for the `OPTIONS` request before each call. That is the browser asking the API whether the page may call it (CORS).
- In the Application tab, under Local Storage, find `study-rooms.tokens`: the tokens the page sends with each request.

## Further reading

- [Network panel](https://developer.chrome.com/docs/devtools/network) in Chrome's developer tools
- [Cross-Origin Resource Sharing](https://developer.mozilla.org/en-US/docs/Web/HTTP/Guides/CORS) on MDN
- [Using the Fetch API](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch) on MDN
