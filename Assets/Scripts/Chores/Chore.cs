using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Chore : MonoBehaviour
{
    [HideInInspector] public ChoreData choreData;

    [SerializeField] protected TMP_Text nameText;
    [SerializeField] protected TMP_Text pointsText;
    [SerializeField] protected TMP_Text dateText;

    void Start()
    {
        nameText.text = choreData.name;
        pointsText.text = choreData.points.ToString();
        dateText.text = choreData.date;
    }
}
