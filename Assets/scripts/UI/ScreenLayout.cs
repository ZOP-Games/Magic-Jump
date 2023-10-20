using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace GameExtensions.UI
{
    public class ScreenLayout : UIComponent
    {
        [SerializeField] protected GameObject firstObj;

        protected void Start()
        {
            UnityEngine.Debug.Assert(ES.currentInputModule is InputSystemUIInputModule,
                "Current Input Module is not of type InputSystemUIInputModule.");
        }

        protected void OnEnable()
        {
            ES.SetSelectedGameObject(firstObj);
        }
    }
}