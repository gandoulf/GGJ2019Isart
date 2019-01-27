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

    public override void OnButtonPressed(GameObject player, MainController.eInputType buttonTypePressed)
    {
        if (buttonTypePressed == MainController.eInputType.ACTION && bIsUseable == true)
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
				player.GetComponent<MainController>().CleanDestroyedObject(this);
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
		DestroyImmediate(this.transform.parent.gameObject);
	}
}
