using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUILobbyState : GUIState
{

    public void OnClick_CreateRoom()
    {
        gui.fsm.ChangeState(gui.roomCreateState);
    }
    
    public void OnClick_SearchRoom()
    {
        gui.fsm.ChangeState(gui.roomSearchState);
    }
   
}
