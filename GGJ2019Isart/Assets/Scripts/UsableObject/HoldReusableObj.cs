using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldReusableObj : HoldBreakableObj
{
    [SerializeField]
    protected int pointDivider = 2;

    protected override IEnumerator HoldButton()
    {
        yield return new WaitForSeconds(holdTime);
        bIsUseable = true;
        bInUse = false;
        GivePoint();
        pointWon /= pointDivider;
        StopCoroutine(playSoundCoroutine);
        yield return null;
    }

	protected override void GivePoint()
	{
		GameManagerSingleton.Instance.IncScore(this.pointWon);
	}
}
