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
    [SerializeField] private SceneLoader sceneLoader;

    private string email;
    private string password;

    public async void Register()
    {
        email = emailInput.text;
        password = passwordInput.text;

        bool registrationSuccesful = await APIManager.Instance.Register(
            email, password);

        if (registrationSuccesful)
        {
            ProfileManager.Instance.CreateProfile(emailInput.text);
            await APIManager.Instance.Login(email, password);
            ProfileManager.Instance.SetLastUpdate();
            sceneLoader.LoadScene("SelectRoleScene");
        }
    }
}
