using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// ReSharper disable UseNullPropagation

public abstract class MenuScreen : MonoBehaviour
{
    protected GameObject GObj => gameObject;
    protected static MenuController Controller => MenuController.Controller;
    public MenuScreen Parent => transform.parent.GetComponent<MenuScreen>();
    protected static EventSystem ES => EventSystem.current;

    public virtual void Open()
    {
        Controller.ActiveScreen = this;
        GObj.SetActive(true);
        var firstButton = GetComponentInChildren<Button>();
        if(firstButton is not null) ES.SetSelectedGameObject(firstButton.gameObject);
    }
    public virtual void Close()
    {
        Controller.ActiveScreen = Parent;
        if(Parent is not null) Parent.Open();
        GObj.SetActive(false);
    }

}
