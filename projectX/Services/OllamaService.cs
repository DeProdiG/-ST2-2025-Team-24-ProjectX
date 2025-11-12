using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace projectX.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _ollamaUrl = "http://localhost:11434/api/generate";

        public OllamaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AskAsync(string prompt)
        {
            var requestBody = new
            {
                model = "llama3", // or your preferred model
                prompt = prompt,
                stream = false
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await _httpClient.PostAsync(_ollamaUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    return $"❌ Error from Ollama: {response.StatusCode}. Please make sure Ollama is running on localhost:11434.";
                }

                var responseText = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                try
                {
                    using var doc = JsonDocument.Parse(responseText);
                    var text = doc.RootElement.GetProperty("response").GetString();
                    return text ?? "(no response)";
                }
                catch
                {
                    return $"Raw Ollama output: {responseText}";
                }
            }
            catch (HttpRequestException)
            {
                return "❌ Cannot connect to Ollama. Please make sure Ollama is installed and running on localhost:11434.";
            }
        }
    }
}