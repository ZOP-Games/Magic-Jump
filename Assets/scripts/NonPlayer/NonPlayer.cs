using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameExtensions.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class NonPlayer : MonoBehaviour
{
    public string characterName;
    [FormerlySerializedAs("firstBox")] [SerializeField] private InGameDialogBox firstDialogBox;

    public void Interact()
    {
        firstDialogBox.Open();
    }
}
