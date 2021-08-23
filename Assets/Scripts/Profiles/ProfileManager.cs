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

    /// <summary>
    /// Retrieves all profiles from local storage.
    /// </summary>
    /// <returns>List containing all profiles.</returns>
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

    /// <summary>
    /// Sets profile that is currently used by application.
    /// </summary>
    /// <param name="currentProfileEmail">E-mailadres of profile that should be current.</param>
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

    /// <summary>
    /// Creates new profile and adds it to local storage.
    /// </summary>
    /// <param name="email">E-mailadres of new profile.</param>
    public void CreateProfile(string email)
    {
        Profile newProfile = new Profile
        {
            email = email
        };
        LocalStorageManager.SaveProfile(newProfile);

        currentProfile = newProfile;
    }

    /// <summary>
    /// Saves all profile data to local storage.
    /// </summary>
    public void SaveProfile()
    {
        LocalStorageManager.SaveProfile(currentProfile);
        SetLastUpdate();
    }

    /// <summary>
    /// Edits points of current profile.
    /// </summary>
    /// <param name="points">Points to be added.</param>
    public async void SetPoints(int points)
    {
        currentProfile.points += points;
        LocalStorageManager.SaveProfile(currentProfile);

        await APIManager.Instance.SetPoints(currentProfile.points);
    }

    /// <summary>
    /// Syncs data between API and local storage. Most up-to-date database is used.
    /// </summary>
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

    /// <summary>
    /// Sets timestamp of last time API and local storage were updated.
    /// </summary>
    public async void SetLastUpdate()
    {
        DateTime currentTime = DateTime.Now;
        currentProfile.lastUpdate = currentTime;
        await APIManager.Instance.SetLastUpdate(currentTime);
    }
}