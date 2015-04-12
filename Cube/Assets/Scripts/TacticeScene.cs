using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TacticeScene : MonoBehaviour
{
    public MoveRange moveRange;
    public TacticePlayer selectedTacticePlayer;
    
    public const int MAX_MAP_NODE_NUM = 10;
    private PathFinding pathFinding = new PathFinding();
    private PathNode[,] nodes = new PathNode[MAX_MAP_NODE_NUM, MAX_MAP_NODE_NUM];

    // Use this for initialization
    private void Start()
    {
        CreateTestTiles();

        selectedTacticePlayer.currentNode = nodes[0, 0];

        /*PathNode startNode = new PathNode();
        startNode.nodePosX = 1;
        startNode.nodePosZ = 1;

        PathNode goalNode = new PathNode();
        goalNode.nodePosX = 3;
        goalNode.nodePosZ = 3;*/

        //pathFinding.FindPath(nodes, selectedTacticePlayer.currentNode, goalNode);
    }

    public void CreateTestTiles()
    {
        Object obj = Resources.Load("etc/MapTile");

        for (int i = 0; i < MAX_MAP_NODE_NUM; ++i)
        {
            for (int j = 0; j < MAX_MAP_NODE_NUM; ++j)
            {
                GameObject tileObj = Instantiate(obj) as GameObject;
                MapTile mapTile = tileObj.GetComponent<MapTile>();
                nodes[i, j] = mapTile.pathNode;
                nodes[i, j].nodePosX = i;
                nodes[i, j].nodePosZ = j;
                mapTile.transform.position = new Vector3(i, 0, j);
                mapTile.transform.parent = transform;
                nodes[i, j].movable = true;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayCastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayCastHit))
            {
                if (rayCastHit.collider && rayCastHit.transform.tag == "Player")
                {
                    TacticePlayer tacticePlayer = rayCastHit.transform.GetComponent<TacticePlayer>();
                    moveRange.ShowMoveRange(tacticePlayer.moveRange);
                    selectedTacticePlayer = tacticePlayer;
                }

                if (rayCastHit.collider && rayCastHit.transform.tag == "MoveRange")
                {
                    selectedTacticePlayer.MoveToCell(rayCastHit.transform);
                }

                if (rayCastHit.collider && rayCastHit.transform.tag == "PathNode")
                {
                    MapTile mapTile = rayCastHit.transform.GetComponent<MapTile>();
                    Stack<PathNode> paths = pathFinding.FindPath(nodes, selectedTacticePlayer.currentNode, mapTile.pathNode);
                    foreach (PathNode node in paths)
                    {
                        Debug.Log("node.nodePosX : " + node.nodePosX + ", node.nodePosZ  : " + node.nodePosZ);                        
                    }
                    selectedTacticePlayer.MoveToPaths(paths);
                }
            }
        }
    }
}
    ;