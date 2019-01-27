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

    public override void OnButtonPressed(GameObject player, MainController.eInputType buttonTypePressed)
    {
        if (buttonTypePressed == MainController.eInputType.ACTION && bIsUseable == true)
        {
            soundEmitter[Random.Range(0, soundEmitter.Length)].EmitSound();
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
		GameManagerSingleton.Instance.IncScore(this.pointWon);
		DestroyImmediate(this.transform.parent.gameObject);
	}
}
