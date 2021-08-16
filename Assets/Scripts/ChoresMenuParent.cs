using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ChoresMenuParent : MonoBehaviour
{
    [SerializeField] private ChoreParent choreParent;
    [SerializeField] private GameObject content;

    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField pointsInput;
    [SerializeField] private TMP_InputField dateInput;

    private List<ChoreParent> chorePrefabs = new List<ChoreParent>();

    private void Start()
    {
        GetChores();
    }

    private void GetChores()
    {
        List<Chore> chores = ProfileManager.Instance.currentProfile.chores;
        
        /*
        string response = await APIManager.Instance.client.GetStringAsync(
            "http://127.0.0.1:8000/api/chores/self");

        List<Chore> chores = JsonConvert.DeserializeObject<List<Chore>>(response);
        */

        if(chorePrefabs.Count != 0)
        {
            foreach (ChoreParent chorePrefab in chorePrefabs)
                Destroy(chorePrefab);

            chorePrefabs.Clear();
        }

        for (int i = 0; i < chores.Count; i++)
        {
            chorePrefabs.Add(Instantiate(choreParent, content.transform));
            chorePrefabs[i].chore = chores[i];
        }
    }

    public async void Submit()
    {
        await AddChore(nameInput.text, pointsInput.text, dateInput.text);
    }

    private async Task AddChore(string name, string points, string date)
    {
        int pointsInt = Int32.Parse(points);
        Chore newChore = new Chore
        {
            name = name,
            points = pointsInt,
            date = date
        };
        ProfileManager.Instance.currentProfile.chores.Add(newChore);
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

}
