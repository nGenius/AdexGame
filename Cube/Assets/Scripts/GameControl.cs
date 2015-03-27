using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
    public PlayerCube playerCube { get; private set; }
    public Stage stage { get; private set; }

    private static GameControl instance;
    public static GameControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameControl)) as GameControl;
            }

            return instance;
        }
    }

	// Use this for initialization
	void Start ()
	{
        stage = FindObjectOfType(typeof(Stage)) as Stage;
	    SpawnPlayerCube();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (stage.stageMovingState == StageMovingState.Wait)
	    {
	        InputProcess();
	    }
	}

    private void SpawnPlayerCube()
    {
        Vector3 startPosition = stage.GetStartPosition();

        GameObject cube = Instantiate(Resources.Load("PlayerCube")) as GameObject;
        playerCube = cube.GetComponent<PlayerCube>();
        playerCube.fastTransform.parent = stage.fastTransform;
        playerCube.transform.localPosition = startPosition;
    }

    public void InputProcess()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerCube.Up();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            playerCube.Down();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerCube.Left();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            playerCube.Right();
        }
    }
}