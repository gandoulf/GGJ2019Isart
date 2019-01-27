using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObj : AUsable
{
    public override void OnButtonPressed(GameObject player, MainController.eInputType buttonTypePressed)
    {
        if (buttonTypePressed == MainController.eInputType.ACTION && IsUseable)
        {
            player.GetComponent<MainController>().IsThrowingObjAvailable = true;
            player.GetComponent<MainController>().OnObjectNearExit(this);
            Destroy(this.gameObject);
        }
    }
}
