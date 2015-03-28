﻿using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections.Generic;

public class MoveRange : MonoBehaviour
{
    public int cellRowNum;
    public float cellWidth = 1.0f;

    private List<GameObject> moveCells = new List<GameObject>();
    public GameObject target;

	void Start ()	
    {
	    CreateCells();

	    ShowMoveRange(3);
    }

    void CreateCells()
    {
        float startPos = 0;
        if (cellRowNum % 2 == 1)
        {
            startPos = -(int)(cellRowNum * cellWidth / 2.0f);
        }
        else
        {
            startPos = -(int)(cellRowNum * cellWidth / 2.0f) + 0.5f;
        }

        Vector3 pos = Vector3.zero;
        for (int i = 0; i < cellRowNum; ++i)
        {
            for (int j = 0; j < cellRowNum; ++j)
            {
                pos.x = startPos + (i * cellWidth);
                pos.z = startPos + (j * cellWidth);
                GameObject cell = Instantiate(Resources.Load("Etc/MoveRangeCell")) as GameObject;
                cell.transform.parent = transform;
                cell.transform.localPosition = pos;
                moveCells.Add(cell);
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowMoveRange(int moveRange)
    {
        transform.position = target.transform.position;
        moveCells.ForEach(x => x.SetActive(false));

        SearchMoveRangeRecursive(target.transform.position, 0, moveRange);
    }

    private void SearchMoveRangeCell(Vector3 checkPos)
    {
        GameObject findCell 
            = moveCells.Find(x => x.transform.localPosition.x.Equals(checkPos.x)
                && x.transform.localPosition.z.Equals(checkPos.z));

        if (findCell != null)
        {
            findCell.SetActive(true);
        }
    }

    private void SearchMoveRangeRecursive(Vector3 currentPos, int depth, int maxDepth)
    {
        if (depth >= maxDepth)
        {
            return;
        }

        Vector3 checkPos = currentPos;
        checkPos.x += 1.0f;
        SearchMoveRangeCell(checkPos);
        SearchMoveRangeRecursive(checkPos, depth + 1, maxDepth);

        checkPos = currentPos;
        checkPos.z += 1.0f;
        SearchMoveRangeCell(checkPos);
        SearchMoveRangeRecursive(checkPos, depth + 1, maxDepth);

        checkPos = currentPos;
        checkPos.x -= 1.0f;
        SearchMoveRangeCell(checkPos);
        SearchMoveRangeRecursive(checkPos, depth + 1, maxDepth);

        checkPos = currentPos;
        checkPos.z -= 1.0f;
        SearchMoveRangeCell(checkPos);
        SearchMoveRangeRecursive(checkPos, depth + 1, maxDepth);
    }
}