using Newtonsoft.Json;
using System.Collections.Generic;
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

    private List<PrizeParent> prizePrefabs = new List<PrizeParent>();

    private async void Start()
    {
        await GetPrizes();
    }

    private async Task GetPrizes()
    {
        string response = await APIManager.Instance.client.GetStringAsync(
            "http://127.0.0.1:8000/api/prizes/self");

        List<Prize> prizes = JsonConvert.DeserializeObject<List<Prize>>(response);
        
        for(int i = 0; i < prizes.Count; i++)
        {
            prizePrefabs.Add(Instantiate(prizePrefab, content.transform));
            prizePrefabs[i].prize = prizes[i];
        }
    }

    public async void Submit()
    {
        await AddPrize(nameInput.text, pointsInput.text);
    }

    private async Task AddPrize(string name, string points)
    {
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
}
