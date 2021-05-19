using UnityEngine;
[CreateAssetMenu(menuName = "Data/Server/Server Data", order = 0)]
public class ServerSlotData : ScriptableObject
    {
        [Header("Название сервера")]
        [SerializeField]
        private string nameServer = "NameServer";

        [Header("AppId сервера")]
        [TextArea]
        [SerializeField]
        private string appId = "AppId";

        [Header("Сервер для разработчиков")]
        [SerializeField]
        private bool developServer = false;

        public string NameServer { get => nameServer; }
        public string AppId { get => appId; }
        public bool DevelopServer { get => developServer; }
    }