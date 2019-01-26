using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObj : AUsable
{
    public override void OnButtonPressed(ButtonType type, GameObject player)
    {
        if (type == ButtonType.ACTION && IsUseable)
        {
            player.GetComponent<MainController>().IsThrowingObjAvailable = true;
            player.GetComponent<MainController>().OnObjectNearExit(this);
            Destroy(this.gameObject);
        }
    }
}
