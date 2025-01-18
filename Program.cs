using System.Runtime.InteropServices;

namespace HelloApp{
    class Program{
        [DllImport("libX11.so")]
        private static extern IntPtr XOpenDisplay(string display);

        [DllImport("libX11.so")]
        private static extern int XQueryPointer(
            IntPtr display, IntPtr window,
            out IntPtr root_return, out IntPtr child_return,
            out int root_x, out int root_y,
            out int win_x, out int win_y,
            out uint mask_return
        );

        [DllImport("libX11.so")]
        private static extern IntPtr XDefaultRootWindow(IntPtr display);

        [DllImport("libX11.so")]
        private static extern int XCloseDisplay(IntPtr display);

        static void Main(string[] args){
            IntPtr display = XOpenDisplay(null);
            if (display == IntPtr.Zero){
                Console.WriteLine("Unable to open display. Are you running a graphical environment?");
                return;
            }

            IntPtr rootWindow = XDefaultRootWindow(display);
            Console.WriteLine("Tracking mouse movement. Press Ctrl+C to exit.");

            while (true){
                if (XQueryPointer(display, rootWindow, out _, out _, out int rootX, out int rootY, out _, out _,
                        out _) != 0){
                    Console.Clear();
                    Console.WriteLine($"Mouse Position: X = {rootX}, Y = {rootY}");
                }

                Thread.Sleep(100); // Delay to prevent excessive CPU usage
            }

            XCloseDisplay(display);
        }
    }
}