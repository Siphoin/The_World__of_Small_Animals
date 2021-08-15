using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class AuthUser : AuthComponent, IRequestSender, IAuthComponent
    {

    private const string PREFIX_ID_REQUEST = "auth_user_";

    [Header("Страница получения информации о пользоваителе")]
    [SerializeField] private string _urlGetInfoUser = "users/auth/info";

    private static AuthUser _manager;

    private UserData _userData;


    public static AuthUser Manager => _manager;
    public UserData UserData  => _userData;



    public void Auth(TokenString data)
    {
      if (string.IsNullOrEmpty(data.Token))
        {
            throw new AuthUserException("token string is null");
        }

        if (isAuthing)
        {
#if UNITY_EDITOR
            Debug.Log("new request auth canceled because old request not finished");
#endif
            return;
        }

        TokenActive = data.Token;
        SendRequest();
        
    }

    public void SendRequest()
    {
        Dictionary<string, object> form = new Dictionary<string, object>();

        form.Add("token", TokenActive);


        idRequest = PREFIX_ID_REQUEST + requestManager.GenerateRequestID();

        requestManager.SendRequestToServer(idRequest, _urlGetInfoUser, RequestType.POST, form);
        SendEventAuth();
    }

    public void ReceiveRequest(string id, string text, RequestResult requestResult, long responseCode)
    {
     if (id == idRequest && requestResult == RequestResult.OK)
        {
            try
            {
                _userData = JsonConvert.DeserializeObject<UserData>(text);

#if UNITY_EDITOR
                Debug.Log("user auth is success");
#endif
                SendEventAuthFinish();
            }
            catch
            {
                SendEventAuthFalled();
                throw;
            }
        }
    }

    public override void Ini()
    {
        if (string.IsNullOrEmpty(_urlGetInfoUser))
        {
            throw new AuthUserException("url get user info is emtry");
        }

        if (!_manager)
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

    void Start() => Ini();
}