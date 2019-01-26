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

	public virtual void OnObjectFocused(bool isFocused)
	{

	}

	public virtual void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("Player") == true)
		{
			collider.gameObject.GetComponent<MainController>().OnObjectNearEnter(this);
		}
	}

	public virtual void OnTriggerExit(Collider collider)
	{
		if (collider.CompareTag("Player") == true)
		{
			collider.gameObject.GetComponent<MainController>().OnObjectNearExit(this);
		}
	}
}
