using System.Net.Sockets;
using System.Text;

namespace HelloApp{
    class TcpClientApp{
        public static void Run(string[] args){
            string serverAddress = "127.0.0.1";
            int serverPort = 12345;

            Console.WriteLine("Starting server...");

            TcpClient client = new TcpClient();

            try{
                client.Connect(serverAddress, serverPort);
                Console.WriteLine("Connected to the server.");

                NetworkStream stream = client.GetStream();

                while (true){
                    Console.WriteLine("\nPress 'Enter' to send another message, or type 'exit' to quit.");
                    string input = Console.ReadLine();
                    if (input.ToLower() == "exit"){
                        break;
                    }

                    byte[] messageBytes2 = Encoding.ASCII.GetBytes(input + "\n");
                    stream.Write(messageBytes2, 0, messageBytes2.Length);

                    Console.WriteLine("Input received: " + input);
                }
            }
            catch (Exception ex){
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally{
                client.Close();
                Console.WriteLine("Connection closed.");
            }
        }
        
        public static NetworkStream StartTcpClient(string serverAddress, int serverPort){
            TcpClient client = new TcpClient();

            try{
                client.Connect(serverAddress, serverPort);
                Console.WriteLine("Connected to the server.");
                return client.GetStream();
            }
            catch (Exception ex){
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }
    }
}