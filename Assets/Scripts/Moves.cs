using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves
{
    public MoveBaseAttacks Base { get; set; }
    public int PP { get; set; }
    public Moves(MoveBaseAttacks pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }
}