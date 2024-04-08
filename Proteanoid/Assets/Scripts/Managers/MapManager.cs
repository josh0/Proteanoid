using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    private List<MapEventButton> mapButtons;
    public MapEventButton currentNode;
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
            yield return new WaitForSeconds(0.1f);
        }

        SetButtonsInteractable(true);
    }

    /// <summary>
    /// If false, deactivate all nodes. <br />
    /// If true, activate the nodes adjacent to the player.
    /// </summary>
    /// <param name="a"></param>
    public void SetButtonsInteractable(bool a)
    {
        //deactivate all nodes
        foreach (MapEventButton b in mapButtons)
            b.SetButtonInteractable(false);

        //re-activate the nodes that should be active.
        if (a)
        {
            foreach (MapEventButton b in currentNode.connectedNodes)
                b.SetButtonInteractable(true);
        }
    }
}
