using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
    public class WindowPictureBanners : MonoBehaviour, IRequestSender, IRequestSenderGetTexture, IActivatorGameObject, ISeterColor, IInvokerMono
    {


    private const string PATH_PREFAB_BUTTON_BANNERS_PAGE = "Prefabs/UI/buttonPageBannerPictures";


    [Header("URL получения информации о баннерах")]
    [TextArea]
    [SerializeField] string url = "images/public/picSelectCharactersBanners";

    [Header("Задержка пролистывания нового баннера")]
    [SerializeField] private float timeOutNextBanners = 5;

    [Header("Изображение ошибки от сервера")]
    [SerializeField] private GameObject warmingImage;

    [Header("Изображение ожидания ответа от сервера")]
    [SerializeField] private GameObject loadingImage;

    [Header("Контейнер кнопок страниц")]
    [SerializeField] private Transform gridButtons;

    private WindowPictureBannersTypeRequest typeRequestStep = WindowPictureBannersTypeRequest.GetCount;

    private string idRequest;

    private int currentIndexBanner = 0;

    private ButtonPageBanner buttonPageBannerPrefab;

    private RequestManager requestManager;

    private List<ButtonPageBanner> buttons = new List<ButtonPageBanner>();

    private Sprite[] pictures;

    private Sprite oldGetedSprite;

    private bool textureResponseWait = false;

    private Image image;


    // Use this for initialization
    void Start()
        {
        Ini();
        }


    private void Ini()
    {
        if (timeOutNextBanners <= 0)
        {
            throw new WindowPictureBannersException("time out next banners has a invalid value");
        }
        if (warmingImage == null)
        {
            throw new WindowPictureBannersException("warming image not seted");
        }

        if (loadingImage == null)
        {
            throw new WindowPictureBannersException("loading image not seted");
        }

        if (gridButtons == null)
        {
            throw new WindowPictureBannersException("grid buttons not seted");
        }

        if (!TryGetComponent(out image))
        {
            throw new WindowPictureBannersException($"{name} not have component Image");
        }


        buttonPageBannerPrefab = Resources.Load<ButtonPageBanner>(PATH_PREFAB_BUTTON_BANNERS_PAGE);

        if (buttonPageBannerPrefab == null)
        {
            throw new WindowPictureBannersException("prefab button page banners not found");
        }

        requestManager = RequestManager.Manager;

        SetStateVisiblePagesButtons(false);

        SendRequest();
    }

    public void SendRequest()
    {
        idRequest = requestManager.GenerateRequestID();

        switch (typeRequestStep)
        {
            case WindowPictureBannersTypeRequest.GetCount:

                requestManager.OnRequestFinish += ReceiveRequest;

                requestManager.SendRequestToServer(idRequest, url, RequestType.GET, null, false);

                break;



            case WindowPictureBannersTypeRequest.GetPictures:
                break;
            default:
                throw new WindowPictureBannersException("type step request invalid");
        }
    }

    public void ReceiveRequest(string id, string text, RequestResult requestResult, long responseCode)
    {
        if (id == idRequest)
        {
            if (requestResult == RequestResult.Error)
            {
                SwitchImageState(requestResult);
                return;
            }

            else
            {
                if (responseCode == 200)
                {
                    try
                    {
                        CountObject countObject = JsonConvert.DeserializeObject<CountObject>(text);

                        int countPictures = (int)countObject.Count;


                        LoadButtonsPageBanners(countPictures);

                        typeRequestStep = WindowPictureBannersTypeRequest.GetPictures;

                        StartCoroutine(AsyncLoadBannersPictures(countPictures));

                    }
                    catch (Exception E)
                    {
#if UNITY_EDITOR
                        Debug.LogError(E.Message);
#endif

                        SwitchImageState(RequestResult.Error);

                    }
                }
            }

            requestManager.OnRequestFinish -= ReceiveRequest;
        }
    }

    public void ReceiveTexture(string id, Texture2D texture, RequestResult requestResult, long responseCode)
    {
        if (id == idRequest)
        {
            if (requestResult == RequestResult.OK && responseCode == 200)
            {
                Debug.Log(texture.graphicsFormat);
                Rect rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                oldGetedSprite = Sprite.Create(texture, rect, pivot);

                textureResponseWait = false;
            }

            else
            {
                StopAllCoroutines();
                SwitchImageState(RequestResult.Error);
            }
        }
    }

   private void SwitchImageState (RequestResult requestResult)
    {
        SetActiveSelfGameObject(warmingImage, requestResult == RequestResult.Error);
        SetActiveSelfGameObject(loadingImage, false);
    }

    public void SetActiveSelfGameObject(GameObject gameObject, bool activeState)
    {
        gameObject.SetActive(activeState);
    }

    private void SetStateVisiblePagesButtons (bool state)
    {
        SetActiveSelfGameObject(gridButtons.gameObject, state);
    }

    private void LoadButtonsPageBanners (int count)
    {
        for (int i = 0; i < count; i++)
        {
            ButtonPageBanner button = Instantiate(buttonPageBannerPrefab, gridButtons);
            button.SetIndexPage(i);
            button.onSelect += SetBannerStopingInvoke;
            buttons.Add(button);
        }


    }

    private IEnumerator AsyncLoadBannersPictures (int count)
    {

        requestManager.OnRequestGetTextureFinish += ReceiveTexture;


        List<Sprite> sprites = new List<Sprite>();

#if UNITY_EDITOR
        Debug.Log($"start load banners pictures: count: {count}");
#endif


        int i = 0;
        while (true)
        {
            if (i < count)
            {
                yield return new WaitForSeconds(0.5f);


                idRequest = requestManager.GenerateRequestID();
               
                requestManager.SendRequestGetTextureToServer(idRequest, $"{url}/{i + 1}", false);

                textureResponseWait = true;

               if (textureResponseWait)
                {
                    yield return new WaitForSeconds(1.0f / 60.0f);
                }

                sprites.Add(oldGetedSprite);
                i++;

            }

            else
            {
#if UNITY_EDITOR
                Debug.Log("pictures banners is loaded");
#endif
                requestManager.OnRequestGetTextureFinish -= ReceiveTexture;

                pictures = sprites.ToArray();


                SwitchImageState(RequestResult.OK);


                SetStateVisiblePagesButtons(true);

                CallInvokingEveryMethod(NextBanner, timeOutNextBanners);

                SetColor(Color.white);

                SetBanner(0);
                yield break;
            }
        }
    }

    private void NextBanner()
    {

         if (currentIndexBanner < pictures.Length - 1)
        {
            currentIndexBanner++;
        }

         else
        {
            currentIndexBanner = 0;
        }
        SetBanner(currentIndexBanner);
    }



    public void SetColor(Color color)
    {
        image.color = color;
    }

    private void SetBanner (int index)
    {
        image.sprite = pictures[index];
        currentIndexBanner = index;
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetStateColor(index);
        }
    }

    private void SetBannerStopingInvoke(int index)
    {
        CancelInvoke();
        CallInvokingEveryMethod(NextBanner, timeOutNextBanners);

        SetBanner(index);
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