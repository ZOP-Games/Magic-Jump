using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

namespace GameExtensions.UI.HUD
{
    public class MinimapIndicatorCreator : MonoBehaviour
    {
        [SerializeField] private GameObject indicatorObject;
        [SerializeField] private Material material;

        private Transform constraintSource;
        private GameObject marker;

        public void Start()
        {
            constraintSource = GetComponent<Transform>();
            marker = Instantiate(indicatorObject, Vector3.zero, Quaternion.identity);
            var posConstraint = marker.GetComponent<PositionConstraint>();
            var sauce = new ConstraintSource
            {
                sourceTransform = constraintSource,
                weight = 1
            };
            posConstraint.AddSource(sauce);
            var posAtRest = posConstraint.translationAtRest;
            posAtRest.y = 0;
            posConstraint.translationAtRest = posAtRest;
            posConstraint.constraintActive = true;
            var rotConstraint = marker.GetComponent<RotationConstraint>();
            rotConstraint.AddSource(sauce);
            rotConstraint.constraintActive = true;
            var meshRenderer = marker.GetComponent<MeshRenderer>();
            meshRenderer.material = material;
            meshRenderer.receiveShadows = false;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        private void OnDestroy()
        {
            Destroy(marker);
        }

        private void OnDisable()
        {
            if (marker is null) return;
            marker.SetActive(false);
        }

        private void OnEnable()
        {
            if (marker is null) return;
            marker.SetActive(true);
        }

    }
}