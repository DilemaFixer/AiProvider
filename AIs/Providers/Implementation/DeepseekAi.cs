using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace AIs;

public class DeepseekAi : IAiProvider
{
    private HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiBaseUrl = "https://api.deepseek.com/v1";
    public string Model { get; private set; }
    
    public DeepseekAi(string model , string apiKey)
    {
        if(string.IsNullOrEmpty(model) || string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(model));
      
        Model = model;
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }
    
    public async Task<string> GetResponseAsync(Message[] story)
    {
        var requestBody = new
        {
            model = Model,
            messages = story
        };

        string json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Make the POST request to the DeepSeek API
        HttpResponseMessage response = await _httpClient.PostAsync($"{_apiBaseUrl}/chat/completions", content);

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string responseString = await response.Content.ReadAsStringAsync();

            // Deserialize the response JSON to a dynamic object
            dynamic responseData = JsonConvert.DeserializeObject(responseString);

            // Extract the generated text from the response
            string generatedText = responseData.choices[0].message.content;

            return generatedText;
        }
        else
        {
            throw new Exception($"Error: {response.StatusCode}");
        }
    }

    public void SetModel(string model)
    {
        if(string.IsNullOrEmpty(model)) throw new ArgumentNullException(nameof(model));
        Model = model;
    }

    public static class Models
    {
        public static string BaseModel = "deepseek-coder";
        public static string CoderModel = "deepseek-chat";
    }
}