using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using System.Collections;

public class PathNode
{
    public bool movable;
    public int nodePosX;
    public int nodePosY;

    //실제 위치
    public Vector3 position;
    public PathNode parentNode;
    public float gValue;

    public bool Equals(PathNode obj)
    {
        return nodePosX == obj.nodePosX && nodePosY == obj.nodePosY;
    }
}


//tactice용 A* path finding(대각선 이동이 없다. 즉 네방향 검사)
//F = G + H
//G - 시작점 A부터 새로운 사각형까지의 이동비용(직각이면 10. 대각선은 1.4-루트2)
//H - 얻어진 사각형으로부터 최종목적지점까지의 이동비용. 가로세로 거리
public class PathFinding
{

    private const int MAX_MAP_NODE_NUM = 10;

    private PathNode[,] nodes = new PathNode[MAX_MAP_NODE_NUM, MAX_MAP_NODE_NUM];

    private Stack<PathNode> findPaths = new Stack<PathNode>();
    private List<PathNode> openList = new List<PathNode>();
    private List<PathNode> closeList = new List<PathNode>();
    private PathNode goalNode = new PathNode();
    
    //private PathNode current

    public void CreateTestNodes()
    {
        for (int i = 0; i < MAX_MAP_NODE_NUM; ++i)
        {
            for (int j = 0; j < MAX_MAP_NODE_NUM; ++j)
            {
                nodes[i, j] = new PathNode();
                nodes[i, j].nodePosX = i;
                nodes[i, j].nodePosY = j;
                nodes[i, j].movable = true;
            }
        }
    }
    //찾으면 true 못찾으면 false
    public bool FindPath(PathNode startNode, PathNode goalNode)
    {
        this.goalNode = goalNode;

        PathNode currentLowFNode = startNode;

        while (openList.Find(x => x.Equals(goalNode)) == null) 
        {
            closeList.Add(currentLowFNode);

            //대각선 이동은 없으므로 g값은 10으로 한다.
            InsertOpenList(currentLowFNode, GetNode(currentLowFNode.nodePosX + 1, currentLowFNode.nodePosY), 1);
            InsertOpenList(currentLowFNode, GetNode(currentLowFNode.nodePosX - 1, currentLowFNode.nodePosY), 1);
            InsertOpenList(currentLowFNode, GetNode(currentLowFNode.nodePosX, currentLowFNode.nodePosY + 1), 1);
            InsertOpenList(currentLowFNode, GetNode(currentLowFNode.nodePosX, currentLowFNode.nodePosY - 1), 1);

            currentLowFNode = FindLowFNode();
            openList.Remove(currentLowFNode);
        }


        PathNode findedNode = currentLowFNode;
        findPaths.Push(goalNode);
        findPaths.Push(findedNode);
        while (findedNode.parentNode != null)
        {
            findPaths.Push(findedNode.parentNode);
            findedNode = findedNode.parentNode;
        }

        return false;
    }

    private PathNode GetNode(int posX, int posY)
    {
        if (posX < 0 || posX >= MAX_MAP_NODE_NUM || posY < 0 || posY >= MAX_MAP_NODE_NUM)
        {
            return null;
        }

        return nodes[posX, posY];
    }

    private void InsertOpenList(PathNode parentNode, PathNode node, int gValue)
    {
        if (node == null || !node.movable || closeList.Find(x => x.Equals(node)) != null
            || openList.Find(x => x.Equals(node)) != null)
        {
            return;
        }

        node.parentNode = parentNode;
        node.gValue = parentNode.gValue + gValue;
        openList.Add(node);
    }

    private PathNode FindLowFNode()
    {
        float lowF = Mathf.Infinity;
        PathNode lowFNode = null;

        foreach (PathNode node in openList)
        {
            float f = node.gValue + distanceToGoal(node);
            if (f < lowF)
            {
                lowF = f;
                lowFNode = node;
            }
        }

        return lowFNode;
    }

    //두 노드간의 h를 구하는 함수
    private float distanceToGoal(PathNode node)
    {
        return Mathf.Abs(node.nodePosX - goalNode.nodePosX) 
            + Mathf.Abs(node.nodePosY - goalNode.nodePosY);
    }
}
