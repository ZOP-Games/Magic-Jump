using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSUpdater : MonoBehaviour
{
    private TextMeshProUGUI fpsText;
    private void Start()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
        InvokeRepeating(nameof(ShowFPS),0,1);
    }
    //show FPS so we can see it in builds
    private void ShowFPS()
    {
        fpsText.SetText("FPS: " + 1 / Time.deltaTime); //FPS = 1 / frametime
    }
}