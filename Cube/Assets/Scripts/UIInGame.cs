using UnityEngine;
using System.Collections;

public class UIInGame : MonoBehaviour
{
    public TacticeScene tacticeScene;
    public GameObject commandMenu;
	
	void Start () {
	    commandMenu.SetActive(false);

	    tacticeScene.characterSelectedEvent += () =>
	    {
	        SetActiveCommandMenu(true);
	    };

	    tacticeScene.characterMoveStartedEvent += () =>
	    {
	        SetActiveCommandMenu(false);
	    };

	}

	void Update () {
	
	}

    public void SetActiveCommandMenu(bool value)
    {
        commandMenu.SetActive(value);
    }

    public void OnClickMoveCommand()
    {
        tacticeScene.ShowMoveRange();   
    }

    public void OnClickActionCommand()
    {
        tacticeScene.ShowMoveRange();
    }
}
