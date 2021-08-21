using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ChoresMenuParent : MonoBehaviour
{
    [SerializeField] private ChoreParent chorePrefab;
    [SerializeField] private GameObject content;

    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField pointsInput;
    [SerializeField] private TMP_InputField dateInput;

    private List<ChoreParent> chores = new List<ChoreParent>();

    private void OnEnable()
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
            chores[i].choresMenu = this;
        }
    }

    private void ClearChores()
    {
        foreach (ChoreParent chore in chores)
            Destroy(chore.gameObject);

        chores.Clear();
    }

    public void AddChore()
    {
        ChoreManager.AddChore(nameInput.text, pointsInput.text, dateInput.text);
        GetChores();
    }

    public void RemoveChore(Guid id)
    {
        ChoreManager.RemoveChore(id);

        ClearChores();
        GetChores();
    }
}
