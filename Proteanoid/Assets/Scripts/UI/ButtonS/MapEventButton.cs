using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MapEventButton : MonoBehaviour
{
    [field: SerializeField] public List<MapEventButton> connectedNodes { get; private set; } = new();
    [SerializeField] private List<MapEvent> possibleEvents = new();
    public MapEvent mapEvent { get; private set; }
    private Image icon;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        icon = GetComponent<Image>();
        button.onClick.AddListener(StartEvent);
    }

    public void GenerateEvent()
    {
        if (possibleEvents.Count == 0)
            return;
        mapEvent = Instantiate(possibleEvents[Random.Range(0, possibleEvents.Count)]);
        icon.sprite = mapEvent.eventIcon;
    }

    private void StartEvent()
    {
        if (mapEvent != null)
            StartCoroutine(EventRoutine());
        MapManager.Instance.currentNode = this;
        SceneManager.LoadScene(mapEvent.scene.name);
    }

    private IEnumerator EventRoutine()
    {
        MapManager.Instance.SetButtonsInteractable(false);
        yield return mapEvent.EventRoutine();
        MapManager.Instance.SetButtonsInteractable(true);
    }

    public void SetButtonInteractable(bool i)
    {
        if (mapEvent != null)
            button.interactable = i;
        else
            button.interactable = false;
    }
}
