namespace Rhythm;

public class Metronome
{
    public int Tempo { get; }

    public event Action<int>? Beat;
    public event Action? Stopped;

    public Metronome(int tempo)
    {
        Tempo = tempo;
    }

    public async Task StartAsync(CancellationToken token)
    {
        int count = 0;
        while (!token.IsCancellationRequested)
        {
            count++;
            Beat?.Invoke(count);
            await Task.Delay(60000 / Tempo);
        }
        Stopped?.Invoke();
    }
}
