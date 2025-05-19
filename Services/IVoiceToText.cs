namespace TodoCodexPoc.Services;

public interface IVoiceToText
{
    Task<string> TranscribeAsync();
}
