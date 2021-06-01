using System;
using System.Collections;
using UnityEngine;

    public class AuthComponent : MonoBehaviour, IRemoveObject
    {

    public event Action onAuth;
    public event Action onAuthFinish;
    public event Action onAuthFalled;

    protected RequestManager requestManager;

    protected string tokenActive = null;

    protected bool isAuthing = false;

    protected string idRequest;

    public virtual void Ini ()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected void SendEventAuthFalled()
    {
        onAuthFalled?.Invoke();
    }

    protected void SendEventAuthFinish()
    {
        onAuthFinish?.Invoke();
    }

    protected void SendEventAuth()
    {
        onAuth?.Invoke();
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void Remove(float time)
    {
        Destroy(gameObject, time);
    }

}