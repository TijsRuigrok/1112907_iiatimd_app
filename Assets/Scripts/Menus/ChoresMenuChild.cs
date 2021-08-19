using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoresMenuChild : MonoBehaviour
{
    [SerializeField] private ChoreChild chorePrefab;
    [SerializeField] private GameObject content;

    private List<ChoreChild> chores = new List<ChoreChild>();

    void OnEnable()
    {
        GetChores();
    }

    private void GetChores()
    {
        List<ChoreData> choresData = ChoreManager.GetChores();

        if (chores.Count != 0)
            ClearChores();

        for (int i = 0; i < choresData.Count; i++)
        {
            chores.Add(Instantiate(chorePrefab, content.transform));
            chores[i].choreData = choresData[i];
            chores[i].CheckmarkValueChanged();
        }
    }

    private void ClearChores()
    {
        foreach (ChoreChild chore in chores)
            Destroy(chore.gameObject);

        chores.Clear();
    }
}
