using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[RequireComponent(typeof(EventTrigger))]
public class ElementBlockerMoveCharacter : MonoBehaviour, IFinderLocalPlayer
{
    private const string TAG_MY_PLAYER = "MyPlayer";


    private CharacterController myPlayer;

    private EventTrigger eventTrigger;

    [Header("Тип элемента")]
    [SerializeField] private ElementBlockerUIType elementType = ElementBlockerUIType.Button;

    private void Start()
    {

        if (!TryGetComponent(out eventTrigger))
        {
            throw new ElementBlockerMoveCharacterException("event trigger component not found");
        }


        switch (elementType)
        {
            case ElementBlockerUIType.Object:
                AddEventsObject();
                break;
            case ElementBlockerUIType.Button:
        AddEventsButton();
                break;
            default:
                throw new ElementBlockerMoveCharacterException($"element blocker type not valid: Value {elementType}");
        }

    }

    private void AddEventsButton()
    {
        var select = CreateNewEventTrigger(Select, EventTriggerType.Select);

        var deselect = CreateNewEventTrigger(Deselect, EventTriggerType.Deselect);

        AddEventOnTrigger(select);
        AddEventOnTrigger(deselect);
    }

    private void AddEventsObject()
    {
        var poinerEnter = CreateNewEventTrigger(Select, EventTriggerType.PointerEnter);

        var pointerExit = CreateNewEventTrigger(Deselect, EventTriggerType.PointerExit);

        AddEventOnTrigger(poinerEnter);
        AddEventOnTrigger(pointerExit);
    }



    private void Deselect(BaseEventData arg0)
    {
        if (myPlayer != null)
        {
            myPlayer.Enable();
        }
    }

    private void Select(BaseEventData arg0)
    {
        if (myPlayer != null)
        {
            myPlayer.Disable();
        }
    }

    private void AddEventOnTrigger (EventTrigger.Entry eventTarget)
    {
        eventTrigger.triggers.Add(eventTarget);
    }

    private EventTrigger.Entry CreateNewEventTrigger (UnityAction<BaseEventData> action, EventTriggerType type)
    {
        var ev = new EventTrigger.Entry();
        ev.eventID = type;
        ev.callback.AddListener(action);
        return ev;
    }


    private void Awake()
    {
        try
        {

            myPlayer = FindLocalPlayerWithTag(TAG_MY_PLAYER);


        }
        catch
        {

        }

    }

    private void OnDestroy()
    {
        try
        {
            Window window = null;


            if (TryGetComponent(out window))
            {
                myPlayer.Enable();
            }
        }
        catch
        {

        }
    }

    public CharacterController FindLocalPlayerWithTag(string tag)
    {
     return   GameObject.FindGameObjectWithTag(tag).GetComponent<CharacterController>();
    }
}