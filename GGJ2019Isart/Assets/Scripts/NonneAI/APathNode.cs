using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class APathNode : MonoBehaviour
{
    [SerializeField]
    private APathNode[] pathNodes;
    [SerializeField]
    private ASoundEmitter soundEmitter;
    public ASoundEmitter SoundEmitter { get { return soundEmitter; } }

    private SphereCollider sphereCollider;

    protected virtual void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public virtual GameObject getMostInterestingPath(GameObject previousNode)
    {
        APathNode returnNode = null;
        foreach (var node in pathNodes)
        {
            if ((returnNode == null) || 
                (node.soundEmitter.SoundWeight == returnNode.soundEmitter.SoundWeight && returnNode.gameObject == previousNode) ||
                (node.soundEmitter.SoundWeight > returnNode.soundEmitter.SoundWeight))
            {
                returnNode = node;
            }
        }
        return returnNode.gameObject;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SoundEmission")
        {
            float objectDistance = Vector3.Distance(transform.position, other.gameObject.transform.position);
            float maxDistance = sphereCollider.radius + other.gameObject.GetComponent<SphereCollider>().radius;
            //Debug.Log("objectDistance = " + objectDistance + " maxDistance = " + maxDistance + " result = " + objectDistance / maxDistance);

            soundEmitter.ComputeSoundWeight(other.GetComponent<ASoundEmitter>().SoundWeight * (1 - (objectDistance / maxDistance)));
        }
        if (other.gameObject.tag == "Nonne")
        {
            soundEmitter.ResetSoundWeight();
        }
    }
}
