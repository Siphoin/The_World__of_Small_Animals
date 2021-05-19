using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
    public class ArrowTeleport : MonoBehaviour, ISeterSprite, IInvokerMono
    {
    private const string TAG_MY_PLAYER = "MyPlayer";
    private ArrowTeleportSettings arrowTeleportSettings;

    private SpriteRenderer spriteRenderer;

    private bool clicked = false;

    [Header("Имя локации")]
    [SerializeField] string locationName;
        // Use this for initialization
        void Start()
        {
        arrowTeleportSettings = Resources.Load<ArrowTeleportSettings>("Data/Arrow Teleport Button/ArrowTeleportSettings");

        if (arrowTeleportSettings == null)
        {
            throw new ArrowTeleportException("arrow teleport settings not found");
        }

        if (!TryGetComponent(out spriteRenderer))
        {
            throw new ArrowTeleportException($"{name} not have component Sprite Renderer");
        }

        SetSprite(arrowTeleportSettings.IdleSprite);
    }



    private void OnMouseDown()
    {
        clicked = true;
    }

    private void OnMouseEnter()
    {
        SetSprite(arrowTeleportSettings.ActiveMouseEnterSprite);
    }

    private void OnMouseExit()
    {
        SetSprite(arrowTeleportSettings.IdleSprite);
    }

    public void SetSprite (Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TAG_MY_PLAYER))
        {
            if (clicked)
            {
                CallInvokingMethod(LoadLocation, arrowTeleportSettings.TimeOutTeleport);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MyPlayer"))
        {
            if (clicked)
            {
                CancelInvoke();
            }
        }
    }

    private void LoadLocation()
    {
        Loading.LoadScene(locationName);
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