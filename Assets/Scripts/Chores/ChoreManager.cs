using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoreManager
{
    public static ChoreData GetChore(Guid id)
    {
        foreach (ChoreData chore in GetChores().ToList())
        {
            if (chore.id == id)
            {
                return chore;
            }
        }
        return null;
    }

    public static List<ChoreData> GetChores()
    {
        return ProfileManager.Instance.currentProfile.choresData;
    }

    public static async void AddChore(string name, string points, string date)
    {
        int pointsInt = Int32.Parse(points);

        if (pointsInt < 0)
            return;

        ChoreData newChoreData = new ChoreData
        {
            name = name,
            points = pointsInt,
            date = date
        };
        ProfileManager.Instance.currentProfile.choresData.Add(newChoreData);
        ProfileManager.Instance.SaveProfile();

        await APIManager.Instance.AddChore(newChoreData);
    }
    
    public static async void RemoveChore(Guid id)
    {
        ProfileManager.Instance.currentProfile.choresData.Remove(GetChore(id));
        ProfileManager.Instance.SaveProfile();

        await APIManager.Instance.RemoveChore(id);
    }

    public static async void CompleteChore(Guid id)
    {
        ChoreData chore = GetChore(id);
        chore.completed = true;

        ProfileManager.Instance.SaveProfile();

        await APIManager.Instance.CompleteChore(id);
    }
}
