using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using OnlineSubsystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIRoomSearchState : GUIState
{
    [SerializeField] private Button refreshButton;
    [SerializeField] private ServerBrowser serverBrowser;
    [SerializeField] private Transform passwordPanel;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI serverNameText;
    private Server server;
    private void Start()
    {
        OnlineSubsystemManager.Instance.OnGetServerlistSuccess += OnGetServerlistSuccess;
        OnlineSubsystemManager.Instance.OnGetServerlistFailed += OnGetServerlistFailed;
    }

    void OnEnable()
    {
        ClosePasswordPanel();
        OnClick_Refresh();
    }

    public void OnClick_Refresh()
    {
        OnlineSubsystemManager.Instance.GetServerList();
        refreshButton.enabled = false;
    }

    private void OnGetServerlistSuccess(GetServerListResult result)
    {
        serverBrowser.Assign(result.servers,gui);
        refreshButton.enabled = true;
    }
    private void OnGetServerlistFailed()
    {
        // @TODO Implement error message displaying
        refreshButton.enabled = true;
    }

    public void OnClick_Back()
    {
        gui.fsm.ChangeState(gui.lobbyState);
    }

    public void OpenPasswordPanel(Server _server)
    {
        server = _server;
        serverNameText.text = server.name;
        passwordPanel.gameObject.SetActive(true);
    }
    
    public void ClosePasswordPanel()
    {
        passwordPanel.gameObject.SetActive(false);
    }
    
    public void OnClick_Join()
    {
        gui.networkManager.ConnectWithPassword(server.ip,server.port,passwordInputField.text);
    }

}
