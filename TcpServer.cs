using System.Net;
using System.Net.Sockets;
using System.Text;


namespace HelloApp{
    public class TcpServer{
        private TcpListener _server;
        private Thread _listenerThread;
        private bool _isRunning;

        public string IpAddress{ get; set; }
        public int Port{ get; set; }

        public TcpServer(string ipAddress, int port){
            IpAddress = ipAddress;
            Port = port;
        }

        public void Start(){
            _server = new TcpListener(IPAddress.Parse(IpAddress), Port);
            _server.Start();
            _isRunning = true;

            Console.WriteLine($"Server started at {IpAddress}:{Port}");
            _listenerThread = new Thread(ListenForClients);
            _listenerThread.Start();
        }

        public void Stop(){
            _isRunning = false;
            _server.Stop();
            _listenerThread.Abort();
            Console.WriteLine("Server stopped.");
        }

        private void ListenForClients(){
            while (_isRunning){
                try{
                    TcpClient client = _server.AcceptTcpClient();
                    Console.WriteLine("Client connected!");

                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
                catch (Exception ex){
                    if (_isRunning){
                        Console.WriteLine($"Error accepting client: {ex.Message}");
                    }
                }
            }
        }

        private void HandleClient(TcpClient client){
            NetworkStream stream = client.GetStream();

            // Send a welcome message
            string welcomeMessage = "Welcome to the TCP Server!";
            byte[] welcomeBytes = Encoding.ASCII.GetBytes(welcomeMessage);
            stream.Write(welcomeBytes, 0, welcomeBytes.Length);

            byte[] buffer = new byte[1024];
            int bytesRead;

            while (true){
                try{
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;

                    string clientMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received from client: {clientMessage}");

                    // Echo the message back to the client
                    stream.Write(buffer, 0, bytesRead);
                }
                catch (Exception ex){
                    Console.WriteLine($"Error handling client: {ex.Message}");
                    break;
                }
            }

            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}