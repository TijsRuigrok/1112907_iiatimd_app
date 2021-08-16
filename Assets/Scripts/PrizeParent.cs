using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrizeParent : MonoBehaviour
{
    public Prize prize;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text pointsText;

    void Start()
    {
        nameText.text = prize.name;
        pointsText.text = prize.points.ToString();
    }

    public async void DeletePrize()
    {
        await APIManager.Instance.client.DeleteAsync(
            APIManager.BaseURL + "prizes/self/delete/" + prize.id);
    }
}
