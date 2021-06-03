using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AuthUser : AuthComponent, IRequestSender, IAuthComponent
    {
    [Header("Страница получения информации о пользоваителе")]
    [SerializeField] private string urlGetInfoUser = "users/auth/info";

    private const string PREFIX_ID_REQUEST = "auth_user_";

    private static AuthUser manager;

    private UserData userData;


    public static AuthUser Manager { get => manager; }
    public UserData UserData { get => userData; }

    // Use this for initialization
    void Start()
        {
        Ini();
        }


    public void Auth(TokenString data)
    {
      if (data == null)
        {
            throw new AuthUserException("token data is null");
        }

      if (string.IsNullOrEmpty(data.token))
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

        TokenActive = data.token;
        SendRequest();
        
    }

    public void SendRequest()
    {
        Dictionary<string, object> form = new Dictionary<string, object>();

        form.Add("token", TokenActive);


        idRequest = PREFIX_ID_REQUEST + requestManager.GenerateRequestID();

        requestManager.SendRequestToServer(idRequest, urlGetInfoUser, RequestType.POST, form);
        SendEventAuth();
    }

    public void ReceiveRequest(string id, string text, RequestResult requestResult, long responseCode)
    {
     if (id == idRequest && requestResult == RequestResult.OK)
        {
            try
            {
                userData = JsonConvert.DeserializeObject<UserData>(text);

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
        if (string.IsNullOrEmpty(urlGetInfoUser))
        {
            throw new AuthUserException("url get user info is emtry");
        }

        if (!manager)
        {
            manager = this;
            requestManager = RequestManager.Manager;
            requestManager.onRequestFinish += ReceiveRequest;
            base.Ini();
        }

        else
        {
            Remove();
        }
    }
}