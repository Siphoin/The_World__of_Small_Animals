using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class AuthCharacter : AuthComponent, IRequestSender, IAuthComponent
    {
    private const string PREFIX_ID_REQUEST = "auth_character_";

    private string _idCharacter;

    [Header("Страница получения информации о персонаже")]
    [SerializeField] private string _urlGetInfoCharacter = "character/info";

    [Header("Страница получения информации о персонаже")]
    [SerializeField] private string _urlGetValuteCharacter = "character/valute";

    private AuthCharacterType _authType;

    private static AuthCharacter _manager;

    private CharacterRequestData _characterData;

    public static AuthCharacter Manager => _manager;
    public CharacterRequestData CharacterData => _characterData;


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
                _characterData = JsonConvert.DeserializeObject<CharacterRequestData>(text);
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

        string page = _authType == AuthCharacterType.Info ? _urlGetInfoCharacter : _urlGetValuteCharacter;

        Dictionary<string, object> form = new Dictionary<string, object>();

        switch (_authType)
        {
            case AuthCharacterType.Info:

                if (string.IsNullOrEmpty(_idCharacter))
                {
                    throw new AuthCharacterException("id character must be seted");
                }

                form.Add("id", _idCharacter);
                form.Add("token", TokenActive);


                break;

            default:
                throw new AuthCharacterException($"invalid auth type: {_authType}");
        }


        requestManager.SendRequestToServer(idRequest, page, RequestType.POST, form);

        SendEventAuth();
        SetAuthingStatus(true);

    }

    public override void Ini()
    {
        _urlGetInfoCharacter = TrimText(_urlGetInfoCharacter);
        _urlGetValuteCharacter = TrimText(_urlGetValuteCharacter);


        if (string.IsNullOrEmpty(_urlGetInfoCharacter))
        {
            throw new AuthCharacterException("url get info character is emtry");
        }

        if (string.IsNullOrEmpty(_urlGetValuteCharacter))
        {
            throw new AuthCharacterException("url get info valute character is emtry");
        }



        if (_manager == null)
        {
            _manager = this;

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

        if (string.IsNullOrEmpty(data.Token))
        {
            throw new AuthCharacterException("string token is null");
        }

        TokenActive = data.Token;

        _authType = AuthCharacterType.Info;

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

        _idCharacter = id;
    }

    private string TrimText (string value)
    {
      return  value.Trim();
    }


    void Start() => Ini();
    private void SetAuthingStatus (bool status) => isAuthing = status;




}
