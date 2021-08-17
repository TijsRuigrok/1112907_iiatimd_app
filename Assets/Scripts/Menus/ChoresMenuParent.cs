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

    private List<ChoreData> choresData = new List<ChoreData>();
    private List<ChoreParent> chores = new List<ChoreParent>();

    private void OnEnable()
    {
        GetChores();
    }

    private void GetChores()
    {
        choresData = ProfileManager.Instance.currentProfile.choresData;

        /*
        string response = await APIManager.Instance.client.GetStringAsync(
            "http://127.0.0.1:8000/api/chores/self");

        List<Chore> chores = JsonConvert.DeserializeObject<List<Chore>>(response);
        */

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

    public async void Submit()
    {
        await AddChore(nameInput.text, pointsInput.text, dateInput.text);
    }

    private async Task AddChore(string name, string points, string date)
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

        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "name", name },
            { "points", points },
            { "date", date }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await APIManager.Instance.client.PostAsync(
            APIManager.BaseURL + "chores/create", content);
        response.EnsureSuccessStatusCode();

        GetChores();
    }

    public void RemoveChore(Guid id)
    {
        foreach(ChoreData chore in choresData.ToList())
        {
            if(chore.id == id)
            {
                ProfileManager.Instance.currentProfile.choresData.Remove(chore);
                ProfileManager.Instance.SaveProfile();
                break;
            }
        }

        ClearChores();
        GetChores();

        /*
        await APIManager.Instance.client.DeleteAsync(
            APIManager.BaseURL + "chores/self/delete/" + chore.id);
        */
    }

}
