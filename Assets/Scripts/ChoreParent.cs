using TMPro;
using UnityEngine;

public class ChoreParent : MonoBehaviour
{
    public Chore chore;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text dateText;

    void Start()
    {
        nameText.text = chore.Name;
        pointsText.text = chore.Points.ToString();
        dateText.text = chore.Date;
    }

    public async void DeleteChore()
    {
        await APIManager.Instance.client.DeleteAsync(
            APIManager.BaseURL + "chores/self/delete/" + chore.Id);
    }
}
