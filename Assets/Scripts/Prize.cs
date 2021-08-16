using System;

[Serializable]
public class Prize
{
    public int id;
    public string name;
    public int points;
    public bool claimed = false;
    public int userId;
}
