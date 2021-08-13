using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Points { get; set; }
    public bool Claimed { get; set; }
    public int UserId { get; set; }
}
