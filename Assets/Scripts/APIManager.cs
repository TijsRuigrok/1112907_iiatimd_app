using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public const string BaseURL = "https://iiatimd-chore-manager.herokuapp.com/api/";

    #region Authentication

    public async Task RefreshJWT()
    {
        string jwt = await client.GetStringAsync(BaseURL + "refresh");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }

    public async Task<bool> Login(string email, string password)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "email", email },
            { "password", password }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await client.PostAsync(
            BaseURL + "login", content);

        response.EnsureSuccessStatusCode();

        string responseStr = await response.Content.ReadAsStringAsync();

        JToken token = JObject.Parse(responseStr);
        string jwt = (string)token.SelectToken("token");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jwt);

        if (response.IsSuccessStatusCode)
            return true;

        else
            return false;
    }

    public async Task<bool> Register(string email, string password)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "email", email },
            { "password", password }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await client.PostAsync(
            BaseURL + "register", content);
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
            return true;

        else
            return false;
    }

    #endregion

    #region Updated At

    public async Task<DateTime> GetLastUpdate()
    {
        string response = await client.GetStringAsync(
            BaseURL + "users/self/updated-at");

        CultureInfo cultureInfo = new CultureInfo("en-US");

        DateTime timestamp = DateTime.ParseExact(response, "yyyy-MM-dd HH:mm:ss", cultureInfo);
        
        return timestamp;
    }

    public async Task SetLastUpdate(DateTime timestamp)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "updated_at", timestamp.ToString("yyyy-MM-dd HH:mm:ss") },
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        
        HttpResponseMessage response = await client.PutAsync(
            BaseURL + "users/self/updated-at", content);
        
        response.EnsureSuccessStatusCode();
    }

    #endregion

    #region Points

    public async Task<int> GetPoints()
    {
        string response = await client.GetStringAsync(BaseURL + "users/self/points");
        return Int32.Parse(response);
    }

    public async Task SetPoints(int points)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "points", points.ToString() },
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);

        HttpResponseMessage response = await client.PutAsync(
            BaseURL + "users/self/points", content);

        response.EnsureSuccessStatusCode();
    }

    #endregion

    #region Chores

    public async Task AddChore(ChoreData chore)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "guid", chore.id.ToString() },
            { "name", chore.name },
            { "points", chore.points.ToString() },
            { "date", chore.date }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await client.PostAsync(
            BaseURL + "chores", content);
        response.EnsureSuccessStatusCode();
    }

    public async void AddChores(List<ChoreData> chores)
    {
        foreach (ChoreData chore in chores)
        {
            await AddChore(chore);
        }
    }

    public async Task RemoveChore(Guid id)
    {
        await client.DeleteAsync(BaseURL + $"chores/{id}");
    }

    public async void RemoveChores(List<ChoreData> chores)
    {
        foreach (ChoreData chore in chores)
        {
            await RemoveChore(chore.id);
        }
    }

    public async Task CompleteChore(Guid id)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "completed", "1" }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);

        HttpResponseMessage response = await client.PutAsync(
            BaseURL + $"chores/{id}/completed", content);

        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ChoreData>> GetChores()
    {
        string response = await client.GetStringAsync(
            BaseURL + "chores/self");

        // Workaround, can't parse to class with Guid
        List<ChoreIdStr> choresIdStr = JsonConvert.DeserializeObject<List<ChoreIdStr>>(response);

        List<ChoreData> chores = new List<ChoreData>();

        foreach (ChoreIdStr chore in choresIdStr)
        {
            chores.Add(new ChoreData
            {
                id = new Guid(chore.guid),
                name = chore.name,
                points = chore.points,
                date = chore.date,
                completed = chore.completed,
                userId = chore.userId
            }); 
        }

        return chores;
    }

    #endregion

    #region Prizes

    public async Task AddPrize(PrizeData prize)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "guid", prize.id.ToString() },
            { "name", prize.name },
            { "points", prize.points.ToString() },
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await client.PostAsync(
            BaseURL + "prizes", content);
        response.EnsureSuccessStatusCode();
    }

    public async void AddPrizes(List<PrizeData> prizes)
    {
        foreach (PrizeData prize in prizes)
        {
            await AddPrize(prize);
        }
    }

    public async Task RemovePrize(Guid id)
    {
        await client.DeleteAsync(BaseURL + $"prizes/{id}");
    }

    public async void RemovePrizes(List<PrizeData> prizes)
    {
        foreach (PrizeData prize in prizes)
        {
            await RemovePrize(prize.id);
        }
    }

    public async Task ClaimPrize(Guid id)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "claimed", "1" }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);

        HttpResponseMessage response = await client.PutAsync(
            BaseURL + $"prizes/{id}/claimed", content);

        response.EnsureSuccessStatusCode();
    }

    public async Task<List<PrizeData>> GetPrizes()
    {
        string response = await client.GetStringAsync(
            BaseURL + "prizes/self");

        // Workaround, can't parse to class with Guid
        List<PrizeIdStr> prizesIdStr = JsonConvert.DeserializeObject<List<PrizeIdStr>>(response);

        List<PrizeData> prizes = new List<PrizeData>();

        foreach (PrizeIdStr prize in prizesIdStr)
        {
            prizes.Add(new PrizeData
            {
                id = new Guid(prize.guid),
                name = prize.name,
                points = prize.points,
                claimed = prize.claimed,
                userId = prize.userId
            });
        }

        return prizes;
    }

    #endregion
}

[Serializable]
class ChoreIdStr
{
    public string id;
    public string guid;
    public string name;
    public int points;
    public string date;
    public bool completed = false;
    public int userId;
}

[Serializable]
class PrizeIdStr
{
    public string id;
    public string guid;
    public string name;
    public int points;
    public bool claimed = false;
    public int userId;
}