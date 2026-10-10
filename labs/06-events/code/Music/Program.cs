using Music;

Player player = new Player();
player.Add(new Song("Imagine", "John Lennon", 183));
player.Add(new Song("Yesterday", "The Beatles", 125));

player.SongStarted += song => Console.WriteLine($"Now playing: {song}");

PlayCounter counter = new PlayCounter();
player.SongStarted += counter.OnSongStarted;

player.QueueFinished += () =>
    Console.WriteLine($"Done: {counter.Played} songs, {counter.Seconds} s");

player.PlayAll();
Console.WriteLine();

player.SongStarted -= counter.OnSongStarted;
player.Add(new Song("Hey Jude", "The Beatles", 431));
player.PlayAll();

// Outside Player, an event only allows += and -=. Both lines give error CS0070.
// player.SongStarted = null;
// player.SongStarted(new Song("Imagine", "John Lennon", 183));
