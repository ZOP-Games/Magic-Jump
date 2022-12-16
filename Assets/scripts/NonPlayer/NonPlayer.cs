using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameExtensions.UI;
using UnityEngine;

/// <summary>
/// An object reprsenting an NPC (Non-Player Character).
/// </summary>
public class NonPlayer : MonoBehaviour
{
    /// <summary>
    /// The name of the character.
    /// </summary>
    public string characterName;
    /// <summary>
    /// The first <see cref="InGameDialogBox"/> the character will say when interacted with.
    /// </summary>
    private InGameDialogBox firstDialogBox;

    /// <summary>
    /// Starts an interaction with the NPC and opens the first dialog box.
    /// </summary>
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
