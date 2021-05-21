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
        try
        {
            Destroy(gameObject);
        }
        catch 
        {

        }
    }

    public void Remove(float time)
    {
        try
        {
        Destroy(gameObject, time);
        }
        catch 
        {

        }

    }

}