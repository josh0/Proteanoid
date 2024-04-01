using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    private List<MapEventButton> mapButtons;
    private void Awake()
    {
        mapButtons = FindObjectsOfType<MapEventButton>(false).ToList();
    }

    private void Start()
    {
        StartCoroutine(GenerateMap());
    }
    public IEnumerator GenerateMap()
    {
        yield return new WaitForSeconds(0.1f);
        foreach(MapEventButton button in mapButtons)
        {
            button.GenerateEvent();
            yield return new WaitForSeconds(0.2f);
        }

    }
}
