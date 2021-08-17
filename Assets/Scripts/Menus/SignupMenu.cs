using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SignupMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private MenuManager menuManager;

    public async void Submit()
    {
        await Register(emailInput.text, passwordInput.text);
    }

    private async Task Register(string email, string password)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "email", email },
            { "password", password }
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await APIManager.Instance.client.PostAsync(
            APIManager.BaseURL + "register", content);
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            ProfileManager.Instance.CreateProfile(email);
            menuManager.OpenMenu("login menu");
        }
    }
}
