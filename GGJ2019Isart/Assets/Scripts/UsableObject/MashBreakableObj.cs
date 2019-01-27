using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashBreakableObj : AUsable
{
    [SerializeField]
    protected SoundEmitter[] soundEmitter;
	[SerializeField] protected bool playSoundFirstTime;
    [SerializeField]
    protected int numberOfMashNeeded;
    [SerializeField]
    protected int pointWon;
	[SerializeField]
	protected SoundEmitter endSound;

	protected int mashedTime;
	protected bool alreadyPlayed;

    public override void OnButtonPressed(ButtonType type, GameObject player)
    {
        if (type == ButtonType.ACTION && bIsUseable == true)
        {
			if (this.playSoundFirstTime == true && this.alreadyPlayed == false)
			{
				this.alreadyPlayed = true;
				this.soundEmitter[0].EmitSound();
			}
			else if (this.playSoundFirstTime == false)
			{
				soundEmitter[Random.Range(0, soundEmitter.Length)].EmitSound();
			}
            mashedTime++;
            if (mashedTime > numberOfMashNeeded)
            {
                bIsUseable = false;
                GivePoint();
            }
        }
    }

    protected virtual void GivePoint()
    {
		if (this.endSound != null)
		{
			this.endSound.EmitSound();
		}
		GameManagerSingleton.Instance.IncScore(this.pointWon);
    }
}
