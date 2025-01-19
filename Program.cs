using System.Net.Sockets;
using System.Threading.Tasks;

namespace HelloApp{
    class Program{
        static async Task Main(string[] args){
            string serverAddress = "127.0.0.1";
            int serverPort = 12345;

            // var mouseTracker = new MouseTracker();
            var mouseClickTracker = new MouseClickTracker();

            try{
                // Start the TCP client and get the NetworkStream
                // NetworkStream stream = TcpClientApp.StartTcpClient(serverAddress, serverPort);

                // var mouseTrackerTask = Task.Run(() => mouseTracker.StartTracking(stream));
                var mouseClickTracker2 = Task.Run(() => mouseClickTracker.StartTracking());

                Console.WriteLine("Press Ctrl+C to exit.");
                // await mouseTrackerTask;
                await mouseClickTracker2;
            }
            catch (Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}