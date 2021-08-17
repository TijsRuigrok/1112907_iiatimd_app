using UnityEngine;

public class ChoreParent : Chore
{
    [HideInInspector] public ChoresMenuParent choresMenu;

    public void Remove()
    {
        choresMenu.RemoveChore(choreData.id);
    }
}
