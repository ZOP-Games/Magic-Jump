using System;
using GameExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public abstract class MenuScreen : MonoBehaviour
{
    protected abstract GameObject GObj { get; }
    public abstract bool IsActive { get; protected set; }
    public abstract event EventHandler Opened;
    public abstract event EventHandler Closed;

    public virtual void Open()
    {
        IsActive = true;
        GObj.SetActive(true);
    }
    public virtual void Close()
    {
        IsActive = false;
        GObj.SetActive(false);
    }

}
