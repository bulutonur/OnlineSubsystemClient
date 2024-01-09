using kcp2k;
using Mirror;
using OnlineSubsystem;
using UnityEngine;
using UnityEngine.Events;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
    API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/


// Custom NetworkManager that simply assigns the correct racket positions when
// spawning players. The built in RoundRobin spawn method wouldn't work after
// someone reconnects (both players would be on the same side).
[AddComponentMenu("")]
public class CustomNetworkManager : NetworkManager
{
    public Transform leftRacketSpawn;
    public Transform rightRacketSpawn;
    GameObject ball;

    private Server server=null;

    public UnityEvent OnServerStarted;
    public UnityEvent OnServerStopped;
    public UnityEvent OnClientStarted;
    public UnityEvent OnClientStopped;
    
    void Start()
    {
        base.Start();
        OnlineSubsystemManager.Instance.OnRegisterServerSuccess += OnRegisterServerSuccess;
        OnlineSubsystemManager.Instance.OnUpdateServerSuccess += OnUpdateServerSuccess;
    }

    private string serverName;
    private string map;
    private int maxPlayers;
    private string gameMode;
    private bool pwProtected;
    public void CreateServer(string _serverName,string _map,int _maxPlayers,string _gameMode,bool _pwProtected,string password="")
    {
        int port = PortFinder.Find();
        GetComponent<KcpTransport>().port=(ushort)port;
        
        PasswordAuthenticator passwordAuthenticator = GetComponent<PasswordAuthenticator>();
        passwordAuthenticator.serverPassword = password;
        // Required for hosting(server + client)
        passwordAuthenticator.password = password;
        
        
        serverName=_serverName;
        map=_map;
        maxPlayers=_maxPlayers;
        gameMode=_gameMode;
        pwProtected=_pwProtected;
        StartHost();
    }

    public void ConnectWithoutPassword(string ip, int port)
    {
        networkAddress = ip;
        GetComponent<KcpTransport>().port = (ushort)port;
        NetworkManager.singleton.StartClient();
    }
    
    public void ConnectWithPassword(string ip, int port,string password)
    {
        networkAddress = ip;
        GetComponent<KcpTransport>().port = (ushort)port;
        
        PasswordAuthenticator passwordAuthenticator =GetComponent<PasswordAuthenticator>();
        passwordAuthenticator.password = password;
        
        NetworkManager.singleton.StartClient();
    }

    
    private void OnRegisterServerSuccess(Server _server,float hearthRate)
    {
        server = _server;
        // It does not immediately add hosting player 
        // So add it brutally
        if (server != null)
        {
            server.playercount=1;
            OnlineSubsystemManager.Instance.UpdateServer(server);
        }
    }
    
    private void OnUpdateServerSuccess(Server _server)
    {
        server = _server;
    }
    
    public override void OnStartServer()
    {
        OnServerStarted?.Invoke();
        int port = GetComponent<KcpTransport>().port;
        OnlineSubsystemManager.Instance.RegisterServer(serverName,port,map,maxPlayers, pwProtected, gameMode);
    }
    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();
        OnlineSubsystemManager.Instance.UnregisterServer(server);
    }
    
    public override void OnStartClient()
    {
        OnClientStarted?.Invoke();
    }
    
    public override void OnStopClient()
    {
        OnClientStopped?.Invoke();
    }
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // add player at correct spawn position
        Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);

        // spawn ball if two players
        if (numPlayers == 2)
        {
            ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
        }

       
        if (server!=null)
        {
            server.playercount+=1;
            // @TODO Implement on failed logic
            OnlineSubsystemManager.Instance.UpdateServer(server);
        }

    }
    

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // destroy ball
        if (ball != null)
        {
            NetworkServer.Destroy(ball);
        }
        
        if (server!=null)
        {
            if (server.playercount > 0)
            {
                server.playercount--;
            }
        }

        // @TODO Implement on failed logic
        OnlineSubsystemManager.Instance.UpdateServer(server);
        
        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }
    
    
}