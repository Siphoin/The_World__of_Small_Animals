using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
    public class ButtonBlockerMoveCharacter : MonoBehaviour, IPointerUpHandler, IPointerExitHandler
{

    private  Button _button;
    private CharacterController _myPlayer;

    private bool _clicked = false;


       private void Start()
        {
        Destroy(this);
        if (!TryGetComponent(out _button))
        {
            throw new ButtonBlockerMoveCharacterException($"{name} not have component UnityEngine.UI.Button");
        }
        
        _myPlayer = GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<CharacterController>();

        _button.onClick.AddListener(BlockPlayer);
    }

    private void BlockPlayer()
    {
        _clicked = true;
        _myPlayer.Disable();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (_clicked)
        {
            _clicked = false;
            myPlayer.Enable();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_clicked)
        {
            _clicked = false;
            _myPlayer.Enable();
        }
    }
}
