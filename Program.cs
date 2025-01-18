namespace HelloApp { 
    class Program {
        static void Main(string[] args){
            Console.WriteLine("Welcome to your C# Console Application!");

            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            Console.WriteLine($"Hello, {name}! How are you today?");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}