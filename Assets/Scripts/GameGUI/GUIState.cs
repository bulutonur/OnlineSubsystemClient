using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIState : MonoBehaviour, FSM.IState<GameGUI>
{
    protected GameGUI gui;
    public void Enter(GameGUI entity)
    {
        gui = entity;
        gameObject.SetActive(true);
    }

    // code that runs when we exit the state
    public void Exit(GameGUI entity)
    {
        gameObject.SetActive(false);
    }
    
    
}