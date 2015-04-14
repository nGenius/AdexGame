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


    public delegate void MouseLButtonDownEvent(RaycastHit rayCastHit);
    public event MouseLButtonDownEvent mouseLButtonEvent;



    // Use this for initialization
    private void Start()
    {
        CreateTestTiles();
        selectedTacticePlayer.currentNode = nodes[0, 0];
        mouseLButtonEvent += selectedTacticePlayer.MouseLButtonEvent;
        mouseLButtonEvent += MouseLButtonEventProc;
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
                if (mouseLButtonEvent != null)
                {
                    mouseLButtonEvent(rayCastHit);
                }
            }
        }
    }

    private void MouseLButtonEventProc(RaycastHit rayCastHit)
    {
        if (rayCastHit.collider)
        {
            if (rayCastHit.transform.tag == "Player")
            {
                TacticePlayer tacticePlayer = rayCastHit.transform.GetComponent<TacticePlayer>();
                moveRange.ShowMoveRange(tacticePlayer.moveRange);
                selectedTacticePlayer = tacticePlayer;   
                MessageDispatcher.Instance().DispatchMessage(0.0f, null, tacticePlayer.GetAIAgent(), AIMsgType.MSG_SELECTED);
            }
            else if (rayCastHit.transform.tag == "MoveRange")
            {
                selectedTacticePlayer.MoveToCell(rayCastHit.transform);
            }
            else if (rayCastHit.transform.tag == "PathNode")
            {
                if (selectedTacticePlayer != null && selectedTacticePlayer.Movabable())
                {
                    MapTile mapTile = rayCastHit.transform.GetComponent<MapTile>();
                    float distance = pathFinding.distanceToGoal(selectedTacticePlayer.currentNode, mapTile.pathNode);
                    if (distance <= selectedTacticePlayer.moveRange)
                    {
                        Stack<PathNode> paths = pathFinding.FindPath(nodes, selectedTacticePlayer.currentNode, mapTile.pathNode);
                        selectedTacticePlayer.MoveToPaths(paths);
                        moveRange.HideMoveRange();
                    }
                }
            }
        }
    }
}
    