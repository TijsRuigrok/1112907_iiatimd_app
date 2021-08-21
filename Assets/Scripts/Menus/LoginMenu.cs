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

    public async void Login()
    {
        bool loginSuccesful = await APIManager.Instance.Login(
            inputEmail.text, inputPassword.text);

        if (loginSuccesful)
        {
            ProfileManager.Instance.SetCurrentProfile(inputEmail.text);
            ProfileManager.Instance.SyncData();
            sceneLoader.LoadScene("SelectRoleScene");
        }
    }
}
