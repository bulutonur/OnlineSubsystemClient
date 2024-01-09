using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGUI : MonoBehaviour
{
    public FSM.StateMachine<GameGUI> fsm;

    [SerializeField] public GUIState mainMenuState;
    [SerializeField] public GUIState lobbyState;
    [SerializeField] public GUIState roomCreateState;
    [SerializeField] public GUIState roomSearchState;
    [SerializeField] public GUIState gameplayState;
    
    [SerializeField] public CustomNetworkManager networkManager;
    
    // Start is called before the first frame update
    void Start()
    {
        fsm = new FSM.StateMachine<GameGUI>(this);
        fsm.ChangeState(mainMenuState);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Execute();
    }

    public void OnServerStarted()
    {
        fsm.ChangeState(gameplayState);
    }
    
    public void OnServerStopped()
    {
        fsm.ChangeState(lobbyState);
    }
    
    public void OnClientStarted()
    {
        fsm.ChangeState(gameplayState);
    }
    
    public void OnClientStopped()
    {
        fsm.ChangeState(lobbyState);
    }
}
