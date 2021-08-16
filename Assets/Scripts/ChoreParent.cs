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
        nameText.text = chore.name;
        pointsText.text = chore.points.ToString();
        dateText.text = chore.date;
    }

    public async void DeleteChore()
    {
        await APIManager.Instance.client.DeleteAsync(
            APIManager.BaseURL + "chores/self/delete/" + chore.id);
    }
}
