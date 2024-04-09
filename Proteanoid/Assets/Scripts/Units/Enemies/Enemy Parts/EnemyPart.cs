using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    public enum PartTypes
    {
        claw,
        fang,
        carapace
    }
    public List<Card> possibleCardRewards = new();
    public int value;
}
