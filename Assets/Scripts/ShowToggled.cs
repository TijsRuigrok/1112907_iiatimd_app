using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowToggled : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private Toggle checkmark;

    private bool checkmarkChecked = false;

    void Start()
    {
        checkmark.onValueChanged.AddListener(delegate {
            CheckmarkValueChanged();
        });
    }

    void CheckmarkValueChanged()
    {
        if(checkmarkChecked)
        {
            darkOverlay.SetActive(false);
            text.fontStyle = FontStyles.Normal;
            transform.SetAsFirstSibling();
            checkmarkChecked = false;
        }
        else
        {
            darkOverlay.SetActive(true);
            text.fontStyle = FontStyles.Strikethrough;
            transform.SetAsLastSibling();
            checkmarkChecked = true;
        }
    }
}
