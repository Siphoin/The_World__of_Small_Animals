using System;
using UnityEngine;

public abstract class AuthComponent : MonoBehaviour, IRemoveObject
    {

    protected bool isAuthing = false;

    protected string idRequest;

    public event Action OnAuth;
    public event Action OnAuthFinish;
    public event Action OnAuthFalled;

    protected RequestManager requestManager;

    public string TokenActive { get; protected set; }

    public virtual void Ini () => DontDestroyOnLoad(gameObject);

    protected void SendEventAuthFalled() => OnAuthFalled?.Invoke();

    protected void SendEventAuthFinish() => OnAuthFinish?.Invoke();

    protected void SendEventAuth() => OnAuth?.Invoke();

    public void Remove() => Destroy(gameObject);

    public void Remove(float time) => Destroy(gameObject, time);

}