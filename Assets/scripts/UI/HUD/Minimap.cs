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
            Player.PlayerReady += () => transform.parent = Player.Instance.transform;
        }
    
    }
}

