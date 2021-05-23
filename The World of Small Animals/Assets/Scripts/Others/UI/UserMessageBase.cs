using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
    public class UserMessageBase : MonoBehaviour, IRemoveObject, IInvokerMono
{
    private PhotonView photonView;

    private const string PREFIX_GRID_MESSAGES = "Grid_Messages_";

    private RectTransform rectTransform;

    private Image image;

    protected bool isLast = false;

    protected PhotonView View { get => photonView; }
    public Image CloudImage { get => image; }

    // Use this for initialization

    public virtual void Ini ()
    {
        if (!PhotonNetwork.IsConnected)
        {

#if UNITY_EDITOR
            Debug.Log($"user message {name} removed, because client not connected on Photon Networking");
#endif
            Remove();

        }
        if (photonView == null)
        {


        if (!TryGetComponent(out photonView))
        {
            throw new UserMessageException($"{name} not have component Photon View");
        }

            if (rectTransform == null)
            {
                if (!TryGetComponent(out rectTransform))
                {
                    throw new UserMessageException($"{name} not have component Rect Transform");
                }
            }

            if (image == null)
            {
                if (!TryGetComponent(out image))
                {
                    throw new UserMessageException($"{name} not have component Image");
                }
            }
        }


        Transform parentGrid = GameObject.Find($"{PREFIX_GRID_MESSAGES}{photonView.Owner.NickName}").transform;


        transform.SetParent(parentGrid);

        rectTransform.localRotation = Quaternion.identity;

        if (photonView.IsMine)
        {
            CallInvokingMethod(Remove, 7);
        }

       
    }

    public void Remove()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void Remove(float time)
    {
        
        if (PhotonNetwork.IsConnected)
        {
            if (photonView.IsMine)
            {
                CallInvokingMethod(Remove, 3);
            }

        }

        else
        {
            Destroy(gameObject, time);
        }
    }

    public void CheckCloudMessageisLast()
    {
        Color alphaColor = image.color;
        alphaColor.a = 0.5f;


        if (isLast)
        {
            image.color = alphaColor;
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