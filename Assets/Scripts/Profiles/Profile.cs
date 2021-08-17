using System;
using System.Collections.Generic;

[Serializable]
public class Profile
{
    public string email;
    public List<ChoreData> choresData = new List<ChoreData>();
    public List<PrizeData> prizesData = new List<PrizeData>();
}
