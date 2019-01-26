using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ANoneAI : MonoBehaviour
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

    //vision of the nonne
    private SphereCollider detectionSphere;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        directionPoint.transform.parent = null;
        detectionSphere = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        FindANewPath();
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
        destination = null;
        previousDestination = null;
        attractionForce = 0;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SoundEmission")
        {
            float objectDistance = Vector3.Distance(transform.position, other.gameObject.transform.position);
            float maxDistance = detectionSphere.radius + other.gameObject.GetComponent<SphereCollider>().radius;
            Debug.Log("objectDistance = " + objectDistance + " maxDistance = " + maxDistance + " result = " + objectDistance / maxDistance);

            ASoundEmitter tmp = other.gameObject.GetComponent<ASoundEmitter>();
            if (tmp != null && tmp.SoundWeight * (1 - (objectDistance / maxDistance)) > attractionForce)
            {
                changeDestination(tmp, tmp.SoundWeight * (1 - (objectDistance / maxDistance)));
            }
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
