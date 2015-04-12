using System.Collections.Generic;
using UnityEngine;


public class TacticePlayer : MonoBehaviour
{
    public int moveRange;
    public bool isShowMoveRange;
    public float moveVeolocity;
    public PathNode currentNode;

    private bool isMoving;
    private Vector3 destPos;
    private bool isArriveForwardMove;
    private bool isSelected;

    //path node
    private Stack<PathNode> movePaths;
    private PathNode currentPathNode;
    private Vector3 arrivePosition;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	    if (isMoving)
	    {
	        transform.position = Vector3.MoveTowards(transform.position, arrivePosition, 0.1f);
	        if (transform.position.Equals(arrivePosition))
	        {
	            NextMovePath();
	        }
	    }
	}

    private void NextMovePath()
    {
        if (movePaths.Count == 0)
        {
            isMoving = false;
            gameObject.GetComponent<Animator>().CrossFade("Idle", 0.0f);    
        }
        else
        {
            currentNode = movePaths.Pop();
            CalcNextMoveDist();
            gameObject.GetComponent<Animator>().CrossFade("Walk", 0.0f);
        }
    }


    public void ShowMoveRange()
    {
        
    }

    public void MoveToCell(Transform transform)
    {
        Debug.Log("move..");
    }

    private void CalcNextMoveDist()
    {
        Vector3 moveOffset = new Vector3(currentNode.nodePosX - transform.position.x,
            0, currentNode.nodePosZ - transform.position.z);

        arrivePosition = transform.position + moveOffset;

        transform.forward = moveOffset.normalized;

        
    }

    public void MoveToPaths(Stack<PathNode> paths)
    {
        isMoving = true;
        movePaths = paths;
        NextMovePath();
    }
}
