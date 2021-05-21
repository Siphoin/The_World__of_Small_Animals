using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinLocation : MonoBehaviourPunCallbacks, ICallerLoadingWaitWindow, IInstantiateNetworkObject
    {

    private const string PATH_PREFAB_LISTENER_CALLBACKS_SERVER = "System/ListenerCallbacksServer";

    private const string PATH_CHARACTER_LIST_DATA = "Data/Character/CharacterList";

    private const string PATH_PREFAB_PANEL_NAME_LOCATION = "Prefabs/UI/locationPanel";


    private LoadingWait loadingWait;

    private CharactersList characterList;


    private ListenerCallbacksServer listenerCallbacksServerPrefab;


    private GameObject mainCanvasPrefab;

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


        listenerCallbacksServerPrefab = Resources.Load<ListenerCallbacksServer>(PATH_PREFAB_LISTENER_CALLBACKS_SERVER);

        if (listenerCallbacksServerPrefab == null)
        {
            throw new JoinLocationException("listener callbacks server prefab not found");
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
        DestroyLoadingWaitWindow();
        Instantiate(listenerCallbacksServerPrefab);
        InstantiatePlayerObject(characterList.GetCharacter(0).gameObject, Vector3.zero);


        mainCanvasPrefab = Resources.Load<GameObject>("Prefabs/UI/MainCanvas");

        if (mainCanvasPrefab == null)
        {
            throw new InitilizatorLocationException("main canvas prefab not found");
        }

        GameObject mainCanvas = Instantiate(mainCanvasPrefab);


        Instantiate(panelNameLocationPrefab, mainCanvas.transform).SetText(locationData.NameLocation);

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




}