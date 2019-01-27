using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBreakableObj : AUsable
{
    [SerializeField]
    private SoundEmitter soundEmitter;

    [SerializeField]
    private int pointWon;

    public override void OnButtonPressed(GameObject player, MainController.eInputType buttonTypePressed)
    {
        if (buttonTypePressed == MainController.eInputType.ACTION && bIsUseable == true)
        {
            bIsUseable = false;
            soundEmitter.EmitSound();
            GivePoint();
			player.GetComponent<MainController>().CleanDestroyedObject(this);
		}
    }

    protected virtual void GivePoint()
    {
		DestroyImmediate(this.transform.parent.gameObject);
    }
}
