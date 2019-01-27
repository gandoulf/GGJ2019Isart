using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBreakableObj : AUsable
{
    [SerializeField]
    protected SoundEmitter[] soundEmitter;
    [SerializeField]
    protected float holdTime;
    [SerializeField]
    protected float intervalBetweenSound;

    [SerializeField]
    protected int pointWon;

    protected bool bInUse = false;
    protected Coroutine holdingCoroutine = null;
    protected Coroutine playSoundCoroutine = null;

	protected GameObject playerHoldingObject = null;

    public override bool IsUseable { get { return bInUse ? false : bIsUseable ? true : false; } }

    public override void OnButtonPressed(GameObject player, MainController.eInputType buttonTypePressed)
    {
        if (playerHoldingObject == null && buttonTypePressed == MainController.eInputType.ACTION && IsUseable)
        {
            bInUse = true;
			playerHoldingObject = player;
			holdingCoroutine = StartCoroutine(HoldButton());
            playSoundCoroutine = StartCoroutine(playSound());
        }
    }

    public override void OnButtonReleased()
    {
        if (bInUse == true)
        {
            bInUse = false;
            StopCoroutine(holdingCoroutine);
            StopCoroutine(playSoundCoroutine);
        }
		playerHoldingObject = null;

	}

    protected virtual IEnumerator playSound()
    {
        while(true)
        {
            soundEmitter[Random.Range(0, soundEmitter.Length)].EmitSound();
            yield return new WaitForSeconds(intervalBetweenSound);
        }
    }

    protected virtual IEnumerator HoldButton()
    {
        yield return new WaitForSeconds(holdTime);
        bIsUseable = false;
        bInUse = false;
        GivePoint();
		playerHoldingObject.GetComponent<MainController>().CleanDestroyedObject(this);
		StopCoroutine(playSoundCoroutine);
        yield return null;
    }

    protected virtual void GivePoint()
	{
		GameManagerSingleton.Instance.IncScore(this.pointWon);
		DestroyImmediate(this.transform.parent.gameObject);
	}
}
