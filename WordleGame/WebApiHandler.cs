using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WordleGame;

public class WebApiHandler
{
    private List<string> _words;
    private HttpClient _httpClient;
    private readonly string _path = "https://api.frontendexpert.io/api/fe/wordle-words";

    public WebApiHandler()
    {
        _words = new List<string>();
        _httpClient = new HttpClient();
    }

    public async Task<List<string>> GetWords()
    {
        HttpResponseMessage responseMessage = await _httpClient.GetAsync(_path);
        string json;
        if (responseMessage.IsSuccessStatusCode)
        {
            json = await responseMessage.Content.ReadAsStringAsync();
            _words = JsonConvert.DeserializeObject<List<string>>(json);
        }
        return _words;
    }
}