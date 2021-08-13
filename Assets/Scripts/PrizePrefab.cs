using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrizePrefab : MonoBehaviour
{
    public string name;
    public int points;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text pointsText;

    void Start()
    {
        nameText.text = name;
        pointsText.text = points.ToString();
    }
}
