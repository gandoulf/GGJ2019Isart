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

    public override bool IsUseable { get { return bInUse ? false : bIsUseable ? true : false; } }

    public override void OnButtonPressed(ButtonType type, GameObject player)
    {
        if (type == ButtonType.ACTION && IsUseable)
        {
            bInUse = true;
            holdingCoroutine = StartCoroutine(HoldButton());
            playSoundCoroutine = StartCoroutine(playSound());
        }
    }

    public override void OnButtonReleased(ButtonType type)
    {
        if (type == ButtonType.ACTION && bInUse == true)
        {
            bInUse = false;
            StopCoroutine(holdingCoroutine);
            StopCoroutine(playSoundCoroutine);
        }
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
        StopCoroutine(playSoundCoroutine);
        yield return null;
    }

    protected virtual void GivePoint()
    {

    }
}
