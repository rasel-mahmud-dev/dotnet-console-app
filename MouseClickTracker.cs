using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace HelloApp{
    public class MouseClickTracker{
        // Import X11 functions
        [DllImport("libX11.so")]
        private static extern IntPtr XOpenDisplay(string display);

        [DllImport("libX11.so")]
        private static extern IntPtr XDefaultRootWindow(IntPtr display);

        [DllImport("libX11.so")]
        private static extern int XNextEvent(IntPtr display, out XEvent ev);

        [DllImport("libX11.so")]
        private static extern int XPending(IntPtr display);

        [DllImport("libX11.so")]
        private static extern int XCloseDisplay(IntPtr display);

        private const uint ButtonPressMask = 1 << 2;  
        private const uint ButtonReleaseMask = 1 << 3; 

        private IntPtr display;
        private IntPtr rootWindow;

        [StructLayout(LayoutKind.Sequential)]
        private struct XEvent{
            public int type;
            public XButtonEvent buttonEvent;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct XButtonEvent{
            public IntPtr window;
            public int x;
            public int y;
            public int button;
            public uint state;
        }

        public void StartTracking(){
            display = XOpenDisplay(null);
            if (display == IntPtr.Zero){
                Console.WriteLine("Failed to open X display. Exiting.");
                return;
            }

            rootWindow = XDefaultRootWindow(display);
            if (rootWindow == IntPtr.Zero){
                Console.WriteLine("Failed to get the root window. Exiting.");
                XCloseDisplay(display);
                return;
            }

            Console.WriteLine("Tracking mouse clicks. Press Ctrl+C to exit.");
            while (true){
                if (XPending(display) > 0){
                    XEvent ev;
                    if (XNextEvent(display, out ev) != 0){
                        Console.WriteLine("Error in XNextEvent.");
                        break;
                    }

                    if (ev.type == 4) // Button press event
                    {
                        Console.Clear();
                        Console.WriteLine("Mouse Click Detected!");
                        Console.WriteLine($"Click at position: ({ev.buttonEvent.x}, {ev.buttonEvent.y})");
                    }
                }

                Thread.Sleep(100); // Sleep to avoid overwhelming the CPU
            }
        }

        public void StopTracking(){
            // Close the X display when done
            XCloseDisplay(display);
        }
    }
}