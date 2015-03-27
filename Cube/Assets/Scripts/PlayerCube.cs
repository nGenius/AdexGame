using UnityEngine;

public enum MovingState
{
    Wait,
    Moving,
}

public class PlayerCube : Cube
{
    public MovingState movingState { get; private set; }
    private Vector3 moveDirection;
    private Vector3 rotateAxis;
    private Vector3 originPosition;
    private Quaternion originRotation;
    private float targetDegree;
    private float degree;

    protected override void AwakeOverride()
    {
        cubeType = CubeType.Player;
        movingState = MovingState.Wait;
    }

    public void Up()
    {
        if (movingState == MovingState.Moving)
        {
            return;
        }

        StartRotateMove(new Vector3(0, 0, 1));
    }

    public void Down()
    {
        if (movingState == MovingState.Moving)
        {
            return;
        }

        StartRotateMove(new Vector3(0, 0, -1));
    }

    public void Left()
    {
        if (movingState == MovingState.Moving)
        {
            return;
        }

        StartRotateMove(new Vector3(-1, 0, 0));
    }

    public void Right()
    {
        if (movingState == MovingState.Moving)
        {
            return;
        }

        StartRotateMove(new Vector3(1, 0, 0));
    }

    private void StartRotateMove(Vector3 direction)
    {
        movingState = MovingState.Moving;

        rotateAxis = Vector3.Cross(Vector3.up, direction);
        originPosition = fastTransform.position;
        originRotation = fastTransform.rotation;

        ModifyMoveVariable(direction);
    }

    private void ModifyMoveVariable(Vector3 direction)
    {
        moveDirection = direction;
        targetDegree = 90.0f;
        degree = 0.0f;

        Vector3 destPosition = originPosition + moveDirection;
        if (GameControl.Instance.stage.IsExistStageCube(destPosition))
        {
            if (GameControl.Instance.stage.IsExistStageCube(destPosition + Vector3.up))
            {
                moveDirection = Vector3.up;
                targetDegree = 90.0f;
            }
            else
            {
                moveDirection += Vector3.up;
                targetDegree = 180.0f;
            }
        }
        else if (GameControl.Instance.stage.IsExistStageCube(destPosition + Vector3.down) == false)
        {
            moveDirection += Vector3.down;
            targetDegree = 180.0f;
        }
    }

    private Vector3 ModifyAxisPoint(Vector3 direction)
    {
        if (direction.y > 0)
        {
            Vector3 offset = direction / 2.0f;
            return offset;
        }
        else
        {
            Vector3 offset = direction / 2.0f;
            offset.y = -0.5f;
            return offset;
        }
    }

    void Update()
    {
        if (movingState == MovingState.Moving)
        {
            UpdateRotate();
        }
    }

    private void UpdateRotate()
    {
        Vector3 axisPoint = originPosition + ModifyAxisPoint(moveDirection);
        degree += 360.0f * Time.deltaTime;
        fastTransform.position = originPosition;
        fastTransform.rotation = originRotation;

        if (degree >= targetDegree)
        {
            fastTransform.rotation = originRotation * Quaternion.AngleAxis(targetDegree, rotateAxis);
            ForceMove(moveDirection);
            movingState = MovingState.Wait;
        }
        else
        {
            fastTransform.RotateAround(axisPoint, rotateAxis, degree);
        }
    }

    private void ForceMove(Vector3 direction)
    {
        fastTransform.position += direction;

        GameControl.Instance.stage.GravityProcess();
    }
}