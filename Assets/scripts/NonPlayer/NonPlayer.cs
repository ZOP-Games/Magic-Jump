using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NonPlayer : MonoBehaviour
{
    protected abstract string Speech { get; }
    protected abstract List<IUIComponent> Components { get; }
}
