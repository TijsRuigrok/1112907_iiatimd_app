using System;
using System.Collections.Generic;

[Serializable]
public class Profile
{
    public string email;
    public List<Chore> chores = new List<Chore>();
    public List<Prize> prizes = new List<Prize>();
}
