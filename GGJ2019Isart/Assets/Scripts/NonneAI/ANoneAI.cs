using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ANoneAI : MonoBehaviour
{
    [SerializeField]
    private Room room;

    private GameObject destination;
    private GameObject previousDestination;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        FindANewPath();
    }

    protected virtual void FindANewPath()
    {
        if (destination)
        {
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && Mathf.Approximately(agent.remainingDistance, 0))
            {
                APathNode pathNode = destination.GetComponent<APathNode>();
                if (pathNode)
                {
                    destination = pathNode.getMostInterestingPath(previousDestination);
                    previousDestination = pathNode.gameObject;
                    agent.SetDestination(destination.transform.position);
                }
                else
                {
                    ResetDestination();
                }
            }
        }
        else
        {
            destination = room.GetClosestPathNode(gameObject).gameObject;
            previousDestination = null;
            agent.SetDestination(destination.transform.position);
        }
    }

    protected virtual void ResetDestination()
    {
        destination = null;
        previousDestination = null;
    }
}
