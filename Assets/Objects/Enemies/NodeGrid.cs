using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeGrid : MonoBehaviour
{

    [SerializeField] private float nodeRadius = 0.5f;
    [SerializeField] private float distanceBetweenNodes;
    [SerializeField] private Grid gridWorld;
    [SerializeField] private LayerMask collisionMask;

    [SerializeField] private List<Alien> Aliens = new List<Alien>();
    [SerializeField] private int alien = 0;

    private Node[,] nodeArray;

    private float nodeDiameter;
    private int gridSizeX;
    private int gridSizeY;
    private int gridWorldSizeX;
    private int gridWorldSizeY;

    private bool ready = false;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;

        Tilemap[] tileMaps = gridWorld.GetComponentsInChildren<Tilemap>();
        int maxX = 0;
        int maxY = 0;
        int minX = 0;
        int minY = 0;
        foreach (Tilemap map in tileMaps)
        {
            if (map.CellToWorld(map.cellBounds.min).x < minX)
            {
                minX = (int)map.CellToWorld(map.cellBounds.min).x;
            }
            if (map.CellToWorld(map.cellBounds.min).y < minY)
            {
                minY = (int)map.CellToWorld(map.cellBounds.min).y;
            }
            if (map.CellToWorld(map.cellBounds.max).x > maxX)
            {
                maxX = (int)map.CellToWorld(map.cellBounds.max).x;
            }
            if (map.CellToWorld(map.cellBounds.max).y > maxY)
            {
                maxY = (int)map.CellToWorld(map.cellBounds.max).y;
            }
            
        }
        gridWorldSizeX = maxX - minX;
        gridWorldSizeY = maxY - minY;
        gridSizeX = Mathf.RoundToInt(gridWorldSizeX / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSizeY / nodeDiameter);

        Debug.Log(gridSizeX * gridSizeY + " Nodes required");
        CreateGrid();
        ready = true;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Entity"))
        {
            Aliens.Add(g.GetComponent<Alien>());
        }
    }

    void OnValidate()
    {
        alien = Mathf.Clamp(alien, 0, Aliens.Count);
    }
    private void CreateGrid()
    {
        nodeArray = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSizeX / 2 - Vector3.up * gridWorldSizeY / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool wall = true;

                if (Physics2D.OverlapCircle(worldPoint, nodeRadius, collisionMask))
                {
                    wall = false;
                }

                nodeArray[x, y] = new Node(wall, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node neighborNode)
    {
        List<Node> neighborList = new List<Node>();

        //Check Right Side
        int checkX = neighborNode.GetGridX() + 1;
        int checkY = neighborNode.GetGridY();
        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighborList.Add(nodeArray[checkX, checkY]);
            }
        }

        //Check Right Top Side
        checkX = neighborNode.GetGridX() + 1;
        checkY = neighborNode.GetGridY() + 1;
        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighborList.Add(nodeArray[checkX, checkY]);
            }
        }

        //Check Right Bottom Side
        checkX = neighborNode.GetGridX() + 1;
        checkY = neighborNode.GetGridY() - 1;
        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighborList.Add(nodeArray[checkX, checkY]);
            }
        }

        //Check Left Side
        checkX = neighborNode.GetGridX() - 1;
        checkY = neighborNode.GetGridY();
        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighborList.Add(nodeArray[checkX, checkY]);
            }
        }

        //Check Left Top Side
        checkX = neighborNode.GetGridX() - 1;
        checkY = neighborNode.GetGridY() + 1;
        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighborList.Add(nodeArray[checkX, checkY]);
            }
        }

        //Check Left Bottom Side
        checkX = neighborNode.GetGridX() - 1;
        checkY = neighborNode.GetGridY() - 1;
        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighborList.Add(nodeArray[checkX, checkY]);
            }
        }

        //Check Top Side
        checkX = neighborNode.GetGridX();
        checkY = neighborNode.GetGridY() + 1;
        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighborList.Add(nodeArray[checkX, checkY]);
            }
        }

        //Check Bottom Side
        checkX = neighborNode.GetGridX();
        checkY = neighborNode.GetGridY() - 1;
        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighborList.Add(nodeArray[checkX, checkY]);
            }
        }

        return neighborList;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float xPos = ((worldPosition.x + gridWorldSizeX / 2) / gridWorldSizeX);
        float yPos = ((worldPosition.y + gridWorldSizeY / 2) / gridWorldSizeY);
        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);
        int x = Mathf.RoundToInt((gridSizeX - 1) * xPos);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPos);
        return nodeArray[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, gridSizeY, 1));//Draw a wire cube with the given dimensions from the Unity inspector

        if (nodeArray != null)//If the grid is not empty
        {
            foreach (Node n in nodeArray)//Loop through every node in the grid
            {
                if (n.GetIsWall())//If the current node is a wall node
                {
                    Gizmos.color = Color.yellow;//Set the color of the node
                }
                else
                {
                    Gizmos.color = Color.white;//Set the color of the node
                }

                if (Aliens != null)//If the final path is not empty
                {
                    if (Aliens[alien].GetCurrentPath().Contains(n))//If the current node is in the final path
                    {
                        Gizmos.color = Color.red;//Set the color of that node
                    }

                }
                Gizmos.DrawCube(n.GetNodePosition(), Vector3.one * (nodeDiameter - distanceBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }


    public bool GetReady()
    {
        return ready;
    }

}
