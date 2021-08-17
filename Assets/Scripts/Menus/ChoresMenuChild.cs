using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoresMenuChild : MonoBehaviour
{
    [SerializeField] private ChoreChild chorePrefab;
    [SerializeField] private GameObject content;

    private List<ChoreData> choresData = new List<ChoreData>();
    private List<ChoreChild> chores = new List<ChoreChild>();

    void OnEnable()
    {
        GetChores();
    }

    private void GetChores()
    {
        choresData = ProfileManager.Instance.currentProfile.choresData;

        if (chores.Count != 0)
            ClearChores();

        for (int i = 0; i < choresData.Count; i++)
        {
            chores.Add(Instantiate(chorePrefab, content.transform));
            chores[i].choreData = choresData[i];
        }
    }

    private void ClearChores()
    {
        foreach (ChoreChild chore in chores)
            Destroy(chore.gameObject);

        chores.Clear();
    }
}
