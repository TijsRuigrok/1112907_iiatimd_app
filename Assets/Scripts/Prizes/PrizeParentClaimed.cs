using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeParentClaimed : Prize
{
    [HideInInspector] public CompletedMenu completedMenu;

    public void Remove()
    {
        completedMenu.RemovePrize(prizeData.id);
    }
}
