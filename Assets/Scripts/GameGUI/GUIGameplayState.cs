using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GUIGameplayState : GUIState
{
   public void OnClick_Back()
   {
       NetworkManagerMode mode=NetworkManager.singleton.mode;
       if (mode==NetworkManagerMode.Host)
       {
           NetworkManager.singleton.StopHost();
       }
       else if (mode==NetworkManagerMode.ClientOnly)
       {
           NetworkManager.singleton.StopClient();
       }
       gui.fsm.ChangeState(gui.lobbyState);
   }
    
}
