using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Prize : MonoBehaviour
{
    [HideInInspector] public PrizeData prizeData;

    [SerializeField] protected TMP_Text nameText;
    [SerializeField] protected TMP_Text pointsText;

    void Start()
    {
        nameText.text = prizeData.name;
        pointsText.text = prizeData.points.ToString();
    }
}
