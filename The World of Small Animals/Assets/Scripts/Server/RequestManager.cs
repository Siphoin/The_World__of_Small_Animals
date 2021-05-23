using System;
using System.Collections;
using UnityEngine;
using shortid;
using System.Collections.Generic;
using UnityEngine.Networking;

public class RequestManager : MonoBehaviour, IRemoveObject
    {
    private const string PATH_STRING_SERVER_ADDRESS = "Data/ServerAddress/ServerAddress";

    private HashSet<string> requestList = new HashSet<string>();

    private static RequestManager manager;

    private ServerAddress serverAddress;

    public event Action<string, string, RequestResult, long> onRequestFinish;

    public static RequestManager Manager { get => manager; }

    // Use this for initialization
    void Awake()
        {
        if (manager == null)
        {
            serverAddress = Resources.Load<ServerAddress>(PATH_STRING_SERVER_ADDRESS);

            if (serverAddress == null)
            {
                throw new RequestManagerException("server addrees Object not found");
            }

            if (string.IsNullOrEmpty(serverAddress.Address))
            {
                throw new RequestManagerException("server addrees string is emtry");
            }

            manager = this;
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
        return ShortId.Generate(true, true, UnityEngine.Random.Range(5, 31));
    }

    private void AddRequest (string key)
    {
        if (requestList.Contains(key))
        {
            throw new RequestManagerException($"request id {key} contains on Request List");
        }

        else
        {
            requestList.Add(key);

#if UNITY_EDITOR
            Debug.Log($"request (id {key}) added on request list");
#endif
        }
    }

    private void RemoveRequest (string key)
    {
        if (!requestList.Contains(key))
        {
            throw new RequestManagerException($"request id {key} not contains on Request List");
        }

        else
        {
            requestList.Remove(key);
#if UNITY_EDITOR
            Debug.Log($"request (id {key}) removed on request list");
#endif
        }
    }

    private void ShowErrorNotficationServer (UnityWebRequest requestTarget)
    {
        ManagerWindowsNotfications.Manager.CreateNotfication($"Произошла ошибка сервера\nОшибка: {requestTarget.downloadHandler.error}\nКод ошибки: {requestTarget.result}", MessageNotficationType.Error, true);
    }

    private void ShowLogError (UnityWebRequest requestTarget)
    {
        Debug.LogError($"Server request error: (URL: {requestTarget.url}). Error: {requestTarget.downloadHandler.error} Result: {requestTarget.result}");
    }

    private void SendObserversEventRequest (string id, string dataText, RequestResult result, long requestCode)
    {
        onRequestFinish?.Invoke(id, dataText, result, requestCode);
    }



    #region Unity Web Request Types
    private IEnumerator SendRequest (string idRequest, string path, RequestType requestType = RequestType.GET, Dictionary<string, object> form = null, bool catchError = true)
    {
        string url = serverAddress.Address + path;


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

    public void SendRequestToServer (string idRequest, string page, RequestType requestType = RequestType.GET, Dictionary<string, object> form = null, bool catchError = true)
    {
        StartCoroutine(SendRequest(idRequest, page, requestType, form, catchError));
    }

    #endregion


}