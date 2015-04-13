using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using System.Collections;

//tactice용 A* path finding(대각선 이동이 없다. 즉 네방향 검사)
//F = G + H
//G - 시작점 A부터 새로운 사각형까지의 이동비용(직각이면 1. 대각선은 1.4-루트2)
//H - 얻어진 사각형으로부터 최종목적지점까지의 이동비용. 가로세로 거리
using UnityEngine.UI;

public class PathFinding
{

    public const int MAX_MAP_NODE_NUM = 10;
    private PathNode[,] nodes;
    private Stack<PathNode> findPaths = new Stack<PathNode>();
    private List<PathNode> openList = new List<PathNode>();
    private List<PathNode> closeList = new List<PathNode>();
    private PathNode goalNode;
    
    //private PathNode current


    //찾으면 true 못찾으면 false
    public Stack<PathNode> FindPath(PathNode[,] nodes, PathNode startNode, PathNode goalNode)
    {
        this.nodes = nodes;
        this.goalNode = goalNode;

        foreach (PathNode node in nodes)
        {
            node.parentNode = null;
        }

        PathNode currentLowFNode = startNode;

        openList.Clear();
        closeList.Clear();
        findPaths.Clear();

        while (openList.Find(x => x.Equals(goalNode)) == null) 
        {
            Debug.Log(currentLowFNode.Equals(goalNode));
            closeList.Add(currentLowFNode);

            //대각선 이동은 없으므로 g값은 1로 한다.
            InsertOpenList(currentLowFNode, GetNode(currentLowFNode.nodePosX + 1, currentLowFNode.nodePosZ), 1);
            InsertOpenList(currentLowFNode, GetNode(currentLowFNode.nodePosX - 1, currentLowFNode.nodePosZ), 1);
            InsertOpenList(currentLowFNode, GetNode(currentLowFNode.nodePosX, currentLowFNode.nodePosZ + 1), 1);
            InsertOpenList(currentLowFNode, GetNode(currentLowFNode.nodePosX, currentLowFNode.nodePosZ - 1), 1);

            currentLowFNode = FindLowFNode();
            if (currentLowFNode.Equals(goalNode))
            {
                break;
            }
            openList.Remove(currentLowFNode);
        }


        PathNode findedNode = currentLowFNode;
        findPaths.Push(findedNode);
        while (findedNode.parentNode != null)
        {
            findPaths.Push(findedNode.parentNode);
            findedNode = findedNode.parentNode;
        }

        return findPaths;
    }

    private PathNode GetNode(int posX, int posY)
    {
        if (posX < 0 || posX >= TacticeScene.MAX_MAP_NODE_NUM || posY < 0 || posY >= TacticeScene.MAX_MAP_NODE_NUM)
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
            float f = node.gValue + distanceToGoal(node, goalNode);
            if (f <= lowF)
            {
                lowF = f;
                lowFNode = node;
            }
        }

        return lowFNode;
    }

    //두 노드간의 h를 구하는 함수
    public float distanceToGoal(PathNode node, PathNode goalNode)
    {
        return Mathf.Abs(node.nodePosX - goalNode.nodePosX) 
            + Mathf.Abs(node.nodePosZ - goalNode.nodePosZ);
    }
}
