using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputEmail;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private SceneLoader sceneLoader;

    public async void Submit()
    {
        if (await APIManager.Instance.Login(inputEmail.text, inputPassword.text) == true)
            sceneLoader.LoadScene("SelectRoleScene");
    }
}
