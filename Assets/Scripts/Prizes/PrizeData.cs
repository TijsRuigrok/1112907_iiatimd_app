using System;

[Serializable]
public class PrizeData
{
    public Guid id = Guid.NewGuid();
    public string name;
    public int points;
    public bool claimed = false;
    public int userId;
}
