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

    public Alien alien;

    private Node[,] nodeArray;

    private float nodeDiameter;
    private int gridSizeX;
    private int gridSizeY;

    private int minX;
    private int minY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;

        Tilemap[] tileMaps = gridWorld.GetComponentsInChildren<Tilemap>();
        int maxX = 0;
        int maxY = 0;
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
        gridSizeX = Mathf.RoundToInt((maxX - minX) / nodeDiameter);
        gridSizeY = Mathf.RoundToInt((maxY - minY) / nodeDiameter);

        Debug.Log(gridSizeX * gridSizeY + "Nodes required");
        CreateGrid();
    }

    private void CreateGrid()
    {
        nodeArray = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = new Vector3(minX, minY, 0);

        int i = 0;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool wall = false;

                if (Physics2D.OverlapCircle(worldPoint, nodeRadius, collisionMask))
                {
                    wall = true;
                }

                nodeArray[x, y] = new Node(wall, worldPoint, x, y);
                i++;
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

        //Check Up Side
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
        float xPos = ((worldPosition.x + gridSizeX / 2) / gridSizeX);
        float yPos = ((worldPosition.y + gridSizeY / 2) / gridSizeY);
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

                if (alien.finalPath != null)//If the final path is not empty
                {
                    if (alien.finalPath.Contains(n))//If the current node is in the final path
                    {
                        Gizmos.color = Color.red;//Set the color of that node
                    }

                }

                Gizmos.DrawCube(n.GetNodePosition(), Vector3.one * (nodeDiameter - distanceBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }

}
