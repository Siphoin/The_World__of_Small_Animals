using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class WindowSelectCharacter : MonoBehaviour, ICallerLoadingWaitWindow
    {
    [Header("Контейнер персонажей")]
    [SerializeField] private ContainerSelectCharacters _container;

    [Header("Кнопка ИГРАТЬ")]
    [SerializeField] private Button _buttonPlay;

    private LoadingWait _loadingWait;

    private AuthCharacter _authCharacter;

    private CharacterRequestData _characterTargetData = null;

    


    void Start()
        {
        if (_container == null)
        {
            throw new WindowSelectCharacterException("container characters not seted");
        }

        if (_buttonPlay == null)
        {
            throw new WindowSelectCharacterException("button play not seted");
        }

        _buttonPlay.onClick.AddListener(JoinOnGame);

        _authCharacter = AuthCharacter.Manager;

        _authCharacter.OnAuthFinish += AuthingCharacter;
        _container.onCharacterSelected += Selecting;


    }

    private void AuthingCharacter()
    {
        PhotonNetwork.NickName = _authCharacter.CharacterData.name;

        UncribeEvents();

        Loading.LoadScene("servers");
    }

    private void JoinOnGame()
    {
       if (_characterTargetData == null)
        {
            ManagerWindowsNotfications.Manager.CreateNotfication("Пожалуйста, выбери персонажа чтобы начать играть.");
        }

        _authCharacter.SetIdCharacter(_characterTargetData.id);

        TokenString token = new TokenString(AuthUser.Manager.TokenActive);

        _authCharacter.Auth(token);
    }

    public void DestroyLoadingWaitWindow() => _loadingWait.Remove();

    private void UncribeEvents ()
    {
        _authCharacter.OnAuthFinish -= AuthingCharacter;

        _container.onCharacterSelected -= Selecting;
    }

    private void Selecting(CharacterRequestData data) => _characterTargetData = data;

    private void OnDestroy()
    {
        try
        {
            UncribeEvents();
        }
        catch
        {
        }
    }




}