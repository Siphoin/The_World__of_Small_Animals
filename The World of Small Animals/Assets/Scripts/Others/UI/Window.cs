using System;
using UnityEngine;
public class Window : MonoBehaviour, IRemoveObject
    {
    public event Action<Window> onExit;

    public virtual void Exit ()
    {
        onExit?.Invoke(this);
        Remove();
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