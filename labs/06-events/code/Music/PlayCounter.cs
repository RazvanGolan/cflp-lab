namespace Music;

public class PlayCounter
{
    public int Played { get; private set; }
    public int Seconds { get; private set; }

    public void OnSongStarted(Song song)
    {
        Played++;
        Seconds += song.Seconds;
    }
}
