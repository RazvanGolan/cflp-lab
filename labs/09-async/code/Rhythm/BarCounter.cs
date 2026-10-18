namespace Rhythm;

public class BarCounter
{
    private readonly int beatsPerBar;

    public int Bars { get; private set; }

    public BarCounter(int beatsPerBar)
    {
        this.beatsPerBar = beatsPerBar;
    }

    public void OnBeat(int beat)
    {
        if (beat % beatsPerBar == 0)
        {
            Bars++;
        }
    }
}
