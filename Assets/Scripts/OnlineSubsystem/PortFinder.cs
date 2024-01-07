using System.Net;
using System.Net.Sockets;

namespace OnlineSubsystem
{
    public class PortFinder
    {
        /// <summary>
        /// https://gist.github.com/jrusbatch/4211535?permalink_comment_id=3437695#gistcomment-3437695
        /// Finds an available port
        /// If you want to open empty port, you should let the system do the work for you:
        /// Because... another process may open the port returning by GetAvailablePort() function, before you.
        /// </summary>
        /// <returns></returns>
        public static int Find()
        {
            var udp = new UdpClient(0, AddressFamily.InterNetwork);
            int port = ((IPEndPoint)udp.Client.LocalEndPoint).Port;
            return port;
        }
    }
}