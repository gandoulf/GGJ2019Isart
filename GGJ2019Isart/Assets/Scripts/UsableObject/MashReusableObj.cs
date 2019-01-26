using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashReusableObj : MashBreakableObj
{
    [SerializeField]
    protected int pointDivider = 2;

    public override void OnButtonPressed(ButtonType type, GameObject player)
    {
        if (type == ButtonType.ACTION && bIsUseable == true)
        {
            soundEmitter[Random.Range(0, soundEmitter.Length)].EmitSound();
            mashedTime++;
            if (mashedTime > numberOfMashNeeded)
            {
                GivePoint();
                mashedTime = 0;
            }
        }
    }

    protected override void GivePoint()
    {
        base.GivePoint();
        pointWon /= pointDivider;
    }
}
