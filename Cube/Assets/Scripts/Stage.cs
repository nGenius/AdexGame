using System.Collections.Generic;
using UnityEngine;

public enum StageMovingState
{
    Wait,
    Moving,
}

public class Stage : MonoBehaviour
{
    public List<StageCube> stageCubes = new List<StageCube>();
    public Transform fastTransform { get; private set; }

    public StageMovingState stageMovingState { get; private set; }

    private Vector3 rotateAxis;
    private Quaternion originRotation;
    private float degree;

    
    void Awake()
    {
        stageCubes = new List<StageCube>(GetComponentsInChildren<StageCube>());
        fastTransform = transform;
        
    }

    public Vector3 GetStartPosition()
    {
        StageCube startCube = stageCubes.Find(x => x.isStartPosition);
        return startCube.fastTransform.position + new Vector3(0, 0.5f, 0);
    }

    public bool IsExistStageCube(Vector3 worldPosition)
    {
        worldPosition += new Vector3(0, 0.5f, 0);

        foreach (StageCube stageCube in stageCubes)
        {
            if (stageCube.IsExist(worldPosition))
            {
                return true;
            }
        }

        return false;
    }

    void Update()
    {
        if (stageMovingState == StageMovingState.Moving)
        {
            UpdateRotate();
        }
    }

    public void GravityProcess()
    {
        Vector3 originPosition = GameControl.Instance.playerCharacter.fastTransform.position;
        
        bool result = GravityProcess(originPosition, Vector3.down);

        if (result == false)
        {
            GravityProcess(originPosition, Vector3.left);
            GravityProcess(originPosition, Vector3.right);
            GravityProcess(originPosition, Vector3.forward);
            GravityProcess(originPosition, Vector3.back);
        }
    }

    private bool GravityProcess(Vector3 originPosition, Vector3 gravityDirection)
    {
        bool result = Physics.Raycast(originPosition, gravityDirection, float.PositiveInfinity,
            1 << LayerMask.NameToLayer("StageCube"));
        if (result)
        {
            StartRotate(gravityDirection);
        }

        return result;
    }

    private void StartRotate(Vector3 direction)
    {
        if (direction.Equals(Vector3.down))
        {
            return;
        }

        stageMovingState = StageMovingState.Moving;

        originRotation = fastTransform.rotation;
        rotateAxis = Vector3.Cross(direction, Vector3.down);

        degree = 0.0f;
    }

    private void UpdateRotate()
    {
        degree += 180.0f * Time.deltaTime;
        fastTransform.localRotation = originRotation;

        if (degree >= 90.0f)
        {
            fastTransform.rotation = Quaternion.AngleAxis(90.0f, rotateAxis) * originRotation;
            stageMovingState = StageMovingState.Wait;
        }
        else
        {
            fastTransform.rotation = Quaternion.AngleAxis(degree, rotateAxis) * originRotation;
        }
    }
}