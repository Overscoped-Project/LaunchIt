using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private int gridX;
    private int gridY;

    private bool isWall;
    private Vector3 nodePosition;
    private Node parentNode;

    private int gCost;
    private int hCost;

    public Node(bool isWall, Vector3 nodePosition, int gridX, int gridY)
    {
        this.isWall = isWall;
        this.nodePosition = nodePosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int GetGridX()
    {
        return gridX;
    }
    public int GetGridY()
    {
        return gridY;
    }
    public bool GetIsWall()
    {
        return isWall;
    }
    public Vector3 GetNodePosition()
    {
        return nodePosition;
    }
    public Node GetParentNode()
    {
        return parentNode;
    }
    public int GetGCost()
    {
        return gCost;
    }
    public void SetGCost(int gCost)
    {
         this.gCost = gCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    public int GetFCost()
    {
        return gCost + hCost;
    }
    public void SetParentNode(Node parentNode)
    {
        this.parentNode = parentNode;
    }
}
