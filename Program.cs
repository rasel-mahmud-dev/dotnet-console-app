namespace HelloApp{
    class Program{
        static void Main(string[] args){
            var mouseTracker = new MouseTracker();
            mouseTracker.StartTracking();

            Console.WriteLine("Press Ctrl+C to exit.");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}g