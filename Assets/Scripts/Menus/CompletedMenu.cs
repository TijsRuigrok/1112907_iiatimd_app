using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompletedMenu : MonoBehaviour
{
    [SerializeField] private ChoreParentCompleted chorePrefab;
    [SerializeField] private PrizeParentClaimed prizePrefab;

    [SerializeField] private GameObject contentChores;
    [SerializeField] private GameObject contentPrizes;

    private List<ChoreData> choresData = new List<ChoreData>();
    private List<ChoreParentCompleted> chores = new List<ChoreParentCompleted>();

    private List<PrizeData> prizesData = new List<PrizeData>();
    private List<PrizeParentClaimed> prizes = new List<PrizeParentClaimed>();

    void OnEnable()
    {
        GetCompletedChores();
        GetClaimedPrizes();
    }

    private void GetCompletedChores()
    {
        choresData = ProfileManager.Instance.currentProfile.choresData;

        if (chores.Count != 0)
            ClearChores();

        for (int i = 0; i < choresData.Count; i++)
        {
            if(choresData[i].completed)
            {
                chores.Add(Instantiate(chorePrefab, contentChores.transform));
                chores[i].choreData = choresData[i];
                chores[i].completedMenu = this;
            }
        }
    }

    private void GetClaimedPrizes()
    {
        prizesData = ProfileManager.Instance.currentProfile.prizesData;

        if (prizes.Count != 0)
            ClearPrizes();

        for (int i = 0; i < prizesData.Count; i++)
        {
            if (prizesData[i].claimed)
            {
                prizes.Add(Instantiate(prizePrefab, contentPrizes.transform));
                prizes[i].prizeData = prizesData[i];
                prizes[i].completedMenu = this;
            }
        }
    }

    private void ClearChores()
    {
        foreach (ChoreParentCompleted chore in chores)
            Destroy(chore.gameObject);

        chores.Clear();
    }

    private void ClearPrizes()
    {
        foreach (PrizeParentClaimed prize in prizes)
            Destroy(prize.gameObject);

        prizes.Clear();
    }

    internal void RemoveChore(Guid id)
    {
        foreach (ChoreData chore in choresData.ToList())
        {
            if (chore.id == id)
            {
                ProfileManager.Instance.currentProfile.choresData.Remove(chore);
                ProfileManager.Instance.SaveProfile();
                break;
            }
        }

        ClearChores();
        GetCompletedChores();
    }

    internal void RemovePrize(Guid id)
    {
        foreach (PrizeData prize in prizesData.ToList())
        {
            if (prize.id == id)
            {
                ProfileManager.Instance.currentProfile.prizesData.Remove(prize);
                ProfileManager.Instance.SaveProfile();
                break;
            }
        }

        ClearPrizes();
        GetClaimedPrizes();
    }
}
