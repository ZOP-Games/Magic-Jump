using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameExtensions.UI;
using UnityEngine;
using Object = UnityEngine.Object;

public class NonPlayer : MonoBehaviour
{
    public string characterName;
    private InGameDialogBox firstDialogBox;

    public void Interact()
    {
        firstDialogBox.Open();
    }

    private void Start()
    {

        firstDialogBox = GetComponentInChildren<InGameDialogBox>(true);
        Debug.Log("children : " + GetComponentsInChildren<InGameDialogBox>().Length);
    }
}
