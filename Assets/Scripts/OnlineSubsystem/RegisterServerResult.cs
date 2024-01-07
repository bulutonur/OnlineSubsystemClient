namespace OnlineSubsystem
{
    /// <summary>
    /// A class for deserializing register server result on register server
    /// </summary>
    public class RegisterServerResult
    {
        public bool error { get; set; }
        public string message { get; set; }
        public int heartbeat { get; set; }
    }
}