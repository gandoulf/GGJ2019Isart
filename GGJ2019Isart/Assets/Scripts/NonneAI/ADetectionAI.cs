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
    public float radius { get { return BaseRadius / 2.0f; } }
    private float BaseRadius;

    protected virtual void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        BaseRadius = gameObject.transform.localScale.x;
    }

    public virtual void SetRange(float range)
    {
        sphereCollider.radius = range;
    }

    public virtual void ResetCollider()
    {
        sphereCollider.enabled = false;
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        Invoke("SetBackValue", 0.1f);
    }

    protected virtual void SetBackValue()
    {
        sphereCollider.enabled = true;
        gameObject.transform.localScale = new Vector3(BaseRadius, 0.05f, BaseRadius);
    }
}
