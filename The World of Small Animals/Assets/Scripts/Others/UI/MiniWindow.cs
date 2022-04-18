using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
    public class MiniWindow : ActivatorGameObject, IRemoveObject, IFinderLocalPlayer
    {
    private const string TAG_MY_PLAYER = "MyPlayer";


    private const float TIME_OUT_ENABLE_MOVE_LOCAL_PLAYER = 0.02f;


    [Header("Кнопка закрытия/скрытия окнв")]
    [SerializeField] private Button _buttonExit;

    [Header("Вариант закрытия окна")]
    [SerializeField] private MiniWindowActionExitType _typeExit;

    private CharacterController _myPlayer;

    protected void Ini ()
    {
        if (_buttonExit == null)
        {
            throw new MiniWindowException("button exit not seted");
        }

        _myPlayer = FindLocalPlayerWithTag(TAG_MY_PLAYER);


        _buttonExit.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        _myPlayer.Enable(TIME_OUT_ENABLE_MOVE_LOCAL_PLAYER);


        switch (_typeExit)
        {
            case MiniWindowActionExitType.Destroy:


                Remove();


                break;
            case MiniWindowActionExitType.Hide:


                SetActiveSelfGameObject(gameObject, false);

                break;
            default:
                throw new MiniWindowException($"invalid type exit: {_typeExit}");
        }
    }


    public CharacterController FindLocalPlayerWithTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag).GetComponent<CharacterController>();
    }
    
    public void Remove() => Destroy(gameObject);

    public void Remove(float time) =>  Destroy(gameObject, time);
}
