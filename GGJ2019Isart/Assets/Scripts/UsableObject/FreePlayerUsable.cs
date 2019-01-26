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
		this.GetComponent<MainCharacter>().IsCaptured = false;
		StopCoroutine(playSoundCoroutine);
		yield return null;
	}
}
