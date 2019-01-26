using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewDetectionAI : ADetectionAI
{
    [SerializeField] OnCollision exitEvent;

    private void OnTriggerEnter(Collider other)
    {
        collisionEvent.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        exitEvent.Invoke(other.gameObject);
    }
}
