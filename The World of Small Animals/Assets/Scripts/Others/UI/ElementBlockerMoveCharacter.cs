using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementBlockerMoveCharacter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const string TAG_MY_PLAYER = "MyPlayer";


    private CharacterController myPlayer;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myPlayer != null)
        {
            myPlayer.Disable();
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (myPlayer != null)
        {
            myPlayer.Enable();
        }

    }

    private void Awake()
    {
        try
        {

            myPlayer = GameObject.FindGameObjectWithTag(TAG_MY_PLAYER).GetComponent<CharacterController>();


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
}