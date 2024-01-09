using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMainMenuState : GUIState
{
 
    public void OnClick_LoginButton()
    {
        gui.fsm.ChangeState(gui.lobbyState);
        
    }
    
    
}
