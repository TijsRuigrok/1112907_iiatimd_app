using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrizes : MonoBehaviour
{
    [SerializeField] private PrizePrefab prizePrefab;
    [SerializeField] private GameObject content;

    private List<PrizePrefab> prizePrefabs = new List<PrizePrefab>();

    private async void Start()
    {
        await GetPrizes();
    }

    private async Task GetPrizes()
    {
        await APIManager.Instance.Login("tijs@gmail.com", "password");

        string response = await APIManager.Instance.client.GetStringAsync(
            "http://127.0.0.1:8000/api/prizes/self");

        List<Prize> prizes = JsonConvert.DeserializeObject<List<Prize>>(response);
        
        for(int i = 0; i < prizes.Count; i++)
        {
            prizePrefabs.Add(Instantiate(prizePrefab, content.transform));
            prizePrefabs[i].transform.SetParent(content.transform);
            prizePrefabs[i].name = prizes[i].Name;
            prizePrefabs[i].points = prizes[i].Points;
        }
    }


}
