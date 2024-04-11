using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Part")]
public class EnemyPart : ScriptableObject
{
    public enum PartTypes
    {
        claw,
        fang,
        carapace
    }
    public string partName;
    public List<Card> possibleCardRewards = new();
    public Sprite icon;
    public int value;
}
