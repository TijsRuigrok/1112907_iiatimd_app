using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LocalStorageManager
{
    private static readonly string profilesDirectory = Application.persistentDataPath + "/profiles";

    public static void SaveProfile(Profile profile)
    {
        if (!Directory.Exists(profilesDirectory))
            Directory.CreateDirectory(profilesDirectory);

        string path = $"{profilesDirectory}/{profile.email}.data";

        FileStream stream = new FileStream(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, profile);
        stream.Close();
    }

    public static Profile LoadProfile(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            Profile profile = formatter.Deserialize(stream) as Profile;
            stream.Close();

            return profile;
        }
        else
        {
            throw new FileLoadException("Unable To Find Profile");
        }
    }

    public static string[] GetAllProfilePaths()
    {
        return Directory.GetFiles(profilesDirectory, "*.data");
    }
}
