using UnityEngine;

public enum CubeType
{
    Stage,
    Player,
    Npc,
    Enemy,
}

public class Cube : MonoBehaviour
{
    public CubeType cubeType { get; protected set; }

    public Transform fastTransform { get; private set; }

    void Awake()
    {
        fastTransform = transform;

        AwakeOverride();
    }

    protected virtual void AwakeOverride()
    {
        
    }
}