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

    private List<PrizeParent> prizes = new List<PrizeParent>();

    private void OnEnable()
    {
        GetPrizes();
    }

    private void GetPrizes()
    {
        List<PrizeData> prizesData = PrizeManager.GetPrizes();

        if (prizes.Count != 0)
            ClearPrizes();

        for (int i = 0; i < prizesData.Count; i++)
        {
            prizes.Add(Instantiate(prizePrefab, content.transform));
            prizes[i].prizeData = prizesData[i];
            prizes[i].prizeMenu = this;
        }

        /*
        string response = await APIManager.Instance.client.GetStringAsync(
            "http://127.0.0.1:8000/api/prizes/self");

        List<Prize> prizes = JsonConvert.DeserializeObject<List<Prize>>(response);
        */
    }

    private void ClearPrizes()
    {
        foreach (PrizeParent prize in prizes)
            Destroy(prize.gameObject);

        prizes.Clear();
    }

    public async void AddPrize()
    {
        await PrizeManager.AddPrize(nameInput.text, pointsInput.text);

        GetPrizes();
    }

    public void RemovePrize(Guid id)
    {
        PrizeManager.RemovePrize(id);

        ClearPrizes();
        GetPrizes();

        /*
        await APIManager.Instance.client.DeleteAsync(
            APIManager.BaseURL + "prizes/self/delete/" + prize.id);
        */
    }
}
