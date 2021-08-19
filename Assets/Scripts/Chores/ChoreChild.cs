using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoreChild : Chore
{
    [SerializeField] private Button checkmark;
    [SerializeField] private Sprite checkmarkCheckedImg;
    [SerializeField] private GameObject darkOverlay;

    public void CompleteChore()
    {
        ChoreManager.CompleteChore(choreData.id);
    }

    public void CheckmarkValueChanged()
    {
        if(choreData.completed)
        {
            checkmark.interactable = false;
            checkmark.image.sprite = checkmarkCheckedImg;
            darkOverlay.SetActive(true);
            nameText.fontStyle = FontStyles.Strikethrough;
            transform.SetAsLastSibling();
        }
    }

    public void SetPoints()
    {
        ProfileManager.Instance.SetPoints(choreData.points);
    }
}
