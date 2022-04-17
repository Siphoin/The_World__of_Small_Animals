using Photon.Pun;
using Photon.Realtime;

public class ListenerCallbacksServer : MonoBehaviourPunCallbacks
{
    private WindowNotfication _activeErrorNotfication;

    private void CreateErrorNotfication (string textError)
    {
        if (_activeErrorNotfication != null)
        {
            return;
        }
        _activeErrorNotfication = ManagerWindowsNotfications.Manager.CreateNotfication(textError, MessageNotficationType.Error, true);
    }

    #region Server Callbacks 



    public override void OnDisconnected(DisconnectCause cause) => CreateErrorNotfication($"Соединение с сервером было прервано.\nКод ошибки: {cause} Проверьте подключение к сети Интернет.");

    public override void OnLeftRoom() => CreateErrorNotfication($"Соединение с локацией было прервано. Проверьте подключение к сети Интернет.");

    #endregion
}
