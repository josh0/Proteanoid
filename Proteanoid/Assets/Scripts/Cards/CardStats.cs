using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStats
{
    public CardStats(int manaCost, int damage, int duration)
    {
        this.manaCost = manaCost;
        this.damage = damage;
        this.duration = duration;
    }

    public int manaCost;
    public int damage;
    public int duration;
}
