using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapEvent : ScriptableObject
{
    public Sprite eventIcon;
    public abstract IEnumerator EventRoutine();
}
