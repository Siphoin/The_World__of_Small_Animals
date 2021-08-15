using Newtonsoft.Json;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AuthCharacter : AuthComponent, IRequestSender, IAuthComponent
    {
    private const string PREFIX_ID_REQUEST = "auth_character_";

    private string idCharacter;


    private static AuthCharacter manager;




    private AuthCharacterType authType;

    [Header("Страница получения информации о персонаже")]
    [SerializeField] private string urlGetInfoCharacter = "character/info";

    [Header("Страница получения информации о персонаже")]
    [SerializeField] private string urlGetValuteCharacter = "character/valute";

    private CharacterRequestData characterData;


    public static AuthCharacter Manager { get => manager; }
    public CharacterRequestData CharacterData { get => characterData; }

    // Use this for initialization
    void Start()
        {
        Ini();
        }

    public void ReceiveRequest(string id, string text, RequestResult requestResult, long responseCode)
    {
        if (id == idRequest)
        {

            SetAuthingStatus(false);
            if (responseCode != 200)
            {
                SendEventAuthFalled();
                return;
            }

            try
            {
                characterData = JsonConvert.DeserializeObject<CharacterRequestData>(text);
                SendEventAuthFinish();

            }
            catch 
            {
                SendEventAuthFalled();
                throw;
            }
        }
    }

    public void SendRequest()
    {
        idRequest = PREFIX_ID_REQUEST + requestManager.GenerateRequestID();
        string page = authType == AuthCharacterType.Info ? urlGetInfoCharacter : urlGetValuteCharacter;
        Dictionary<string, object> form = new Dictionary<string, object>();

        switch (authType)
        {
            case AuthCharacterType.Info:

                if (string.IsNullOrEmpty(idCharacter))
                {
                    throw new AuthCharacterException("id character must be seted");
                }

                form.Add("id", idCharacter);
                form.Add("token", TokenActive);


                break;
            case AuthCharacterType.Valute:


                break;
            default:
                throw new AuthCharacterException($"invalid auth type: {authType}");
        }


        requestManager.SendRequestToServer(idRequest, page, RequestType.POST, form);
        SendEventAuth();
        SetAuthingStatus(true);

    }

    public override void Ini()
    {
        urlGetInfoCharacter = TrimText(urlGetInfoCharacter);
        urlGetValuteCharacter = TrimText(urlGetValuteCharacter);


        if (string.IsNullOrEmpty(urlGetInfoCharacter))
        {
            throw new AuthCharacterException("url get info character is emtry");
        }

        if (string.IsNullOrEmpty(urlGetValuteCharacter))
        {
            throw new AuthCharacterException("url get info valute character is emtry");
        }



        if (manager == null)
        {
            manager = this;

            requestManager = RequestManager.Manager;
            requestManager.OnRequestFinish += ReceiveRequest;

            base.Ini();
        }

        else
        {
            Remove();
        }
    }

    public void Auth (TokenString data)
    {
        if (data == null)
        {
            throw new AuthCharacterException("data token is null");
        }

        if (string.IsNullOrEmpty(data.token))
        {
            throw new AuthCharacterException("string token is null");
        }

        TokenActive = data.token;

        authType = AuthCharacterType.Info;

        if (isAuthing)
        {
#if UNITY_EDITOR
            Debug.Log("new request auth canceled because old request not finished");
#endif
            return;
        }

        SendRequest();
    }

    public void SetIdCharacter (string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new AuthCharacterException("id character is null");
        }

        idCharacter = id;
    }

    private string TrimText (string value)
    {
      return  value.Trim();
    }

    private void SetAuthingStatus (bool status)
    {
        isAuthing = status;
    }


}
