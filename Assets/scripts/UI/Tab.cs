using UnityEngine;
using UnityEngine.UI;

namespace GameExtensions.UI
{
    public class Tab : MonoBehaviour
    {
        [SerializeField] private Color activeColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private GameObject menuLayout;
        private Image background;

        public void Activate()
        {
            background.color = activeColor;
            menuLayout.SetActive(true);
        }

        public void Deactivate()
        {
            background.color = disabledColor;
            menuLayout.SetActive(false);
        }

        private void Start()
        {
            background = GetComponent<Image>();
            Deactivate();
        }
    }
}