using Newtonsoft.Json;
using System;

[Serializable]
public class ChoreData
{
    [JsonIgnore] public Guid id = Guid.NewGuid();
    public string name;
    public int points;
    public string date;
    public bool completed = false;
    public int userId;
}
