using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;

public class LoadClaimedPrizes : MonoBehaviour
{
    HttpClient client = new HttpClient();

    Dictionary<string, string> values = new Dictionary<string, string>
    {
        { "email", "tijs@gmail.com" },
        { "password", "password" }
    };

    void Start()
    {
        ReceivePrizes();
    }

    private async void ReceivePrizes()
    {
        /*
        string BASE_URL = "http://127.0.0.1:8000/api/";

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await client.PostAsync(BASE_URL + "login", content);
        response.EnsureSuccessStatusCode();

        string jwt = await response.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        */

        //APIManager.Instance.Login("tijs@gmail.com", "password");
        //APIManager.Instance.Refresh();
        //string r = await APIManager.Instance.client.GetStringAsync("http://127.0.0.1:8000/api/prizes/self/claimed");
        //print(r);

        try
        {
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://127.0.0.1:8000/api/login", content);
            var responseString = await response.Content.ReadAsStringAsync();
            print(responseString);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseString);

            //string claimedPrizes = await client.GetStringAsync("http://127.0.0.1:8000/api/prizes/self/claimed");
            //print(claimedPrizes);

            string jwt = await client.GetStringAsync("http://127.0.0.1:8000/api/refresh");
            print(jwt);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }
        catch (HttpRequestException e)
        {
            print("\nException Caught!");
            print("Message :{0} " +  e.Message);
        }
    }

}
