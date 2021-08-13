using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputEmail;
    [SerializeField] private TMP_InputField inputPassword;

    public async void Submit()
    {
        await APIManager.Instance.Login(inputEmail.text, inputPassword.text);
    }
}
