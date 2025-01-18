namespace HelloApp{
    class App{
        static void Main(string[] args){
            Console.WriteLine("Welcome to your C# Console Application!");

            // Configure the server with IP and port
            TcpServer server = new TcpServer("127.0.0.1", 5000);

            // Start the server
            server.Start();

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            // Stop the server
            server.Stop();
        }
    }
}