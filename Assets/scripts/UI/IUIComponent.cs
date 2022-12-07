using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Base class for UI components (text boxes, menus, etc.)
/// </summary>
public interface IUIComponent
{
    /// <summary>
    /// The UI component's common background image.
    /// </summary>
    Image BackgroundImage { get; }
    /// <summary>
    /// The UI component's <see cref="RectTransform"/>.
    /// </summary>
    RectTransform Transform { get; }
}
