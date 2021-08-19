using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeChild : Prize
{
    [HideInInspector] public PrizesMenuChild menu;

    [SerializeField] private Image checkmark;
    [SerializeField] private Sprite checkmarkCheckedImg;
    [SerializeField] private Sprite checkmarkUncheckedImg;
    [SerializeField] private GameObject darkOverlay;

    public void ClaimPrize()
    {
        PrizeManager.ClaimPrize(prizeData.id);
    }

    public void CheckmarkValueChanged()
    {
        if (prizeData.claimed)
        {
            checkmark.sprite = checkmarkCheckedImg;
            darkOverlay.SetActive(true);
            nameText.fontStyle = FontStyles.Strikethrough;
            transform.SetAsLastSibling();
        }
        else
        {
            checkmark.sprite = checkmarkUncheckedImg;
            darkOverlay.SetActive(false);
            nameText.fontStyle = FontStyles.Normal;
            transform.SetAsFirstSibling();
        }
    }

    public void SetPoints()
    {
        int playerPoints = ProfileManager.Instance.currentProfile.points;

        if (playerPoints - prizeData.points < 0)
            return;

        ClaimPrize();

        if (prizeData.claimed)
            ProfileManager.Instance.SetPoints(-prizeData.points);
        else
            ProfileManager.Instance.SetPoints(prizeData.points);

        CheckmarkValueChanged();
        menu.GetPoints();
    }
}
