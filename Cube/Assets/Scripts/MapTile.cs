using UnityEngine;
using System.Collections;

public class PathNode
{
    public bool movable;
    public int nodePosX;
    public int nodePosZ;

    //실제 위치
    public Vector3 position;
    public PathNode parentNode;
    public float gValue;

    public bool Equals(PathNode obj)
    {
        return nodePosX == obj.nodePosX && nodePosZ == obj.nodePosZ;
    } 
}


public class MapTile : MonoBehaviour
{
    public PathNode pathNode = new PathNode();

    public bool movable = true;
    public Material[] materials;

    private bool latestMovable = true;
	// Use this for initialization
	void Start ()
	{
	    latestMovable = true;
	}
	
	// Update is called once per frame
	void Update () {

	    if (latestMovable != movable)
	        pathNode.movable = movable;

	    {	        latestMovable = movable;

	        if (movable)
	        {
	            GetComponent<MeshRenderer>().material = materials[1];
	        }
	        else
	        {
                GetComponent<MeshRenderer>().material = materials[0];   
	        }
	    }
	}
}
