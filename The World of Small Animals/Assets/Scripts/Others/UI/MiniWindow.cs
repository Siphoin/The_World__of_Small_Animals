using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
    public class MiniWindow : ActivatorGameObject, IRemoveObject
    {
    private const string TAG_MY_PLAYER = "MyPlayer";


    private const float TIME_OUT_ENABLE_MOVE_LOCAL_PLAYER = 0.02f;


    [Header("Кнопка закрытия/скрытия окнв")]
    [SerializeField] private Button buttonExit;

    [Header("Вариант закрытия окна")]
    [SerializeField] private MiniWindowActionExitType typeExit;

    private CharacterController myPlayer;

        // Use this for initialization
        void Start()
        {

        }

       protected void Ini ()
    {
        if (buttonExit == null)
        {
            throw new MiniWindowException("button exit not seted");
        }

        myPlayer = GameObject.FindGameObjectWithTag(TAG_MY_PLAYER).GetComponent<CharacterController>();


        buttonExit.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        myPlayer.Enable(TIME_OUT_ENABLE_MOVE_LOCAL_PLAYER);


        switch (typeExit)
        {
            case MiniWindowActionExitType.Destroy:


                Remove();


                break;
            case MiniWindowActionExitType.Hide:


                SetActiveSelfGameObject(gameObject, false);

                break;
            default:
                throw new MiniWindowException($"invalid type exit: {typeExit}");
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void Remove(float time)
    {
        Destroy(gameObject, time);
    }
}