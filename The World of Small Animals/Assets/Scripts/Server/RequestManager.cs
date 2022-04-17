using System;
using System.Collections;
using UnityEngine;
using shortid;
using System.Collections.Generic;
using UnityEngine.Networking;

public class RequestManager : MonoBehaviour, IRemoveObject
    {
    private const string PATH_STRING_SERVER_ADDRESS = "Data/ServerAddress/ServerAddress";

    private HashSet<string> _requestList = new HashSet<string>();

    private static RequestManager _manager;

    private ServerAddress _serverAddress;

    public event Action<string, string, RequestResult, long> OnRequestFinish;

    public event Action<string, Texture2D, RequestResult, long> OnRequestGetTextureFinish;

    public static RequestManager Manager => _manager;

    // Use this for initialization
    void Awake()
        {
        if (_manager == null)
        {
            _serverAddress = Resources.Load<ServerAddress>(PATH_STRING_SERVER_ADDRESS);

            if (_serverAddress == null)
            {
                throw new RequestManagerException("server addrees Object not found");
            }

            if (string.IsNullOrEmpty(_serverAddress.Address))
            {
                throw new RequestManagerException("server addrees string is emtry");
            }

            _manager = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Remove();
        }
        }

    public void Remove()
    {
        try
        {
            Destroy(gameObject);
        }
        catch
        {

        }
    }

    public void Remove(float time)
    {
        try
        {
            Destroy(gameObject, time);
        }
        catch
        {

        }

    }

    public string GenerateRequestID ()
    {
        return ShortId.Generate(true, false, UnityEngine.Random.Range(12, 31));
    }

    private void AddRequest (string key)
    {
        if (_requestList.Contains(key))
        {
            throw new RequestManagerException($"request id {key} contains on Request List");
        }

        else
        {
            _requestList.Add(key);

#if UNITY_EDITOR
            Debug.Log($"request (id {key}) added on request list");
#endif
        }
    }

    private void RemoveRequest (string key)
    {
        if (!_requestList.Contains(key))
        {
            throw new RequestManagerException($"request id {key} not contains on Request List");
        }

        else
        {
            _requestList.Remove(key);
#if UNITY_EDITOR
            Debug.Log($"request (id {key}) removed on request list");
#endif
        }
    }

    private void ShowErrorNotficationServer (UnityWebRequest requestTarget) => ManagerWindowsNotfications.Manager.CreateNotfication($"Произошла ошибка сервера\nОшибка: {requestTarget.downloadHandler.error}\nКод ошибки: {requestTarget.result}", MessageNotficationType.Error, true);

    private void ShowErrorNotficationServer(WWW requestTarget) => ManagerWindowsNotfications.Manager.CreateNotfication($"Произошла ошибка сервера\nОшибка: {requestTarget.error}", MessageNotficationType.Error, true);

    private void ShowLogError (UnityWebRequest requestTarget)
    {
        Debug.LogError($"Server request error: (URL: {requestTarget.url}). Error: {requestTarget.downloadHandler.error} Result: {requestTarget.result}");
    }

    private void ShowLogError(WWW requestTarget) =>  Debug.LogError($"Server request error: (URL: {requestTarget.url}). Error: {requestTarget.error}");

    private void SendObserversEventRequest (string id, string dataText, RequestResult result, long requestCode) => OnRequestFinish?.Invoke(id, dataText, result, requestCode);

    private void SendObserversEventRequestTexture(string id, Texture2D texture, RequestResult result, long requestCode) => OnRequestGetTextureFinish?.Invoke(id, texture, result, requestCode);



    #region Unity Web Request Types
    private IEnumerator SendRequest (string idRequest, string path, RequestType requestType = RequestType.GET, Dictionary<string, object> form = null, bool catchError = true)
    {
        string url = _serverAddress.Address + path;


        #region Initializing a web form


        WWWForm webForm = null;

        if (form != null)
        {

            webForm = new WWWForm();


            foreach (var item in form)
            {
                object obj = item.Value;

                string key = item.Key;

                if (obj.IsNumber())
                {
                    webForm.AddField(key, (int)obj);
                }

                if (obj.IsString())
                {
                    webForm.AddField(key, (string)obj);
                }

                if (!obj.IsString() && !obj.IsNumber())
                {
                    throw new RequestManagerException($"object not valid. Key {key}");
                }

            }
        }

        #endregion




        switch (requestType)
        {
            case RequestType.GET:

                #region Get Request

                using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
                {

                    AddRequest(idRequest);
                    yield return webRequest.SendWebRequest();

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.Success:


                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.text, RequestResult.OK, webRequest.responseCode);


                            break;
                        case UnityWebRequest.Result.ConnectionError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
                            #if UNITY_EDITOR


                                ShowLogError(webRequest);


                              #endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);
                            break;
                        case UnityWebRequest.Result.ProtocolError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);

                            break;
                        case UnityWebRequest.Result.DataProcessingError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);

                            break;
                    }

                    RemoveRequest(idRequest);

                }

                break;

            #endregion


            case RequestType.POST:

                #region POST Request

                if (webForm == null)
                {
                    throw new RequestManagerException("form not seted");
                }

                using (UnityWebRequest webRequest = UnityWebRequest.Post(url, webForm))
                {

                    AddRequest(idRequest);

                    yield return webRequest.SendWebRequest();

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.Success:


                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.text, RequestResult.OK, webRequest.responseCode);


                            break;
                        case UnityWebRequest.Result.ConnectionError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);
                            break;
                        case UnityWebRequest.Result.ProtocolError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);

                            break;
                        case UnityWebRequest.Result.DataProcessingError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);

                            break;


                    }
                    RemoveRequest(idRequest);

                }

                #endregion


                break;
            case RequestType.Delete:

                #region Delete Request

                using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
                {

                    AddRequest(idRequest);
                    yield return webRequest.SendWebRequest();

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.Success:


                            SendObserversEventRequest(idRequest, "", RequestResult.OK, webRequest.responseCode);


                            break;
                        case UnityWebRequest.Result.ConnectionError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);
                            break;
                        case UnityWebRequest.Result.ProtocolError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);

                            break;
                        case UnityWebRequest.Result.DataProcessingError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);

                            break;
                    }

                    RemoveRequest(idRequest);

                }
                #endregion

                break;
            case RequestType.PUT:

                #region Put Request


                if (webForm == null)
                {
                    throw new RequestManagerException("form not seted");
                }

                using (UnityWebRequest webRequest = UnityWebRequest.Put(url, webForm.data))
                {

                    AddRequest(idRequest);


                    yield return webRequest.SendWebRequest();

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.Success:


                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.text, RequestResult.OK, webRequest.responseCode);


                            break;
                        case UnityWebRequest.Result.ConnectionError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);
                            break;
                        case UnityWebRequest.Result.ProtocolError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);

                            break;
                        case UnityWebRequest.Result.DataProcessingError:

                            if (catchError)
                            {
                                ShowErrorNotficationServer(webRequest);
                            }

                            else
                            {
#if UNITY_EDITOR


                                ShowLogError(webRequest);


#endif
                            }

                            SendObserversEventRequest(idRequest, webRequest.downloadHandler.error, RequestResult.Error, webRequest.responseCode);

                            break;
                    }

                    RemoveRequest(idRequest);

                }
                #endregion


                break;

        }
    }

    private IEnumerator SendRequestGetImage (string idRequest, string path, bool catchError = true)
    {
        string url = _serverAddress.Address + path;

        WWW www = new WWW(url);
        yield return www;

        bool isOk = string.IsNullOrEmpty(www.error);

       
        if (isOk)
        {
            www.LoadImageIntoTexture(www.texture);
            SendObserversEventRequestTexture(idRequest, www.texture, RequestResult.OK, 200);
        }

        else
        {
            if (catchError)
            {
                ShowErrorNotficationServer(www);
            }

            else
            {
#if UNITY_EDITOR


                ShowLogError(www);

#endif
            }

            SendObserversEventRequestTexture(idRequest, null, RequestResult.Error, 500);
        }

        www.Dispose();
        www = null;
    }

    public void SendRequestToServer (string idRequest, string page, RequestType requestType = RequestType.GET, Dictionary<string, object> form = null, bool catchError = true)
    {
        StartCoroutine(SendRequest(idRequest, page, requestType, form, catchError));
    }

    public void SendRequestGetTextureToServer(string idRequest, string page, bool catchError = true) => StartCoroutine(SendRequestGetImage(idRequest, page, catchError));

    #endregion


}
