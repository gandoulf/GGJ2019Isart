using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPlayer : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<MainCharacter>().IsCaptured = true;
        }
        if (other.gameObject.tag == "Nonne")
        {
            other.gameObject.GetComponent<ANoneAI>().NonneToClose();
        }
    }
}
