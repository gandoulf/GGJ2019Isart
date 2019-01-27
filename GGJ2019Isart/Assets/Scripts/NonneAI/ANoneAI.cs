using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ANoneAI : MonoBehaviour
{
    [SerializeField]
    private Room room;
    [SerializeField]
    private GameObject directionPoint;

    //navigation
    private GameObject destination;
    private GameObject previousDestination;
    private NavMeshAgent agent;

    //gonna make the nonne choose if she change her target
    private float attractionForce;

    //detection of the nonne
    [SerializeField]
    private ADetectionAI SoundDetection;
    [SerializeField]
    private ADetectionAI ViewDetection;

    //chasing player
    private bool bIsChasing = false;
    private bool IsBreakTime = false;

    [SerializeField]
    private float breakTime;
    private float actualTime = 0;

	private Animator animator;
	private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        directionPoint.transform.parent = null;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (IsBreakTime)
            Waiting();
        else
        {
            if (bIsChasing == false)
                FindANewPath();
            else
                KeepChasing();
        }
		SetRendererOrientation();

	}

    protected virtual void Waiting()
    {
        if (actualTime < breakTime)
            actualTime += Time.deltaTime;
        else
        {
            actualTime = 0;
            IsBreakTime = false;
		}
    }

    protected virtual void KeepChasing()
    {
        if (destination.GetComponent<MainCharacter>().IsCaptured)
        {
            ResetDestination();
            IsBreakTime = true;
			animator.SetBool("IsWalking", false);
		}
        else if (destination.transform.position != directionPoint.transform.position)
        {
            agent.SetDestination(destination.transform.position);
            directionPoint.transform.position = destination.transform.position;
        }
    }

    protected virtual void FindANewPath()
    {
        if (destination)
        {
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity && Mathf.Approximately(agent.remainingDistance, 0))
            {
                APathNode pathNode = destination.GetComponent<APathNode>();
                if (pathNode)
                {
                    destination = pathNode.getMostInterestingPath(previousDestination);
                    previousDestination = pathNode.gameObject;
                    agent.SetDestination(destination.transform.position);
                    attractionForce = destination.GetComponent<APathNode>().SoundEmitter.SoundWeight;
                }
                else
                {
                    ResetDestination();
                }
            }
        }
        if (destination == null)
        {
            destination = room.GetClosestPathNode(gameObject).gameObject;
            previousDestination = null;
            agent.SetDestination(destination.transform.position);
            attractionForce = destination.GetComponent<APathNode>().SoundEmitter.SoundWeight;
		}
		animator.SetBool("IsWalking", true);
	}

    public virtual void NonneToClose()
    {
        ResetDestination();
    }

    protected virtual void ResetDestination()
    {
        bIsChasing = false;
        destination = null;
        previousDestination = null;
        attractionForce = 0;
        ViewDetection.ResetCollider();
        SoundDetection.ResetCollider();
		animator.SetBool("IsRunning", false);
	}

    public virtual void SoundTriggerEvent(GameObject other)
    {
        if (other.tag == "SoundEmission" && bIsChasing == false)
        {
            float objectDistance = Vector3.Distance(transform.position, other.transform.position);
            float maxDistance = SoundDetection.radius + other.GetComponent<SphereCollider>().radius;
            ASoundEmitter tmp = other.gameObject.GetComponent<ASoundEmitter>();
            Debug.Log("tmp.SoundWeight = " + tmp.SoundWeight + " objectDistance = " + objectDistance + " maxDistance = " + maxDistance + " result = " + tmp.SoundWeight * (1 - (objectDistance / maxDistance)));

           if (tmp != null && tmp.SoundWeight * (1 - (objectDistance / maxDistance)) > attractionForce)
            {
                changeDestination(tmp, tmp.SoundWeight * (1 - (objectDistance / maxDistance)));
            }
        }
    }

    public virtual void ViewTriggerEvent(GameObject other)
    {
        if (other.tag == "Player" && bIsChasing == false && !other.GetComponent<MainCharacter>().IsCaptured)
        {
            Debug.Log("kidssssss");
            ResetDestination();
            bIsChasing = true;
            destination = other;
            attractionForce = 1;
            directionPoint.transform.position = other.transform.position;
            agent.SetDestination(destination.transform.position);
			animator.SetBool("IsRunning", true);
		}
    }

    public virtual void ViewExitEvent(GameObject other)
    {
        if (other.tag == "Player" && other == destination)
        {
            Debug.Log("noooooo he left");
            ResetDestination();
        }
    }

    protected void changeDestination(ASoundEmitter dest, float newAttractionForce)
    {
        directionPoint.transform.position = dest.gameObject.transform.position;
        attractionForce = newAttractionForce;
        destination = directionPoint;
        agent.SetDestination(destination.transform.position);
    }

	protected void SetRendererOrientation()
	{
		spriteRenderer.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

		animator.SetBool("IsFront", true);
		this.spriteRenderer.flipX = false;
		float angle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.up);
		if (angle >= -45.0f && angle <= 45.0f)
		{
			animator.SetBool("IsFront", false);
		}
		if (angle < -45.0f && angle >= -135.0f)
		{
			this.spriteRenderer.flipX = true;
		}
	}
}
