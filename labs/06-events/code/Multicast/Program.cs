Action<string>? log = ToScreen;
log += Shout;
log("hello");

log -= ToScreen;
log?.Invoke("again");

log -= Shout;
log?.Invoke("nobody hears this");
Console.WriteLine(log == null);

// log is null now, so this throws NullReferenceException.
// log("crash");

void ToScreen(string text)
    => Console.WriteLine($"screen: {text}");
void Shout(string text)
    => Console.WriteLine($"shout: {text.ToUpper()}");
