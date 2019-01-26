using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreePlayerUsable : HoldReusableObj
{
	protected override IEnumerator HoldButton()
	{
		Debug.Log("HoldButton0");
		yield return new WaitForSeconds(holdTime);
		Debug.Log("HoldButton");
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
			Debug.Log("collider");
			collider.gameObject.GetComponent<MainController>().OnObjectNearEnter(this);
		}
	}

	public override void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject != this.transform.parent.gameObject && collider.CompareTag("Player") == true)
		{
			Debug.Log("collider out");
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
