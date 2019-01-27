using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreePlayerUsable : HoldReusableObj
{
	protected override IEnumerator HoldButton()
	{
		yield return new WaitForSeconds(holdTime);
		bIsUseable = true;
		bInUse = false;
		this.GetComponentInParent<MainCharacter>().IsCaptured = false;
		StopCoroutine(playSoundCoroutine);
		yield return null;
	}

	public override void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject != this.transform.parent.gameObject && collider.CompareTag("Player") == true && IsUseable == true)
		{
			collider.gameObject.GetComponent<MainController>().OnObjectNearEnter(this);
		}
	}

	public override void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject != this.transform.parent.gameObject && collider.CompareTag("Player") == true)
		{
			collider.gameObject.GetComponent<MainController>().OnObjectNearExit(this);
		}
	}

	protected override void setMaterial()
	{
	}

	protected override IEnumerator playSound()
	{
		yield return null;
	}
}
