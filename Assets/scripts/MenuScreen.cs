using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class MenuScreen : MonoBehaviour
{
    protected abstract PlayerInput PInput { get; }
    protected abstract GameObject GObj { get; }

    public virtual void Open()
    {
        GObj.SetActive(true);
    }
    public virtual void Close()
    {
        GObj.SetActive(false);
    }
    
}
