using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MapEvent : ScriptableObject
{
    public Sprite eventIcon;
    public abstract IEnumerator EventRoutine();
    public SceneAsset scene;
}
