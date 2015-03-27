using UnityEngine;

public class StageCube : Cube
{
    public bool isStartPosition;

    protected override void AwakeOverride()
    {
        cubeType = CubeType.Stage;
    }

    public bool IsExist(Vector3 worldPosition)
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        return boxCollider.bounds.Contains(worldPosition);
    }
}