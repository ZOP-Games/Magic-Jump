using TMPro;
using UnityEngine;

namespace GameExtensions
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField]private TextMeshPro hpText;
        private EntityStateManager entity;
        private const string HeadText = "HP: ";

        private void Start()
        {
            entity = GetComponentInParent<EntityStateManager>();
            entity.HealthChanged += () => hpText.SetText(HeadText + entity.Hp);
            hpText.SetText(HeadText + entity.Hp);
        }
    }
}