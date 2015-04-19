using System;
using UnityEngine;
using System.Collections;

public class UIInGame : MonoBehaviour
{
    public TacticeScene tacticeScene;
    public GameObject commandMenu;
    public UILabel stateLabel;
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

	    if (tacticeScene.selectedTacticePlayer)
	    {
	        PlayerState state = tacticeScene.selectedTacticePlayer.GetCurrentState();
	        stateLabel.text = state.GetType().ToString();
	    }
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
        tacticeScene.ShowActionRange();
    }

   
}
