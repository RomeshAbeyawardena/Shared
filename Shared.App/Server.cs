using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DotNetInsights.Shared.App
{
    class Server : IDisposable
    {
        private readonly TcpListener tcpListener;
        public bool IsRunning;
        public Server()
        {
            tcpListener = new TcpListener(IPAddress.Any, 4999);
            tcpListener.Start();
            IsRunning = true;
        }

        public async Task ListenAsync(Action<TcpClient> onAcceptTcpClient)
        {
            while (IsRunning)
            {
                if(tcpListener.Pending())
                    onAcceptTcpClient(await tcpListener.AcceptTcpClientAsync().ConfigureAwait(false));
            }
        }

        public void Dispose()
        {
            IsRunning = false;
            tcpListener.Stop();
        }
    }
}
