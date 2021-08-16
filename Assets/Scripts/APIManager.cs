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
    public const string BaseURL = "http://127.0.0.1:8000/api/";

    public async Task Refresh()
    {
        string jwt = await client.GetStringAsync(BaseURL + "refresh");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }
}
