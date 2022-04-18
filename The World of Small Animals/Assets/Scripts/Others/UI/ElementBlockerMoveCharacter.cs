using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[RequireComponent(typeof(EventTrigger))]
public class ElementBlockerMoveCharacter : MonoBehaviour, IFinderLocalPlayer
{
    private const string TAG_MY_PLAYER = "MyPlayer";


    private CharacterController _myPlayer;

    private EventTrigger _eventTrigger;

    [Header("Тип элемента")]
    [SerializeField] private ElementBlockerUIType _elementType = ElementBlockerUIType.Button;

    private void Start()
    {

        if (!TryGetComponent(out _eventTrigger))
        {
            throw new ElementBlockerMoveCharacterException("event trigger component not found");
        }


        switch (_elementType)
        {
            case ElementBlockerUIType.Object:
                AddEventsObject();
                break;
            case ElementBlockerUIType.Button:
        AddEventsButton();
                break;
            default:
                throw new ElementBlockerMoveCharacterException($"element blocker type not valid: Value {_elementType}");
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
        if (_myPlayer != null)
        {
            _myPlayer.Enable();
        }
    }

    private void Select(BaseEventData arg0)
    {
        if (_myPlayer != null)
        {
            _myPlayer.Disable();
        }
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

            _myPlayer = FindLocalPlayerWithTag(TAG_MY_PLAYER);


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
    
    private void AddEventOnTrigger (EventTrigger.Entry eventTarget) => eventTrigger.triggers.Add(eventTarget);

    public CharacterController FindLocalPlayerWithTag(string tag)
    {
     return   GameObject.FindGameObjectWithTag(tag).GetComponent<CharacterController>();
    }
}
