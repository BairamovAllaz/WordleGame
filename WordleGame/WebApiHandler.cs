using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WordleGame;

public class WebApiHandler
{
    private HttpClient _httpClient;
    private readonly string _path = "https://api.frontendexpert.io/api/fe/wordle-words";

    public WebApiHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string?> GetWords()
    {
        var responsemessage = await _httpClient.GetAsync(_path);
        if (responsemessage.IsSuccessStatusCode)
        {
            var stream = await responsemessage.Content.ReadAsStringAsync();
            return stream;
        }
        return string.Empty;
    }
    
    public static List<string> ConvertToList(string? streamToConvert)
    {
        var convertedResult = JsonConvert.DeserializeObject<List<string>>(streamToConvert);
        if (convertedResult is null)
        {
            throw new ArgumentNullException();
        }
        return convertedResult;
    }
}