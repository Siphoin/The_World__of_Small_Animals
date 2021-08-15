using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerSelectCharacters : MonoBehaviour, IActivatorGameObject, IRequestSender, IInvokerMono
    {
    private const string PATH_PREFAB_CARD_CHARACTER = "Prefabs/UI/cardCharacter_select_characters";


    [Header("URL для получения информации о персонажах пользователя")]
    [TextArea] private string urlGetCharacters = "users/characterList";


    [Header("Контейнер выбора персонажей")]
    [SerializeField] private Transform containerCharacters;

    [Header("Изображение ошибки от сервера")]
    [SerializeField] private GameObject warmingImage;

    [Header("Изображение ожидания ответа от сервера")]
    [SerializeField] private GameObject loadingImage;

    [Header("Изображение отсутствия персонажей")]
    [SerializeField] private GameObject charactersNotFoundImage;

    [Header("Кнопка обновить список персонажей")]
    [SerializeField] private Button buttonRefresh;

    private string idRequest;

    private RequestManager requestManager;

    private CardCharacterButton cardCharacterButtonPrefab;

    private CharacterRequestData[] dataCharacters;

    private CharacterRequestData currentSelectedCharacter;

    private List<CardCharacterButton> cards = new List<CardCharacterButton>();

    public event Action<CharacterRequestData> onCharacterSelected;




    // Use this for initialization
    void Start()
        {
        Ini();
        }

    private void Ini ()
    {
        if (warmingImage == null)
        {
            throw new ContainerSelectCharactersException("warming image not seted");
        }

        if (loadingImage == null)
        {
            throw new ContainerSelectCharactersException("loading image not seted");
        }

        if (containerCharacters == null)
        {
            throw new ContainerSelectCharactersException("container characters not seted");
        }

        if (charactersNotFoundImage == null)
        {
            throw new ContainerSelectCharactersException("characters not found image not seted");
        }

        if (buttonRefresh == null)
        {
            throw new ContainerSelectCharactersException("button refresh not seted");
        }


        cardCharacterButtonPrefab = Resources.Load<CardCharacterButton>(PATH_PREFAB_CARD_CHARACTER);

        if (cardCharacterButtonPrefab == null)
        {
            throw new ContainerSelectCharactersException("prefab card character button not found");
        }


        requestManager = RequestManager.Manager;
            SetStateVisibleRefreshButton(false);

        buttonRefresh.onClick.AddListener(RefreshCharactersList);
        SendRequest();
    }

    public void ReceiveRequest(string id, string text, RequestResult requestResult, long responseCode)
    {
       if (id == idRequest)
        {
          if (requestResult == RequestResult.OK && responseCode == 200)
            {
                try
                {
                    dataCharacters = JsonConvert.DeserializeObject<CharacterRequestData[]>(text);
                    LoadCardsCharacters();


                    SwitchImageState(requestResult);

                }
                catch (ContainerSelectCharactersException)
                {
                    SwitchImageState(RequestResult.Error);
                    throw;
                }
            }

          else
            {
                SwitchImageState(RequestResult.Error);
                SetStateVisibleRefreshButton(true);
            }

            UncribeEventsRequestManager();
        }
    }

    public void SendRequest()
    {
        idRequest = requestManager.GenerateRequestID();

        requestManager.OnRequestFinish += ReceiveRequest;

        Dictionary<string, object> form = new Dictionary<string, object>();


        form.Add("id", AuthUser.Manager.UserData.id);
        form.Add("token", AuthUser.Manager.TokenActive);


        requestManager.SendRequestToServer(idRequest, urlGetCharacters, RequestType.POST, form);
    }

    public void SetActiveSelfGameObject(GameObject gameObject, bool activeState)
    {
        gameObject.SetActive(activeState);
    }

    private void SwitchImageState(RequestResult requestResult)
    {
        SetActiveSelfGameObject(warmingImage, requestResult == RequestResult.Error);
        SetActiveSelfGameObject(loadingImage, false);
    }

    private void SetStateVisibleRefreshButton (bool state) => SetActiveSelfGameObject(buttonRefresh.gameObject, state);

    private void UncribeEventsRequestManager () => requestManager.OnRequestFinish -= ReceiveRequest;

    private void LoadCardsCharacters ()
    {
        if (dataCharacters.Length == 0)
        {
            SetActiveSelfGameObject(charactersNotFoundImage, true);

            return;
        }

        
        for (int i = 0; i < dataCharacters.Length; i++)
        {
        CardCharacterButton newCard = Instantiate(cardCharacterButtonPrefab, containerCharacters);
            newCard.SetTextNameCardIsDefault = false;
            newCard.SetData(dataCharacters[i]);
            newCard.SetText(dataCharacters[i].name);
            newCard.onSelect += Select;
            cards.Add(newCard);
        }

    }

    private void Select(CharacterRequestData obj)
    {

        if (currentSelectedCharacter != null && obj == currentSelectedCharacter)
        {
            return;
        }
        currentSelectedCharacter = obj;

        for (int i = 0; i < cards.Count; i++)
        {
            CardCharacterButton card = cards[i];
            card.SetActiveSelectingCameraAngle(card.CurrentData == obj);
        }

        onCharacterSelected?.Invoke(currentSelectedCharacter);


    }


    private void RefreshCharactersList ()
    {
        for (int i = 0; i < containerCharacters.childCount; i++)
        {
            GameObject go = containerCharacters.GetChild(i).gameObject;
            Destroy(go);
        }

        cards.Clear();

        dataCharacters = new CharacterRequestData[0];


        currentSelectedCharacter = null;


        SetActiveSelfGameObject(warmingImage, false);
        SetActiveSelfGameObject(loadingImage, true);
        SetActiveSelfGameObject(charactersNotFoundImage, false);

        SetStateVisibleRefreshButton(false);

        SendRequest();
    }

    private void OnDestroy()
    {
        try
        {
            if (requestManager)
            {
                UncribeEventsRequestManager();
            }
        }
        catch
        {

            
        }
    }

    public void CallInvokingEveryMethod(Action method, float time)
    {
        InvokeRepeating(method.Method.Name, time, time);
    }

    public void CallInvokingMethod(Action method, float time)
    {
        Invoke(method.Method.Name, time);
    }
}