using System.Net.Sockets;
using System.Text;

namespace HelloApp{
    class TcpClientApp{
        public static void Run(string[] args){
            // Server address and port to connect to
            string serverAddress = "127.0.0.1"; // Localhost for testing
            int serverPort = 12345; // Example port

            // Create TCP client
            TcpClient client = new TcpClient();

            try{
                // Connect to the server
                client.Connect(serverAddress, serverPort);
                Console.WriteLine("Connected to the server.");

                // Get network stream to send and receive data
                NetworkStream stream = client.GetStream();

                while (true) // Keep the connection alive
                {
                    // Prepare the message to send
                    byte[] messageBytes = Encoding.ASCII.GetBytes("Hello from TCP Client!");

                    // Send the message to the server
                    stream.Write(messageBytes, 0, messageBytes.Length);

                    Console.WriteLine("Message sent: " + messageBytes.Length);


                    // Read the server's response
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string serverResponse = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    Console.WriteLine("Server response: " + serverResponse);

                    // Wait for user input to send another message or exit
                    Console.WriteLine("\nPress 'Enter' to send another message, or type 'exit' to quit.");
                    string input = Console.ReadLine();
                    if (input.ToLower() == "exit"){
                        break;
                    }

                    byte[] messageBytes2 = Encoding.ASCII.GetBytes(input);
                    stream.Write(messageBytes2, 0, messageBytes2.Length);

                    Console.WriteLine("Input received: " + input);
                }
            }
            catch (Exception ex){
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally{
                // Close the connection
                client.Close();
                Console.WriteLine("Connection closed.");
            }
        }
    }
}