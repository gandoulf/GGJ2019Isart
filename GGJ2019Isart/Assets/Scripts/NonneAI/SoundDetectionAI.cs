using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetectionAI : ADetectionAI
{
    private void OnTriggerEnter(Collider other)
    {
        collisionEvent.Invoke(other.gameObject);
    }
}
