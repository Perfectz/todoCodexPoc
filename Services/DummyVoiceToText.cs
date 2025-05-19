namespace TodoCodexPoc.Services;

public class DummyVoiceToText : IVoiceToText
{
    public Task<string> TranscribeAsync()
    {
        return Task.FromResult("Buy milk");
    }
}
