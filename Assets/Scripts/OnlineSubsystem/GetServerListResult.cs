using System.Collections.Generic;

namespace OnlineSubsystem
{
    /// <summary>
    /// A class for deserializing get serverlist result on get serverlist
    /// </summary>
    public class GetServerListResult
    {
        public bool error { get; set; }
        public string message { get; set; }
        public List<Server> servers { get; set; }
    }
}