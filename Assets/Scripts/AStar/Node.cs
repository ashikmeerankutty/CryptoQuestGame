using System;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector2Int gridPosition;
    public int gCost = 0; // distance from starting node
    public int hCost = 0; // distance from finishing node
    public Node parentNode;

    public Node(Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;

        parentNode = null;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        // compare will be <0 if this instance Fcost is less than nodeToCompare.FCost
        // compare will be >0 if this instance Fcost is greater than nodeToCompare.FCost
        // compare will be ==0 if the values are the same

        int compare = FCost.CompareTo(nodeToCompare.FCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return compare;
    }
}
