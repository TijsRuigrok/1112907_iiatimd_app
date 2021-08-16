using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LoginMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputEmail;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private SceneLoader sceneLoader;

    public async void Submit()
    {
        await Login(inputEmail.text, inputPassword.text);        
    }

    public async Task Login(string email, string password)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "email", email },
            { "password", password }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await APIManager.Instance.client.PostAsync(
            APIManager.BaseURL + "login", content);
        
        response.EnsureSuccessStatusCode();

        string responseStr = await response.Content.ReadAsStringAsync();

        JToken token = JObject.Parse(responseStr);
        string jwt = (string)token.SelectToken("token");

        APIManager.Instance.client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", jwt);

        if (response.IsSuccessStatusCode)
        {
            ProfileManager.Instance.SetCurrentProfile(email);
            sceneLoader.LoadScene("SelectRoleScene");
        }
    }
}
