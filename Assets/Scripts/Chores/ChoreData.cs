using System;

[Serializable]
public class ChoreData
{
    public Guid id = Guid.NewGuid();
    public string name;
    public int points;
    public string date;
    public bool completed = false;
    public int userId;
}
