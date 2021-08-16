using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if(currentProfile is null)
            CreateProfile(currentProfileEmail);
    }

    public void CreateProfile(string email)
    {
        Profile newProfile = new Profile
        {
            email = email
        };
        LocalStorageManager.SaveProfile(newProfile);
    }

    public void SaveProfile()
    {
        LocalStorageManager.SaveProfile(currentProfile);
    }
}
