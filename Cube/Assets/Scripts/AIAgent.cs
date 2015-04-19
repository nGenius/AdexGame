using System.Collections.Generic;
using UnityEngine;

public enum AIMsgType
{
    MSG_SELECTED,
    MSG_MOVED,
    MSG_PRESS_MOVE_BTN,
    MSG_PRESS_ATTACK_BTN,
    MSG_ATTACK_WAIT,
    MSG_PICKED_NODE,
    MSG_ATTACK_START,
    MSG_ATTACK_END,
}


public class AIAgent
{
    //ai
    public TacticePlayer owner { get; private set; }
    private FSMStateMachince stateMachine;

    //ai function
    public void HandleMessage(AIMessage aiMessage)
    {
        stateMachine.HandleMessage(this, aiMessage);
    }

    public void Init(TacticePlayer tacticePlayer)
    {
        owner = tacticePlayer;
        stateMachine = new FSMStateMachince(tacticePlayer);
        stateMachine.SetCurrentState(new IdleState());
    }

    public FSMStateMachince GetFSMState()
    {
        return stateMachine;
    }
}

public struct AIMessage
{
    public AIMessage(AIAgent sender, AIAgent receiver, AIMsgType msg, params object[] args)
    {
        this.sender = sender;
        this.receiver = receiver;
        this.msg = msg;
        this.args = args;
    }

    public AIAgent sender;

    public AIAgent receiver;

    public AIMsgType msg;

    public object[] args;

}

public interface PlayerState
{
    void Enter(TacticePlayer owner);

    void Excute();

    void Exit(TacticePlayer owner);

    bool OnMessage(AIAgent aiAgent, AIMessage aiMessage);
}

public class MoveState : PlayerState
{
    public void Enter(TacticePlayer owner)
    {
        throw new System.NotImplementedException();
    }

    public void Excute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit(TacticePlayer owner)
    {
        throw new System.NotImplementedException();
    }

    public bool OnMessage(AIAgent aiAgent, AIMessage aiMessage)
    {
        throw new System.NotImplementedException();
    }
}
public class WaitCommandState : PlayerState
{
    public void Enter(TacticePlayer owner)
    {
        //Debug.Log("WaitCommandState Enter..");
    }

    public void Excute()
    {
    }

    public void Exit(TacticePlayer owner)
    {
    }

    public bool OnMessage(AIAgent aiAgent, AIMessage aiMessage)
    {
        switch (aiMessage.msg)
        {
            case AIMsgType.MSG_PRESS_MOVE_BTN:
                aiAgent.GetFSMState().ChangeState(new MoveWaitState());
                return true;
          
            case AIMsgType.MSG_PRESS_ATTACK_BTN:
                aiAgent.GetFSMState().ChangeState(new AttackWaitState());
                return true;
        }

        //전역 상태로 메시지를 넘긴다.
        return false;
    }
}

public class IdleState : PlayerState
{
    public void Enter(TacticePlayer owner)
    {
        //Debug.Log("IdleState Enter..");
    }

    public void Excute()
    {

    }

    public void Exit(TacticePlayer owner)
    {

    }

    public bool OnMessage(AIAgent aiAgent, AIMessage aiMessage)
    {

        switch (aiMessage.msg)
        {
            case AIMsgType.MSG_SELECTED:
                aiAgent.GetFSMState().ChangeState(new WaitCommandState());
                return true;

            case  AIMsgType.MSG_PRESS_MOVE_BTN:
                aiAgent.GetFSMState().ChangeState(new MoveWaitState());
                return true;
        }

        //전역 상태로 메시지를 넘긴다.
        return false;
    }
}

public class MoveWaitState : PlayerState
{
    public void Enter(TacticePlayer owner)
    {
    }

    public void Excute()
    {
    }

    public void Exit(TacticePlayer owner)
    {
    }

    public bool OnMessage(AIAgent aiAgent, AIMessage aiMessage)
    {
        switch (aiMessage.msg)
        {
            case AIMsgType.MSG_MOVED:
                aiAgent.GetFSMState().ChangeState(new IdleState());
                return true;

            case AIMsgType.MSG_PICKED_NODE:
                Stack<PathNode> paths = aiMessage.args[0] as Stack<PathNode>;
                float distance = (float)aiMessage.args[1];
                return aiAgent.owner.TryToMove(paths, distance);
        }
        return false;
    }
}


public class AttackWaitState : PlayerState
{
    public void Enter(TacticePlayer owner)
    {
        Debug.Log("WaitCommandState Enter..");
    }

    public void Excute()
    {
    }

    public void Exit(TacticePlayer owner)
    {
    }

    public bool OnMessage(AIAgent aiAgent, AIMessage aiMessage)
    {
        switch (aiMessage.msg)
        {
            case AIMsgType.MSG_PICKED_NODE:
                PathNode attackNode = aiMessage.args[2] as PathNode;
                aiAgent.owner.RotateToGoalNode(attackNode);
                aiAgent.GetFSMState().ChangeState(new NormalAttackState());
                return true;
        }
        return false;
    }
}

public class NormalAttackState : PlayerState
{
    public void Enter(TacticePlayer owner)
    {
        owner.NormalAttack();
    }

    public void Excute()
    {
    }

    public void Exit(TacticePlayer owner)
    {
    }

    public bool OnMessage(AIAgent aiAgent, AIMessage aiMessage)
    {
        switch (aiMessage.msg)
        {
            case AIMsgType.MSG_ATTACK_END:
                aiAgent.GetFSMState().ChangeState(new IdleState());
            return true;
        }

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

    public void DispatchMessage(double delay, AIAgent sender, AIAgent receiver, AIMsgType msg, params object[] args)
    {
        AIMessage aiMessage = new AIMessage(sender, receiver, msg, args);
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
    public FSMStateMachince(TacticePlayer owner)
    {
        this.owner = owner;
    }

    public PlayerState currentState { get; private set; }
    private PlayerState previousState;

    private TacticePlayer owner;

    public void SetCurrentState(PlayerState state)
    {
        currentState = state;
    }

    public void ChangeState(PlayerState newState)
    {
        previousState = currentState;

        currentState.Exit(owner);

        currentState = newState;

        currentState.Enter(owner);
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
