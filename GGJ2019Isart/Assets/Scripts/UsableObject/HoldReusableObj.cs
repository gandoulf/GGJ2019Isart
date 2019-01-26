using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldReusableObj : HoldBreakableObj
{
    protected override IEnumerator HoldButton()
    {
        yield return new WaitForSeconds(holdTime);
        bIsUseable = true;
        bInUse = false;
        GivePoint();
        StopCoroutine(playSoundCoroutine);
        yield return null;
    }
}
