using System.Collections.Generic;
using UnityEngine;


public enum AIMsgType
{
    MSG_SELECTED,
    MSG_MOVED,
}

public class AIAgent
{
    //ai
    private FSMStateMachince stateMachine = new FSMStateMachince();

    //ai function
    public void HandleMessage(AIMessage aiMessage)
    {
        stateMachine.HandleMessage(this, aiMessage);
    }

    public void Init()
    {
        stateMachine.SetCurrentState(new IdleState());
    }

    public FSMStateMachince GetFSMState()
    {
        return stateMachine;
    }
}

public struct AIMessage
{
    public AIMessage(AIAgent sender, AIAgent receiver, AIMsgType msg)
    {
        this.sender = sender;
        this.receiver = receiver;
        this.msg = msg;
    }

    public AIAgent sender;

    public AIAgent receiver;

    public AIMsgType msg;
}

public interface PlayerState
{
    void Enter();

    void Excute();

    void Exit();

    bool OnMessage(AIAgent aiAgent, AIMessage aiMessage);
}

public class WaitMoveState : PlayerState
{
    public void Enter()
    {
        Debug.Log("WaitMoveState Enter..");
    }

    public void Excute()
    {
    }

    public void Exit()
    {
    }

    public bool OnMessage(AIAgent aiAgent, AIMessage aiMessage)
    {
        switch (aiMessage.msg)
        {
            case AIMsgType.MSG_MOVED:
                aiAgent.GetFSMState().ChangeState(new IdleState());
                return true;
        }

        //전역 상태로 메시지를 넘긴다.
        return false;
    }
}

public class IdleState : PlayerState
{
    public void Enter()
    {
        Debug.Log("IdleState Enter..");
    }

    public void Excute()
    {
        
    }

    public void Exit()
    {
        
    }

    public bool OnMessage(AIAgent aiAgent, AIMessage aiMessage)
    {

        switch (aiMessage.msg)
        {
            case AIMsgType.MSG_SELECTED:
                Debug.Log("WaitState MSG_SELECTED.."); 
                aiAgent.GetFSMState().ChangeState(new WaitMoveState());
            return true;
        }

        //전역 상태로 메시지를 넘긴다.
        return false;
    }
}



public class MessageDispatcher
{
    protected MessageDispatcher()
    {
        
    }

    private static MessageDispatcher instance;

    public static MessageDispatcher Instance()
    {
        if (instance == null)
        {
            instance = new MessageDispatcher();
        }

        return instance;
    }

    public void DispatchMessage(double delay, AIAgent sender, AIAgent receiver, AIMsgType msg)
    {
        AIMessage aiMessage = new AIMessage(sender, receiver, msg);
        Discharge(receiver, aiMessage);

        //TODO : msg delay에 대한 처리
    }

    private void Discharge(AIAgent receiver, AIMessage msg)
    {
        receiver.HandleMessage(msg);
    }

    //TODO : 대기 중인 메시지(Delay된)를 매틱 검사, 처리
    //public void DispatchDelayedMessages() {}
}

public class FSMStateMachince
{
    public PlayerState currentState { get; private set; }
    private PlayerState previousState;

    public void SetCurrentState(PlayerState state)
    {
        currentState = state;
    }

    public void ChangeState(PlayerState newState)
    {
        previousState = currentState;

        currentState.Exit();

        currentState = newState;

        currentState.Enter();
    }

    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }


    public bool HandleMessage(AIAgent aiAgent, AIMessage aiMessage)
    {
        if (currentState.OnMessage(aiAgent, aiMessage))
        {
            return true;
        }

        //TODO : 전역 상태에 대한 처리

        return false;
    }
}

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

    //ai 
    private AIAgent aiAgent = new AIAgent();

    void Start ()
    {
        aiAgent.Init();
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

    public bool Movabable()
    {
        if (aiAgent.GetFSMState().currentState.GetType() == typeof (WaitMoveState))
        {
            return true;
        }

        return false;
    }
}
