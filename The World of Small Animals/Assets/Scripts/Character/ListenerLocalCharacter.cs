using Photon.Pun;
using System;
using UnityEngine;

public class ListenerLocalCharacter : MonoBehaviour, IFinderLocalPlayer, IInvokerMono
    {
       private const string TAG_MY_PLAYER = "MyPlayer";

       private const string PATH_SETTINGS_CHARACTER = "Data/Character/CharacterSettings";

       private bool _invokeStarted = false;

       private CharacterController _myPlayer;

       private CharacterSettings _characterSettings;

 private void Start()
        {

        _characterSettings = Resources.Load<CharacterSettings>(PATH_SETTINGS_CHARACTER);

        if (_characterSettings == null)
        {
            throw new ListenerLocalCharacterException("character settings not found");
        }

        _myPlayer = FindLocalPlayerWithTag(TAG_MY_PLAYER);

        _myPlayer.OnMoveStatusChanged += StartInvokingAFK;

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
            if (!_invokeStarted)
            {
                _invokeStarted = true;

                CallInvokingMethod(DisconnectOnServer, _characterSettings.AfkTimeOut);
            }
        }
    }

    private void DisconnectOnServer()
    {
        if (_myPlayer.Moved)
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

        _invokeStarted = false;
    }


    public CharacterController FindLocalPlayerWithTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag).GetComponent<CharacterController>();
    }

    public void CallInvokingEveryMethod(Action method, float time) => InvokeRepeating(method.Method.Name, time, time);

    public void CallInvokingMethod(Action method, float time) => Invoke(method.Method.Name, time);
}
