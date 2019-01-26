using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StudioEventEmitter))]
public class SoundEmitter : ASoundEmitter
{
    private SphereCollider sphereCollider;
    private float maxRange;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float soundIntensity;

    protected StudioEventEmitter soundEmitter;

    protected override void Start()
    {
        base.Start();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
        sphereCollider.isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
        soundEmitter = GetComponent<StudioEventEmitter>();
        maxRange = sphereCollider.radius;
    }

    protected override void OnSoundOver()
    {
        base.OnSoundOver();
        if (soundEmitter)
        {
            soundEmitter.Stop();
        }
    }

    public void EmitSound()
    {
        sphereCollider.enabled = true;
        sphereCollider.radius = maxRange;
        ComputeSoundWeight(soundIntensity);
        if (soundEmitter)
        {
            soundEmitter.Play();
        }
        Invoke("StopEmitting", 0.1f);
    }

    private void StopEmitting()
    {
        sphereCollider.enabled = false;
        sphereCollider.radius = 0.0f;
    }
}
