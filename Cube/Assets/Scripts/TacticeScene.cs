using UnityEngine;
using System.Collections;

public class TacticeScene : MonoBehaviour
{
    public MoveRange moveRange;

    private TacticePlayer selectedTacticePlayer;
    private PathFinding pathFinding = new PathFinding();
    // Use this for initialization
    private void Start()
    {
        pathFinding.CreateTestNodes();

        PathNode startNode = new PathNode();
        startNode.nodePosX = 1;
        startNode.nodePosY = 1;

        PathNode goalNode = new PathNode();
        goalNode.nodePosX = 3;
        goalNode.nodePosY = 3;

        pathFinding.FindPath(startNode, goalNode);
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
                    selectedTacticePlayer = tacticePlayer;
                }

                if (rayCastHit.collider && rayCastHit.transform.tag == "MoveRange")
                {
                    selectedTacticePlayer.MoveToCell(rayCastHit.transform);
                }
            }
        }
    }
}
    ;