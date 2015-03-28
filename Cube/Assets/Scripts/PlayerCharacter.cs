using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Cube
{
    public MovingState movingState { get; private set; }
    private Vector3 originPosition;
    private float deltaTime;
    private Animator animator;
    private bool isReserveStop;
    private bool isFirstUpdatePosition;
    private List<Vector3> moveDirections = new List<Vector3>();
    protected override void AwakeOverride()
    {
        cubeType = CubeType.Player;
        movingState = MovingState.Wait;
        animator = GetComponent<Animator>();
    }

    public void Stop()
    {
        isReserveStop = true;
    }

    public void Up()
    {
        isReserveStop = false;

        if (movingState == MovingState.Moving)
        {
            return;
        }

        StartMove(new Vector3(0, 0, 1));
    }

    public void Down()
    {
        isReserveStop = false;

        if (movingState == MovingState.Moving)
        {
            return;
        }

        StartMove(new Vector3(0, 0, -1));
    }

    public void Left()
    {
        isReserveStop = false;

        if (movingState == MovingState.Moving)
        {
            return;
        }

        StartMove(new Vector3(-1, 0, 0));
    }

    public void Right()
    {
        isReserveStop = false;

        if (movingState == MovingState.Moving)
        {
            return;
        }

        StartMove(new Vector3(1, 0, 0));
    }

    private void StartMove(Vector3 direction)
    {
        movingState = MovingState.Moving;

        originPosition = fastTransform.position;
        fastTransform.forward = direction;
        
        CalculateMoveDirection(direction);
        deltaTime = 0;
        isFirstUpdatePosition = true;
    }

    private void CalculateMoveDirection(Vector3 direction)
    {
        moveDirections.Clear();

        Vector3 destPosition = originPosition + direction;
        if (GameControl.Instance.stage.IsExistStageCube(destPosition))
        {
            if (GameControl.Instance.stage.IsExistStageCube(destPosition + Vector3.up))
            {
                //moveDirections.Add(Vector3.up);
            }
            else
            {
                moveDirections.Add(Vector3.up);
                moveDirections.Add(direction);
            }
        }
        else if (GameControl.Instance.stage.IsExistStageCube(destPosition + Vector3.down) == false)
        {
            moveDirections.Add(direction + Vector3.down);
        }
        else
        {
            moveDirections.Add(direction);
        }
    }

    public void ManualUpdate()
    {
        if (movingState == MovingState.Moving)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        if (moveDirections.Count == 0)
        {
            ForceStop();
            return;
        }

        if (moveDirections[0].Equals(Vector3.up))
        {
            if (isFirstUpdatePosition)
            {
                isFirstUpdatePosition = false;
                animator.CrossFade("Up", 0.1f, 0, 0.2f);
                originPosition = fastTransform.position + fastTransform.forward / 3;
                fastTransform.position = originPosition + moveDirections[0];

                if (moveDirections.Count == 2)
                {
                    moveDirections[1] -= moveDirections[1] / 3;
                }
            }
            else if (animator.IsInTransition(0) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                ForceMove(originPosition + moveDirections[0]);
                moveDirections.RemoveAt(0);
                isFirstUpdatePosition = true;
            }
        }
        else
        {
            if (isFirstUpdatePosition)
            {
                isFirstUpdatePosition = false;
                animator.CrossFade("Walk", 0.1f);
            }

            fastTransform.position += moveDirections[0] * Time.deltaTime;

            deltaTime += Time.deltaTime;

            if (deltaTime >= 1)
            {
                ForceMove(originPosition + moveDirections[0]);
                moveDirections.RemoveAt(0);
                isFirstUpdatePosition = true;
            }
        }
    }

    private void ForceMove(Vector3 position)
    {
        fastTransform.position = position;
        originPosition = fastTransform.position;
        deltaTime = 0.0f;
    }

    private void ForceStop()
    {
        movingState = MovingState.Wait;
        GameControl.Instance.stage.GravityProcess();

        if (isReserveStop)
        {
            animator.CrossFade("Idle", 0.1f);
        }
    }
}