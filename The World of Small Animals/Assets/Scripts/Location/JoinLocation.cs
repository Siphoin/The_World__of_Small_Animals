using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class JoinLocation : MonoBehaviourPunCallbacks
    {
    private LoadingWait loadingWait;
        // Use this for initialization
        void Start()
        {
        if (LoadingWaitManager.Manager == null)
        {
            throw new JoinLocationException("loading wait manager not found");
        }

       loadingWait = LoadingWaitManager.Manager.CreateLoadingWait();
        loadingWait.SetText("Заходим на локацию...");



        PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name, new RoomOptions(), TypedLobby.Default);
        }

    public override void OnJoinedRoom()
    {
        loadingWait.Remove();
    }

}