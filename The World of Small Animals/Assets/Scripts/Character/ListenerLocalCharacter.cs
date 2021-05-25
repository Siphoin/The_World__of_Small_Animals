using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

    public class ListenerLocalCharacter : MonoBehaviour, IFinderLocalPlayer, IInvokerMono
    {
        private const string TAG_MY_PLAYER = "MyPlayer";


    private const string PATH_SETTINGS_CHARACTER = "Data/Character/CharacterSettings";

    private CharacterController myPlayer;

    private CharacterSettings characterSettings;

    private bool invokeStarted = false;


    // Use this for initialization
    void Start()
        {

        characterSettings = Resources.Load<CharacterSettings>(PATH_SETTINGS_CHARACTER);

        if (characterSettings == null)
        {
            throw new ListenerLocalCharacterException("character settings not found");
        }
        myPlayer = FindLocalPlayerWithTag(TAG_MY_PLAYER);

        myPlayer.onMoveStatusChanged += StartInvokingAFK;

        StartInvokingAFK(false);
        }

    private void StartInvokingAFK(bool status)
    {
        if (status)
        {
            StopInvoke();
        }

        else
        {
            if (!invokeStarted)
            {
                invokeStarted = true;
                CallInvokingMethod(DisconnectOnServer, characterSettings.AfkTimeOut);
            }
        }
    }

    private void DisconnectOnServer()
    {
        if (myPlayer.Moved)
        {
            return;
        }

        else
        {
            ManagerWindowsNotfications.Manager.CreateNotfication("Твой персонаж слишком долго стоял на месте, нам пришлось отключить тебя от сервера. Пожалуйста, старайся не стоять на месте долго.", MessageNotficationType.Message, true);
            PhotonNetwork.Disconnect();
        }
    }

    private void StopInvoke()
    {
        CancelInvoke();
        invokeStarted = false;
    }

    // Update is called once per frame

    public CharacterController FindLocalPlayerWithTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag).GetComponent<CharacterController>();
    }

    public void CallInvokingEveryMethod(Action method, float time)
    {
        InvokeRepeating(method.Method.Name, time, time);
    }

    public void CallInvokingMethod(Action method, float time)
    {
        Invoke(method.Method.Name, time);
    }
}