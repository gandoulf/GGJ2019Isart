using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private SoundEmitter soundEmitter;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            soundEmitter.EmitSound();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("start emitting");
            Destroy(gameObject, 0.5f);
        }
    }
}
