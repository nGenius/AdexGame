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

    public bool isEnemy;

    private Animator animator;
    private JobClass jobClass;
    //ai 
    private AIAgent aiAgent = new AIAgent();

    void Start ()
    {
        aiAgent.Init(this);
        animator = GetComponent<Animator>();
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
            MessageDispatcher.Instance().DispatchMessage(0.0f, GetAIAgent(), GetAIAgent(), AIMsgType.MSG_MOVED);
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

    public void MouseLButtonEvent(RaycastHit rayCastHit)
    {
    }

    public AIAgent GetAIAgent()
    {
        return aiAgent;
    }

    public bool Movable()
    {
        if (aiAgent.GetFSMState().currentState.GetType() == typeof (MoveWaitState))
        {
            return true;
        }

        return false;
    }

    public PlayerState GetCurrentState()
    {
        return aiAgent.GetFSMState().currentState;
    }

    public void NormalAttack()
    {
        Debug.Log("Attack!!!!");
        animator.CrossFade("Attack", 0.0f);
        MessageDispatcher.Instance().DispatchMessage(0.0f, null, GetAIAgent(), AIMsgType.MSG_ATTACK_END);
    }

    public bool TryToMove(MapTile pickedTile)
    {
        Debug.Log("picked MapTile");



        return true;
    }

    public bool TryToMove(Stack<PathNode> paths, float distance)
    {
        if (moveRange >= distance)
        {
            MoveToPaths(paths);
            return true;
        }

        return false;
    }

    public void RotateToGoalNode(PathNode goalNode)
    {
        Vector3 goldNodePosition = new Vector3(goalNode.nodePosX, 0, goalNode.nodePosZ);
        Vector3 currentPosition = new Vector3(currentNode.nodePosX, 0, currentNode.nodePosZ);

        transform.forward = (goldNodePosition - currentPosition).normalized;
    }
}
