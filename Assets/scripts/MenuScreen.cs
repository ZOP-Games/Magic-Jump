using System;
using GameExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public abstract class MenuScreen : MonoBehaviour
{
    protected GameObject GObj => gameObject;
    protected MenuController Controller => MenuController.GetInstance();
    protected MenuScreen Parent => transform.parent.GetComponent<MenuScreen>();

    public virtual void Open()
    {
        Controller.ActiveScreen = this;
        GObj.SetActive(true);
    }
    public virtual void Close()
    {
        Controller.ActiveScreen = Parent;
        GObj.SetActive(false);
    }

}
