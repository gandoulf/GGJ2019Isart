using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUsable : MonoBehaviour
{
    public enum ButtonType
    {
        ACTION,
        SPECIAL
    }

    [SerializeField] protected bool bIsUseable;
    public virtual bool IsUseable { get { return bIsUseable; } }

    public virtual void OnButtonPressed(ButtonType type, GameObject player)
    {

    }

    public virtual void OnButtonReleased(ButtonType type)
    {

    }
}
