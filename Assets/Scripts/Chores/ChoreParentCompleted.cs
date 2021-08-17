using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoreParentCompleted : Chore
{
    [HideInInspector] public CompletedMenu completedMenu;

    public void Remove()
    {
        completedMenu.RemoveChore(choreData.id);
    }
}
