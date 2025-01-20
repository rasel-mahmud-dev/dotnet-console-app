using System;
using System.Runtime.InteropServices;

namespace HelloApp{
    class Program{
        private const string X11Library = "libX11.so.6";

        [DllImport(X11Library)]
        private static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport(X11Library)]
        private static extern int XCloseDisplay(IntPtr display);

        [DllImport(X11Library)]
        private static extern int XWarpPointer(IntPtr display, IntPtr srcWindow, IntPtr destWindow, int srcX, int srcY,
            uint srcWidth, uint srcHeight, int destX, int destY);

        [DllImport(X11Library)]
        private static extern int XFlush(IntPtr display);

        static void Main(string[] args){
            IntPtr display = XOpenDisplay(IntPtr.Zero);
            if (display == IntPtr.Zero){
                Console.WriteLine("Unable to open X11 display.");
                return;
            }

            while (true){
                // Example: Move the mouse to coordinates (100, 100)
                XWarpPointer(display, IntPtr.Zero, IntPtr.Zero, 0, 0, 0, 0, 100, 100);
                XFlush(display);
                Console.WriteLine("Mouse moved to (100, 100).");
                Thread.Sleep(1000);
            }

            XCloseDisplay(display);
        }
    }
}