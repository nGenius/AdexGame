using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
    public PlayerCharacter playerCharacter { get; private set; }
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

	    playerCharacter.ManualUpdate();
	}

    private void SpawnPlayerCube()
    {
        Vector3 startPosition = stage.GetStartPosition();

        GameObject cube = Instantiate(Resources.Load("PlayerCharacter")) as GameObject;
        playerCharacter = cube.GetComponent<PlayerCharacter>();
        playerCharacter.fastTransform.parent = stage.fastTransform;
        playerCharacter.transform.localPosition = startPosition;
    }

    public void InputProcess()
    {
        playerCharacter.Stop();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerCharacter.Up();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            playerCharacter.Down();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerCharacter.Left();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            playerCharacter.Right();
        }
    }
}