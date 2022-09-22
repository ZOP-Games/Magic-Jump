using GameExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class MenuScreen : MonoBehaviour
{
    protected abstract GameObject GObj { get; }
    protected Warehouse wh;
    public abstract bool IsActive { get; protected set; }
    

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
