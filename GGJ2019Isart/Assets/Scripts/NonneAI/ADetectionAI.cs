using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnCollision : UnityEvent<GameObject>
{
}

[RequireComponent(typeof(SphereCollider))]
public class ADetectionAI : MonoBehaviour
{
    [SerializeField]
    protected OnCollision collisionEvent;

    protected SphereCollider sphereCollider;
    public float radius { get { return sphereCollider.radius; } }
    private float BaseRadius;

    protected virtual void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        BaseRadius = sphereCollider.radius;
    }

    public virtual void SetRange(float range)
    {
        sphereCollider.radius = range;
    }

    public virtual void ResetCollider()
    {
        sphereCollider.enabled = false;
        sphereCollider.radius = 0;
        Invoke("SetBackValue", 0.1f);
    }

    protected virtual void SetBackValue()
    {
        sphereCollider.enabled = true;
        sphereCollider.radius = BaseRadius;
    }
}
