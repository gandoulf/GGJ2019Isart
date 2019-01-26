using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public APathNode[] pathNode;

    public APathNode GetClosestPathNode(GameObject reference)
    {
        APathNode closestNode = null;

        foreach (var node in pathNode)
        {
            if (closestNode == null)
            {
                closestNode = node;
            }
            else if (Vector3.Distance(node.gameObject.transform.position, reference.transform.position) < Vector3.Distance(closestNode.gameObject.transform.position, reference.transform.position))
            {
                closestNode = node;
            }
        }
        return closestNode;
    }
}
