using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using shortid;
using Newtonsoft.Json;

    public class ServerListWindow : MonoBehaviourPunCallbacks, ICallerLoadingWaitWindow, IRequestSender
    {
        private const string PATH_PREFAB_BUTTON_SELECT_SERVER = "Prefabs/UI/buttonServerSlot";

        private const string START_SCENE = "SampleScene";

        private const string PATH_DATA_SLOTS_SERVERS = "Data/ServerSlots";

        private const string PATH_REQUEST_DATA_SERVERS = "servers/";

        private string _idRequest;

        [Header("Задержка перед появлением списка кнопок выбора сервера")]
        [SerializeField] float _timeoutSpawnButtons = 2.1f;

        [Header("Задержка перед появлением кнопки выбора сервера")]
        [SerializeField] float _timeoutSpawnButton = 0.5f;

        [Header("Контент для кнопок")]
        [SerializeField] private Transform _contentButtons;

        private ServerSlotData[] _slotsData;

        private ServerRequestData[] _serversRequestData;

        private ServerSlot _slotServerPrefab;

        private LoadingWait _loadingWait;

        private RequestManager _requestManager;
        // Use this for initialization
        void Start()
        {
            if (_timeoutSpawnButton <= 0)
            {
                throw new ServerListWindowException("time out spawn button not valid");
            }

            if (_timeoutSpawnButtons <= 0)
            {
                throw new ServerListWindowException("time out spawn buttons not valid");
            }

            _slotServerPrefab = Resources.Load<ServerSlot>(PATH_PREFAB_BUTTON_SELECT_SERVER);

            if (_slotServerPrefab == null)
            {
                throw new ServerListWindowException("slot server prefab not found");
            }

            _slotsData = Resources.LoadAll<ServerSlotData>(PATH_DATA_SLOTS_SERVERS);

            if (_slotsData.Length == 0)
            {
                throw new ServerListWindowException("slots not found");
            }
            _requestManager = RequestManager.Manager;

            SendRequest();
        }

        private void ShoeServerSlots (Func<ServerSlotData, bool> predicate)
        {
            ServerSlotData[] array = _slotsData.Where(predicate).ToArray();

            StartCoroutine(CreatingAsyncSlots(array));
        }

        private IEnumerator CreatingAsyncSlots (ServerSlotData[] slots)
        {
            yield return new WaitForSeconds(_timeoutSpawnButtons);

            int i = 0;


            while (i < slots.Length)
            {
                yield return new WaitForSeconds(_timeoutSpawnButton);

                ServerSlot slot = Instantiate(_slotServerPrefab, _contentButtons);

                slot.SetData(slots[i]);

                slot.OnClick += SelectServer;

                try
                {
                    float countPlayers = _serversRequestData.First(x => x.name == slots[i].NameServer).countPlayers;

                    slot.SetOccupancyRate(countPlayers);
                }
                catch
                {


                }
                i++;

                yield return null;
            }
        }

        private void SelectServer(ServerSlotData data)
        {
            _loadingWait = LoadingWaitManager.Manager.CreateLoadingWait();
            _loadingWait.SetText($"Подключение к серверу {data.NameServer}");

            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = data.AppId;


            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }

            SetLevelLoggingServer();
            PhotonNetwork.ConnectUsingSettings();
        }

        private static void SetLevelLoggingServer()
        {
#if UNITY_EDITOR
            PhotonNetwork.LogLevel = PunLogLevel.Informational;

            PhotonNetwork.PhotonServerSettings.EnableSupportLogger = true;

#else
            PhotonNetwork.LogLevel = PunLogLevel.ErrorsOnly;

            PhotonNetwork.PhotonServerSettings.EnableSupportLogger = false;

#endif
        }

        public void DestroyLoadingWaitWindow()
        {
            _loadingWait.Remove();
        }
        #region Server Callbacks


        public override void OnConnectedToMaster()
        {
            DestroyLoadingWaitWindow();

            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
            {
                PhotonNetwork.NickName = $"Avatar_{ShortId.Generate(false, false, 10 + PhotonNetwork.CountOfPlayers)}";
            }


            Loading.LoadScene(START_SCENE);
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            DestroyLoadingWaitWindow();

            ManagerWindowsNotfications.Manager.CreateNotfication($"Не удалось подключиться к серверу.\n Код ошибки: {cause}\n Возможно сервер переполнен. Проверьте подключение к Интернету.", MessageNotficationType.Error);
        }

        public void SendRequest()
        {
            _idRequest = _requestManager.GenerateRequestID();

            _requestManager.OnRequestFinish += ReceiveRequest;

            _requestManager.SendRequestToServer(_idRequest, PATH_REQUEST_DATA_SERVERS);


            _loadingWait = LoadingWaitManager.Manager.CreateLoadingWait();
            _loadingWait.SetText("Получение данных о списке серверов...");
        }

        public void ReceiveRequest(string id, string text, RequestResult requestResult, long responseCode)
        {
            if (_idRequest == id)
            {
                DestroyLoadingWaitWindow();
            }

            if (requestResult == RequestResult.OK && responseCode == 200)
            {
                _requestManager.OnRequestFinish -= ReceiveRequest;

                try
                {
                _serversRequestData = JsonConvert.DeserializeObject<ServerRequestData[]>(text);
                }
                catch 
                {

                    ManagerWindowsNotfications.Manager.CreateNotfication("Произошла системная ошибка игры.", MessageNotficationType.Error);
                }


#if UNITY_EDITOR
                ShoeServerSlots(serverUsers => serverUsers.DevelopServer);
#endif


                ShoeServerSlots(serverUsers => serverUsers.DevelopServer == false);
            }
        }

        #endregion

    }
