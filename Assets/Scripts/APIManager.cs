using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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

    /// <summary>
    /// Refreshes JWT because it's only valid for a limited time.
    /// </summary>
    public async Task RefreshJWT()
    {
        string jwt = await client.GetStringAsync(BaseURL + "refresh");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }

    /// <summary>
    /// Retrieves JWT using login information.
    /// </summary>
    /// <param name="email">Users e-mailadres.</param>
    /// <param name="password">Users password.</param>
    /// <returns>Returns if login was succesful.</returns>
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

    /// <summary>
    /// Registers new account.
    /// </summary>
    /// <param name="email">Users e-mailadres.</param>
    /// <param name="password">Users password.</param>
    /// <returns>Returns if registration was succesful.</returns>
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

    /// <summary>
    /// Retrieves last date/time API was updated.
    /// </summary>
    /// <returns>Timestamp of last API update.</returns>
    public async Task<DateTime> GetLastUpdate()
    {
        string response = await client.GetStringAsync(
            BaseURL + "users/self/updated-at");

        CultureInfo cultureInfo = new CultureInfo("en-US");

        DateTime timestamp = DateTime.ParseExact(response, "yyyy-MM-dd HH:mm:ss", cultureInfo);
        
        return timestamp;
    }

    /// <summary>
    /// Edits last update of API.
    /// </summary>
    /// <param name="timestamp">Timestamp of last API update.</param>
    /// <returns>Timestamp of last API update.</returns>
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

    /// <summary>
    /// Retrieves points of user.
    /// </summary>
    /// <returns>Users points.</returns>
    public async Task<int> GetPoints()
    {
        string response = await client.GetStringAsync(BaseURL + "users/self/points");
        return Int32.Parse(response);
    }

    /// <summary>
    /// Edits points of user.
    /// </summary>
    /// <param name="points">Amount of points to replace points in API.</param>
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

    /// <summary>
    /// Adds chore to API.
    /// </summary>
    /// <param name="chore">Chore to be added.</param>
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

    /// <summary>
    /// Adds list of chores to API.
    /// </summary>
    /// <param name="chores">List of chores to be added.</param>
    public async void AddChores(List<ChoreData> chores)
    {
        foreach (ChoreData chore in chores)
        {
            await AddChore(chore);
        }
    }

    /// <summary>
    /// Removes chore from API.
    /// </summary>
    /// <param name="id">Id of chore to be removed.</param>
    public async Task RemoveChore(Guid id)
    {
        await client.DeleteAsync(BaseURL + $"chores/{id}");
    }

    /// <summary>
    /// Removes list of chores from API.
    /// </summary>
    /// <param name="chores">List of chores to be removed.</param>
    public async void RemoveChores(List<ChoreData> chores)
    {
        foreach (ChoreData chore in chores)
        {
            await RemoveChore(chore.id);
        }
    }

    /// <summary>
    /// Changes "completed" value of chore to true.
    /// </summary>
    /// <param name="id">Id of chore to be completed.</param>
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

    /// <summary>
    /// Retrieves all chores of user.
    /// </summary>
    /// <returns>List containing all chores.</returns>
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

    /// <summary>
    /// Adds prize to API.
    /// </summary>
    /// <param name="prize">Prize to be added.</param>
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

    /// <summary>
    /// Adds prize to API.
    /// </summary>
    /// <param name="prizes"></param>
    public async void AddPrizes(List<PrizeData> prizes)
    {
        foreach (PrizeData prize in prizes)
        {
            await AddPrize(prize);
        }
    }

    /// <summary>
    /// Removes prize from API.
    /// </summary>
    /// <param name="id">Id of prize to be removed.</param>
    public async Task RemovePrize(Guid id)
    {
        await client.DeleteAsync(BaseURL + $"prizes/{id}");
    }

    /// <summary>
    /// Removes list of prizes from API.
    /// </summary>
    /// <param name="prizes">List of prizes to be removed.</param>
    public async void RemovePrizes(List<PrizeData> prizes)
    {
        foreach (PrizeData prize in prizes)
        {
            await RemovePrize(prize.id);
        }
    }

    /// <summary>
    /// Changes "claimed" value of prize to true.
    /// </summary>
    /// <param name="id">Id of prize to be claimed.</param>
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

    /// <summary>
    /// Retrieves all prizes of user.
    /// </summary>
    /// <returns>List containing all prizes of user.</returns>
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