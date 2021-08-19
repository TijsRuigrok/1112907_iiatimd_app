using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class PrizeManager
{
    public static PrizeData GetPrize(Guid id)
    {
        foreach (PrizeData prize in GetPrizes().ToList())
        {
            if (prize.id == id)
            {
                return prize;
            }
        }
        return null;        
    }

    public static List<PrizeData> GetPrizes()
    {
        return ProfileManager.Instance.currentProfile.prizesData;
    }

    public static async Task AddPrize(string name, string points)
    {
        int pointsInt = Int32.Parse(points);

        if (pointsInt < 0)
            return;

        PrizeData newPrize = new PrizeData
        {
            name = name,
            points = pointsInt
        };
        ProfileManager.Instance.currentProfile.prizesData.Add(newPrize);
        ProfileManager.Instance.SaveProfile();

        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "name", name },
            { "points", points }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await APIManager.Instance.client.PostAsync(
            APIManager.BaseURL + "prizes/create", content);
        response.EnsureSuccessStatusCode();
    }

    public static void RemovePrize(Guid id)
    {
        ProfileManager.Instance.currentProfile.prizesData.Remove(GetPrize(id));
        ProfileManager.Instance.SaveProfile();
    }

    public static void ClaimPrize(Guid id)
    {
        PrizeData prize = GetPrize(id);

        if (prize.claimed)
            prize.claimed = false;
        else
            prize.claimed = true;

        ProfileManager.Instance.SaveProfile();
    }
}
