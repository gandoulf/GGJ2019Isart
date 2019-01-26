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


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        directionPoint.transform.parent = null;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (bIsChasing == false)
            FindANewPath();
        else
            KeepChasing();

    }

    protected virtual void KeepChasing()
    {
        if (destination.transform.position != directionPoint.transform.position)
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
    }

    protected virtual void ResetDestination()
    {
        bIsChasing = false;
        destination = null;
        previousDestination = null;
        attractionForce = 0;
        ViewDetection.ResetCollider();
        SoundDetection.ResetCollider();
    }

    public virtual void SoundTriggerEvent(GameObject other)
    {
        if (other.tag == "SoundEmission" && bIsChasing == false)
        {
            float objectDistance = Vector3.Distance(transform.position, other.transform.position);
            float maxDistance = SoundDetection.radius + other.GetComponent<SphereCollider>().radius;
            Debug.Log("objectDistance = " + objectDistance + " maxDistance = " + maxDistance + " result = " + objectDistance / maxDistance);

            ASoundEmitter tmp = other.gameObject.GetComponent<ASoundEmitter>();
            if (tmp != null && tmp.SoundWeight * (1 - (objectDistance / maxDistance)) > attractionForce)
            {
                changeDestination(tmp, tmp.SoundWeight * (1 - (objectDistance / maxDistance)));
            }
        }
    }

    public virtual void ViewTriggerEvent(GameObject other)
    {
        if (other.tag == "Player" && bIsChasing == false)
        {
            Debug.Log("kidssssss");
            ResetDestination();
            bIsChasing = true;
            destination = other;
            attractionForce = 1;
            directionPoint.transform.position = other.transform.position;
            agent.SetDestination(destination.transform.position);
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
}
