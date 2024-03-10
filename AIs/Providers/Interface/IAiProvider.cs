namespace AIs;

public interface IAiProvider
{
    public string Model { get; }
    public Task<string> GetResponseAsync(Message[] story);
    public void SetModel(string model);
}