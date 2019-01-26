using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBreakableObj : AUsable
{
    [SerializeField]
    private SoundEmitter soundEmitter;

    [SerializeField]
    private int pointWon;

    public override void OnButtonPressed(ButtonType type, GameObject player)
    {
        if (type == ButtonType.ACTION && bIsUseable == true)
        {
            bIsUseable = false;
            soundEmitter.EmitSound();
            GivePoint();
        }
    }

    protected virtual void GivePoint()
    {

    }
}
