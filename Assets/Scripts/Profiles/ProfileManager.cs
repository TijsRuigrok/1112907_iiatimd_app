using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProfileManager
{
    private static ProfileManager _instance;
    public static ProfileManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ProfileManager();
            return _instance;
        }
    }

    public Profile currentProfile;

    private List<Profile> GetAllProfiles()
    {
        List<Profile> profiles = new List<Profile>();

        foreach (string profilePath in LocalStorageManager.GetAllProfilePaths())
        {
            Profile profile = LocalStorageManager.LoadProfile(profilePath);
            profiles.Add(profile);
        }

        return profiles;
    }

    public void SetCurrentProfile(string currentProfileEmail)
    {
        foreach (Profile profile in GetAllProfiles())
        {
            if (profile.email == currentProfileEmail)
            {
                currentProfile = profile;
                break;
            }
        }

        if (currentProfile is null)
            CreateProfile(currentProfileEmail);
    }

    public void CreateProfile(string email)
    {
        Profile newProfile = new Profile
        {
            email = email
        };
        LocalStorageManager.SaveProfile(newProfile);

        currentProfile = newProfile;
    }

    public void SaveProfile()
    {
        LocalStorageManager.SaveProfile(currentProfile);
        SetLastUpdate();
    }

    public async void SetPoints(int points)
    {
        currentProfile.points += points;
        LocalStorageManager.SaveProfile(currentProfile);

        await APIManager.Instance.SetPoints(currentProfile.points);
    }

    public async void SyncData()
    {
        DateTime lastUpdateAPI = await APIManager.Instance.GetLastUpdate();
        DateTime lastUpdateLocal = currentProfile.lastUpdate;

        int result = DateTime.Compare(lastUpdateAPI, lastUpdateLocal);

        // Local is more up-to-date
        if (result < 0)
        {

            APIManager.Instance.RemoveChores(currentProfile.choresData);
            APIManager.Instance.AddChores(currentProfile.choresData);

            APIManager.Instance.RemovePrizes(currentProfile.prizesData);
            APIManager.Instance.AddPrizes(currentProfile.prizesData);

            await APIManager.Instance.SetPoints(currentProfile.points);
        }

        // API is more up-to-date
        else if (result > 0)
        {
            currentProfile.points = await APIManager.Instance.GetPoints();

            currentProfile.choresData.Clear();
            currentProfile.choresData = await APIManager.Instance.GetChores();

            currentProfile.prizesData.Clear();
            currentProfile.prizesData = await APIManager.Instance.GetPrizes();
        }
    }

    public async void SetLastUpdate()
    {
        DateTime currentTime = DateTime.Now;
        currentProfile.lastUpdate = currentTime;
        await APIManager.Instance.SetLastUpdate(currentTime);
    }
}