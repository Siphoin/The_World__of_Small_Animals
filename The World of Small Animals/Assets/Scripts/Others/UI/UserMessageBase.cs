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

    private const string PATH_DATA_PARAMS_USER_MESSAGE = "Params/UserMessageParams";

    private RectTransform rectTransform;

    private UserMessageParams userMessageParams;

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

        if (userMessageParams == null)
        {
            userMessageParams = Resources.Load<UserMessageParams>(PATH_DATA_PARAMS_USER_MESSAGE);

            if (userMessageParams == null)
            {
                throw new UserMessageException("user message params not found");
            }

        }
        if (photonView == null)
        {

        if (!TryGetComponent(out photonView))
        {
            throw new UserMessageException($"{name} not have component Photon View");
        }


        }

        if (image == null)
        {
            if (!TryGetComponent(out image))
            {
                throw new UserMessageException($"{name} not have component Image");
            }
        }

        if (rectTransform == null)
        {
            if (!TryGetComponent(out rectTransform))
            {
                throw new UserMessageException($"{name} not have component Rect Transform");
            }
        }


        Transform parentGrid = GameObject.Find($"{PREFIX_GRID_MESSAGES}{photonView.Owner.NickName}").transform;


        transform.SetParent(parentGrid);

        rectTransform.localRotation = Quaternion.identity;

        if (photonView.IsMine)
        {
            CallInvokingMethod(Remove, userMessageParams.TimeDestroyUserMessage);
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

    public virtual void CheckCloudMessageisLast()
    {

        if (isLast)
        {
            image.color = GetAlphaColor(image.color);
            
        }
    }

    protected void CheckParamsMessage ()
    {
        if (isLast)
        {
            return;
        }


        if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
        {
            isLast = true;
            CheckCloudMessageisLast();
            return;
        }
    }

    protected Color GetAlphaColor(Color original)
    {
        Color alphaColor = CloudImage.color;
        alphaColor.a = 0.5f;
        return alphaColor;
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