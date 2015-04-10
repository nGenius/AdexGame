using UnityEngine;


public class TacticePlayer : MonoBehaviour
{
    public int moveRange;
    public bool isShowMoveRange;
    public float moveVeolocity;

    private bool isMoving;
    private Vector3 destPos;
    private bool isArriveForwardMove;
    private bool isSelected;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	    if (isMoving)
	    {
	        //Vector3 calcPos = transform.position;
	        //Vector3.MoveTowards(transform.position, )
	    }
	}

    public void ShowMoveRange()
    {
        
    }

    public void MoveToCell(Transform transform)
    {
        Debug.Log("move..");


    }
}
