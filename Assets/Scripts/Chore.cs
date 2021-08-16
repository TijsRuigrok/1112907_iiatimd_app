using System;

[Serializable]
public class Chore
{
    public int id;
    public string name;
    public int points;
    public string date;
    public bool completed = false;
    public int userId;
}
