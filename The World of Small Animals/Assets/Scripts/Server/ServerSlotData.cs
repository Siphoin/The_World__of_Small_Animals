using UnityEngine;
[CreateAssetMenu(menuName = "Data/Server/Server Data", order = 0)]
public class ServerSlotData : ScriptableObject
    {
        [Header("Название сервера")]
        [SerializeField]
        private string _nameServer = "NameServer";

        [Header("AppId сервера")]
        [TextArea]
        [SerializeField]
        private string _appId = "AppId";

        [Header("Максимальное количество игроков на сервере")]
        [SerializeField]
        private long _maxCountPlayers = 20;

        [Header("Сервер для разработчиков")]
        [SerializeField]
        private bool _developServer = false;

        public string NameServer  => _nameServer;
        public string AppId  => _appId;
        
        public long MaxCountPlayers =>  _maxCountPlayers;
        
        public bool DevelopServer  => _developServer;
       
}
