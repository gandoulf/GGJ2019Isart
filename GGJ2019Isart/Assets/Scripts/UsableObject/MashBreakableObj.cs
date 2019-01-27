using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashBreakableObj : AUsable
{
    [SerializeField]
    protected SoundEmitter[] soundEmitter;

    [SerializeField]
    protected int numberOfMashNeeded;
    [SerializeField]
    protected int pointWon;

    protected int mashedTime;

    public override void OnButtonPressed(ButtonType type, GameObject player)
    {
        if (type == ButtonType.ACTION && bIsUseable == true)
        {
            soundEmitter[Random.Range(0, soundEmitter.Length)].EmitSound();
            mashedTime++;
            if (mashedTime > numberOfMashNeeded)
            {
                bIsUseable = false;
                GivePoint();
            }
        }
    }

    protected virtual void GivePoint()
    {d
		GameManagerSingleton.Instance.IncScore(this.pointWon);
    }
}
