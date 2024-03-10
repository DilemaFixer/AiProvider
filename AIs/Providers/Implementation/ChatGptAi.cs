using System.Text;
using Newtonsoft.Json;

namespace AIs;

public class ChatGptAi : IAiProvider
{
    public string Model { get; private set; }
    
    private HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiBaseUrl = "https://api.openai.com/v1";
    
    public ChatGptAi(string model , string apiKey)
    {
        if(string.IsNullOrEmpty(model) || string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(model));
      
        Model = model;
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<string> GetResponseAsync(Message[] story)
    {
        var requestBody = new
        {
            model = Model,
            messages = story
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_apiBaseUrl}/chat/completions", content);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic responseData = JsonConvert.DeserializeObject(responseString);
            return responseData.choices[0].message.content;
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
        public const string BASE_CHAT_GPT_3_5_TURBO = "gpt-3.5-turbo";
        public const string BASE_CHAT_GPT_3_5 = "babbage-002";
        public const string BASE_CHAT_GPT_4 = "gpt-4";
        
        public static readonly string[] GTP_3_5_TURBO = new string[]
        {
            "gpt-3.5-turbo-0125" ,
            "gpt-3.5-turbo" ,
            "gpt-3.5-turbo-1106" , 
            "gpt-3.5-turbo-instruct" ,
            "gpt-3.5-turbo-16k",
            "gpt-3.5-turbo-0613",
            "gpt-3.5-turbo-16k-0613"
        };
        
        public static readonly string[] GTP_4_TURBO = new string[]
        {
            "gpt-4-0125-preview" ,
            "gpt-4-turbo-preview" ,
            "gpt-4-1106-preview" , 
            "gpt-4-vision-preview" ,
            "gpt-4-1106-vision-preview"
        };
        
        public static readonly string[] GTP_4 = new string[]
        {
            "gpt-4",
            "gpt-4-0613",
            "gpt-4-32k", 
            "gpt-4-32k-0613" 
        };
    }
}