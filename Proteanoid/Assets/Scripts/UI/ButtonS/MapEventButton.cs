using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MapEventButton : MonoBehaviour
{
    [SerializeField] private List<MapEvent> possibleEvents;
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
        mapEvent = ScriptableObject.Instantiate(possibleEvents[Random.Range(0,possibleEvents.Count)]);
        icon.sprite = mapEvent.eventIcon;
    }

    private void StartEvent()
    {
        StartCoroutine(EventRoutine());
    }

    private IEnumerator EventRoutine()
    {

        yield return mapEvent.EventRoutine();
    }

    void Start()
    {
        
    }
}
