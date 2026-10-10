namespace Music;

public class Player
{
    private readonly List<Song> queue = [];

    public event Action<Song>? SongStarted;
    public event Action? QueueFinished;

    public void Add(Song song)
    {
        queue.Add(song);
    }

    public void PlayAll()
    {
        foreach (Song song in queue)
        {
            SongStarted?.Invoke(song);
        }
        queue.Clear();
        QueueFinished?.Invoke();
    }
}
