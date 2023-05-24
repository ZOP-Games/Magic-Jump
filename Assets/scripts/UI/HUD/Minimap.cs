using System.Collections;
using System.Collections.Generic;
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
                tf.localPosition = Vector3.zero;
            };
        }
    
    }
}

