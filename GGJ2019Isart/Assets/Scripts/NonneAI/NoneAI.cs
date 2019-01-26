using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneAI : ANoneAI
{
    public override void SoundTriggerEvent(GameObject other)
    {
        base.SoundTriggerEvent(other);
    }

    public override void ViewTriggerEvent(GameObject other)
    {
        base.ViewTriggerEvent(other);
    }

    public override void ViewExitEvent(GameObject other)
    {
        base.ViewExitEvent(other);
    }
}
