namespace projectX.Services
{
    public interface IOllamaService
    {
        Task<string> AskAsync(string prompt);
    }
}