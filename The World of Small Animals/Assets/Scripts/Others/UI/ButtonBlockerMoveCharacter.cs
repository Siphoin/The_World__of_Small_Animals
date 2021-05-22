using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
    public class ButtonBlockerMoveCharacter : MonoBehaviour, IPointerUpHandler, IPointerExitHandler
{

 private   Button button;
    private CharacterController myPlayer;

    private bool clicked = false;


        // Use this for initialization
        void Start()
        {
        Destroy(this);
        if (!TryGetComponent(out button))
        {
            throw new ButtonBlockerMoveCharacterException($"{name} not have component UnityEngine.UI.Button");
        }
        myPlayer = GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<CharacterController>();

        button.onClick.AddListener(BlockPlayer);
    }

    private void BlockPlayer()
    {
        clicked = true;
        myPlayer.Disable();
    }

    // Update is called once per frame
    void Update()
        {

        }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (clicked)
        {
            clicked = false;
            myPlayer.Enable();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clicked)
        {
            clicked = false;
            myPlayer.Enable();
        }
    }
}