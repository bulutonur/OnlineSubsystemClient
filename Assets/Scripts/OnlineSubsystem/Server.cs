namespace OnlineSubsystem
{
    /// <summary>
    /// Server class. Use it for displaying serverlist and connecting to a server
    /// </summary>
    public class Server
    {
        public string name { get; set; }
        public int port { get; set; }
        public string map { get; set; }
        public int playercount { get; set; }
        public int maxplayers { get; set; }
        public bool pwprotected { get; set; }
        public string gamemode { get; set; }
        public string ip { get; set; }
    }
}