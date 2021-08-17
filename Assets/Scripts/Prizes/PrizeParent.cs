using UnityEngine;

public class PrizeParent : Prize
{
    [HideInInspector] public PrizesMenuParent prizeMenu;

    public void Remove()
    {
        prizeMenu.RemovePrize(prizeData.id);
    }
}
