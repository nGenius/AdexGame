using UnityEngine;
using System.Collections;

public class TacticeScene : MonoBehaviour
{
    public MoveRange moveRange;
    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayCastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayCastHit))
            {
                if (rayCastHit.collider && rayCastHit.transform.tag == "Player")
                {
                    TacticePlayer tacticePlayer = rayCastHit.transform.GetComponent<TacticePlayer>();
                    moveRange.ShowMoveRange(tacticePlayer.moveRange);
                }
            }
        }
    }
}
    ;