using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizesMenuParent : MonoBehaviour
{
    [SerializeField] private PrizeParent prizePrefab;
    [SerializeField] private GameObject content;

    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField pointsInput;

    private List<PrizeData> prizesData = new List<PrizeData>();
    private List<PrizeParent> prizes = new List<PrizeParent>();

    private void OnEnable()
    {
        GetPrizes();
    }

    private void GetPrizes()
    {
        prizesData = ProfileManager.Instance.currentProfile.prizesData;

        /*
        string response = await APIManager.Instance.client.GetStringAsync(
            "http://127.0.0.1:8000/api/prizes/self");

        List<Prize> prizes = JsonConvert.DeserializeObject<List<Prize>>(response);
        */

        if (prizes.Count != 0)
            ClearPrizes();

        for (int i = 0; i < prizesData.Count; i++)
        {
            prizes.Add(Instantiate(prizePrefab, content.transform));
            prizes[i].prizeData = prizesData[i];
            prizes[i].prizeMenu = this;
        }
    }

    private void ClearPrizes()
    {
        foreach (PrizeParent prize in prizes)
            Destroy(prize.gameObject);

        prizes.Clear();
    }

    public async void Submit()
    {
        await AddPrize(nameInput.text, pointsInput.text);
    }

    private async Task AddPrize(string name, string points)
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

        GetPrizes();
    }

    public void RemovePrize(Guid id)
    {
        foreach (PrizeData prize in prizesData.ToList())
        {
            if (prize.id == id)
            {
                ProfileManager.Instance.currentProfile.prizesData.Remove(prize);
                ProfileManager.Instance.SaveProfile();
                break;
            }
        }

        ClearPrizes();
        GetPrizes();

        /*
        await APIManager.Instance.client.DeleteAsync(
            APIManager.BaseURL + "prizes/self/delete/" + prize.id);
        */
    }
}
