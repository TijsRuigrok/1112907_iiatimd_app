using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrizesMenuChild : MonoBehaviour
{
    [SerializeField] private PrizeChild prizePrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private TMP_Text pointsText;

    private List<PrizeChild> prizes = new List<PrizeChild>();

    void OnEnable()
    {
        GetPrizes();
        GetPoints();
    }

    private void GetPrizes()
    {
        List<PrizeData> prizesData = PrizeManager.GetPrizes();

        if (prizes.Count != 0)
            ClearChores();

        for (int i = 0; i < prizesData.Count; i++)
        {
            prizes.Add(Instantiate(prizePrefab, content.transform));
            prizes[i].prizeData = prizesData[i];
            prizes[i].menu = this;
            prizes[i].CheckmarkValueChanged();
        }
    }

    private void ClearChores()
    {
        foreach (PrizeChild prize in prizes)
            Destroy(prize.gameObject);

        prizes.Clear();
    }

    public void GetPoints()
    {
        pointsText.text = ProfileManager.Instance.currentProfile.points.ToString();
    }
}
