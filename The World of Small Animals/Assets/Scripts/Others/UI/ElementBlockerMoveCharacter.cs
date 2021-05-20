using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementBlockerMoveCharacter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const string TAG_MY_PLAYER = "MyPlayer";


    private CharacterController myPlayer;
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        myPlayer.Disable();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myPlayer.Enable();
    }

    private void Awake()
    {
        myPlayer = GameObject.FindGameObjectWithTag(TAG_MY_PLAYER).GetComponent<CharacterController>();
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
}