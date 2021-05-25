using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinLocation : MonoBehaviourPunCallbacks, ICallerLoadingWaitWindow, IInstantiateNetworkObject, IRequestSender
    {

    private const string PATH_PREFABS_CALLBACKS_OBJECTS = "System/joinLocation_components";

    private const string PATH_CHARACTER_LIST_DATA = "Data/Character/CharacterList";

    private const string PATH_PREFAB_PANEL_NAME_LOCATION = "Prefabs/UI/locationPanel";

    private const string PATH_PREFAB_MAIN_CANVAS = "Prefabs/UI/MainCanvas";

    private const string NAME_OBJECT_CONTAINER_CALLBACK_OBJECTS = "callbackObjects";


    private const string PREFIX_CLONE_PREFAB = "(Clone)";

    private const string TAG_POINT_SPAWN = "PointSpawn";

    
    private LoadingWait loadingWait;

    private CharactersList characterList;

    private GameObject[] callbacksObjects;

    private PointSpawn[] pointSpawns;




    private GameObject mainCanvasPrefab;

    private string idRequest;

    [Header("Данные о локации")]
    [SerializeField]
    [ReadOnlyField]
    private LocationData locationData;
    private PanelNameLocation panelNameLocationPrefab;

    // Use this for initialization
    void Start()
        {
        if (LoadingWaitManager.Manager == null)
        {
            throw new JoinLocationException("loading wait manager not found");
        }

        pointSpawns = FindObjectsOfType<PointSpawn>();

        if (pointSpawns.Length == 0)
        {
            throw new JoinLocationException("points spawns not found");
        }


        callbacksObjects = Resources.LoadAll<GameObject>(PATH_PREFABS_CALLBACKS_OBJECTS);

        if (callbacksObjects == null || callbacksObjects.Length == 0)
        {
            throw new JoinLocationException("callbacksObjects not found");
        }

        characterList = Resources.Load<CharactersList>(PATH_CHARACTER_LIST_DATA);

        if (characterList == null)
        {
            throw new JoinLocationException("character list not found");
        }

        panelNameLocationPrefab = Resources.Load<PanelNameLocation>(PATH_PREFAB_PANEL_NAME_LOCATION);

        if (panelNameLocationPrefab == null)
        {
            throw new InitilizatorLocationException("panel location name prefab not found");
        }


        loadingWait = LoadingWaitManager.Manager.CreateLoadingWait();
        loadingWait.SetText("Заходим на локацию...");


        PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name, new RoomOptions(), TypedLobby.Default);
        }

    public void DestroyLoadingWaitWindow()
    {
        loadingWait.Remove();
    }


    public void SetLocationData(LocationData data)
    {
        if (data == null)
        {
            throw new JoinLocationException("data location is null");
        }

        locationData = data;
    }


    #region Server Callbacks
    public override void OnJoinedRoom()
    {
        mainCanvasPrefab = Resources.Load<GameObject>(PATH_PREFAB_MAIN_CANVAS);

        if (mainCanvasPrefab == null)
        {
            throw new InitilizatorLocationException("main canvas prefab not found");
        }


        DestroyLoadingWaitWindow();
        
        InstantiatePlayerObject(characterList.GetCharacter(0).gameObject, pointSpawns[Random.Range(0, pointSpawns.Length)].Position);

        GameObject containerCallbackObjects = new GameObject(NAME_OBJECT_CONTAINER_CALLBACK_OBJECTS);


        for (int i = 0; i < callbacksObjects.Length; i++)
        {
            Instantiate(callbacksObjects[i], containerCallbackObjects.transform);
        }



        GameObject mainCanvas = Instantiate(mainCanvasPrefab);

        mainCanvas.name = mainCanvas.name.Replace(PREFIX_CLONE_PREFAB, string.Empty);

        Instantiate(panelNameLocationPrefab, mainCanvas.transform).SetText(locationData.NameLocation);

     //   SendRequest();

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        DestroyLoadingWaitWindow();
       ManagerWindowsNotfications.Manager.CreateNotfication($"Не удалось подключиться к локации. Проверьте подключение к сети Интернет.\n Код ошибки: {returnCode}\nСообщение сервера: {message}", MessageNotficationType.Error, true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        DestroyLoadingWaitWindow();
        ManagerWindowsNotfications.Manager.CreateNotfication($"Соединение с сервером было прервано. Проверьте подключение к сети Интернет.\n Код ошибки: {cause}", MessageNotficationType.Error, true);
    }

    #endregion

    public GameObject InstantiatePlayerObject(GameObject gameObject, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
    {
        return PhotonNetwork.Instantiate(gameObject.name, position, rotation, group, data);
    }

    public GameObject InstantiateSceneObject(GameObject gameObject, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
    {
        return PhotonNetwork.InstantiateRoomObject(gameObject.name, position, rotation, group, data);
    }

    public GameObject InstantiatePlayerObject(GameObject gameObject, Vector3 position, byte group = 0, object[] data = null)
    {
        return PhotonNetwork.Instantiate(gameObject.name, position, Quaternion.identity, group, data);
    }

    public GameObject InstantiateSceneObject(GameObject gameObject, Vector3 position, byte group = 0, object[] data = null)
    {
        return PhotonNetwork.InstantiateRoomObject(gameObject.name, position, Quaternion.identity, group, data);
    }

    public void SendRequest()
    {
        loadingWait = LoadingWaitManager.Manager.CreateLoadingWait();

        idRequest = RequestManager.Manager.GenerateRequestID();

        RequestManager.Manager.onRequestFinish += ReceiveRequest;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("name", "max");
       data.Add("job", "unity developer");
        RequestManager.Manager.SendRequestToServer(idRequest, "api/users2", RequestType.PUT, data);


    }

    public void ReceiveRequest(string id, string text, RequestResult requestResult, long responseCode)
    {
        if (id == idRequest)
        {
            Debug.Log(text);
        }

        RequestManager.Manager.onRequestFinish -= ReceiveRequest;
    }
}