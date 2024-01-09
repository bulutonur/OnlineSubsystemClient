using OnlineSubsystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI playersText;
    [SerializeField] private TextMeshProUGUI passwordText;
    [SerializeField] private Button joinButton;
    [SerializeField] private TextMeshProUGUI joinButtonText;
    private Server server;
    public GameGUI gui;
    public void Assign(Server _server,GameGUI _gui)
    {
        server = _server;
        gui = _gui;
        nameText.text = server.name;
        playersText.text = $"{server.playercount}/{server.maxplayers}";
        if (server.pwprotected)
        {
            passwordText.text = "Yes";
        }
        else
        {
            passwordText.text = "No";
        }

        bool isFull = server.playercount >= server.maxplayers;
        if (isFull)
        {
            joinButton.enabled = false;
            joinButtonText.text = "Full";
        }
        else
        {
            joinButton.enabled = true;
            joinButtonText.text = "Join";
        }
    }

    public void OnClick_Join()
    {
        // @TODO Implement password check
        if (server.pwprotected)
        {
            (gui.roomSearchState as GUIRoomSearchState).OpenPasswordPanel(server);
        }
        else
        {
           gui.networkManager.ConnectWithoutPassword(server.ip,server.port);
        }


    }
}
