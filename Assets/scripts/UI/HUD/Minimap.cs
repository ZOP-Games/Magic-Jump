using UnityEngine;

namespace GameExtensions.UI.HUD
{
    public class Minimap : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            Player.PlayerReady += () =>
            {
                var tf = transform;
                tf.parent = Player.Instance.transform;
                tf.localPosition = new Vector3(0, tf.localPosition.y, 0);
            };
        }
    }
}