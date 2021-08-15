using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

public class APIManager
{
    private static APIManager _instance;
    public static APIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new APIManager();
            return _instance;
        }
    }

    public HttpClient client = new HttpClient();
    public string jwt;
    public const string BaseURL = "http://127.0.0.1:8000/api/";

    public async Task<bool> Login(string email, string password)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "email", email },
            { "password", password }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await client.PostAsync(BaseURL + "login", content);
        response.EnsureSuccessStatusCode();

        string responseStr = await response.Content.ReadAsStringAsync();

        JToken token = JObject.Parse(responseStr);
        jwt = (string)token.SelectToken("token");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        if (response.IsSuccessStatusCode)
            return true;
        
        else return false;
    }

    public async Task Refresh()
    {
        jwt = await client.GetStringAsync(BaseURL + "refresh");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }
}
