using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOutObj : AUsable
{
    [SerializeField]
    private SoundEmitter soundEmitter;

    protected GameObject hidedPlayer;
    protected bool bInUse = false;
    public override bool IsUseable { get { return bInUse ? false : bIsUseable ? true : false; } }

    public override void OnButtonPressed(GameObject player, MainController.eInputType buttonTypePressed)
    {
        if (buttonTypePressed == MainController.eInputType.SPECIAL)
        {
            if (IsUseable)
            {
                hidedPlayer = player;
                bInUse = true;
                soundEmitter.EmitSound();
				//make a call on the player to hide him
				player.GetComponent<MainCharacter>().HideCharacter(this, true);
            }
            else if (player == hidedPlayer)
            {
                hidedPlayer = null;
                bInUse = false;
                soundEmitter.EmitSound();
				//make a call on the player to unhide him
				player.GetComponent<MainCharacter>().HideCharacter(this, false);
			}
        }
        else if (buttonTypePressed == MainController.eInputType.ACTION)
        {
            soundEmitter.EmitSound();
        }
    }
}
