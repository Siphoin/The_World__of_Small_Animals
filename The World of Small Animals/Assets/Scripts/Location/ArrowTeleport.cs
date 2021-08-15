using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
    public class ArrowTeleport : MonoBehaviour, ISeterSprite, IInvokerMono
    {
    private const string TAG_MY_PLAYER = "MyPlayer";

    private const string PATH_DATA_ARROW_SETTINGS = "Data/Arrow Teleport Button/ArrowTeleportSettings";

    private bool _isClicked = false;

    [Header("Имя локации")]
    [SerializeField] private string _locationName;

    private ArrowTeleportSettings _arrowTeleportSettings;

    private SpriteRenderer _spriteRenderer;

        // Use this for initialization
        void Start()
        {
        _arrowTeleportSettings = Resources.Load<ArrowTeleportSettings>(PATH_DATA_ARROW_SETTINGS);

        if (_arrowTeleportSettings == null)
        {
            throw new ArrowTeleportException("arrow teleport settings not found");
        }

        if (!TryGetComponent(out _spriteRenderer))
        {
            throw new ArrowTeleportException($"{name} not have component Sprite Renderer");
        }

        SetSprite(_arrowTeleportSettings.IdleSprite);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TAG_MY_PLAYER))
        {
            if (_isClicked)
            {
                CallInvokingMethod(LoadLocation, _arrowTeleportSettings.TimeOutTeleport);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TAG_MY_PLAYER))
        {
            if (_isClicked)
            {
                CancelInvoke();
            }
        }
    }

    private void LoadLocation()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        Loading.LoadScene(_locationName);
    }

    public void CallInvokingEveryMethod(Action method, float time)
    {
        InvokeRepeating(method.Method.Name, time, time);
    }

    public void CallInvokingMethod(Action method, float time)
    {
        Invoke(method.Method.Name, time);
    }

    private void OnMouseDown() => _isClicked = true;

    private void OnMouseEnter() => SetSprite(_arrowTeleportSettings.ActiveMouseEnterSprite);

    private void OnMouseExit() => SetSprite(_arrowTeleportSettings.IdleSprite);

    public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;


}