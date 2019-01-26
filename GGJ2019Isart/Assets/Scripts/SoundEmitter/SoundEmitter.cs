using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class SoundEmitter : ASoundEmitter
{
    //TO DELETE
    public bool activate = false;

    private SphereCollider sphereCollider;
    private float maxRange;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float soundIntensity;

    //TO DELETE
    private void Update()
    {
        if (activate)
        {
            EmitSound();
            activate = false;
        }
    }

    protected override void Start()
    {
        base.Start();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        maxRange = sphereCollider.radius;
    }

    public void EmitSound()
    {
        sphereCollider.enabled = true;
        sphereCollider.radius = maxRange;
        ComputeSoundWeight(soundIntensity);
        Invoke("StopEmitting", 0.1f);
    }

    private void StopEmitting()
    {
        sphereCollider.enabled = false;
        sphereCollider.radius = 0.0f;
    }
}
