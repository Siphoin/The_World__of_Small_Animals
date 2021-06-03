using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WindowSelectCharacter : MonoBehaviour, ICallerLoadingWaitWindow
    {
    [Header("Контейнер персонажей")]
    [SerializeField] private ContainerSelectCharacters container;

    [Header("Кнопка ИГРАТЬ")]
    [SerializeField] private Button buttonPlay;

    private LoadingWait loadingWait;

    private AuthCharacter authCharacter;

    private CharacterRequestData characterTargetData = null;

    


    // Use this for initialization
    void Start()
        {
        if (container == null)
        {
            throw new WindowSelectCharacterException("container characters not seted");
        }

        if (buttonPlay == null)
        {
            throw new WindowSelectCharacterException("button play not seted");
        }

        buttonPlay.onClick.AddListener(JoinOnGame);

        authCharacter = AuthCharacter.Manager;

        authCharacter.onAuthFinish += AuthingCharacter;
        container.onCharacterSelected += Selecting;


    }

    private void AuthingCharacter()
    {
        PhotonNetwork.NickName = authCharacter.CharacterData.name;

        UncribeEvents();

        Loading.LoadScene("servers");
    }

    private void JoinOnGame()
    {
       if (characterTargetData == null)
        {
            ManagerWindowsNotfications.Manager.CreateNotfication("Пожалуйста, выбери персонажа чтобы начать играть.");
        }

        authCharacter.SetIdCharacter(characterTargetData.id);

        TokenString token = new TokenString(AuthUser.Manager.TokenActive);

        authCharacter.Auth(token);
    }

    public void DestroyLoadingWaitWindow()
    {
        loadingWait.Remove();
    }

    private void UncribeEvents ()
    {
        authCharacter.onAuthFinish -= AuthingCharacter;
        container.onCharacterSelected -= Selecting;
    }

    private void Selecting(CharacterRequestData obj)
    {
        characterTargetData = obj;
    }

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