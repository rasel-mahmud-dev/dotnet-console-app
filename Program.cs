using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace HelloApp
{
    class Program
    {
        private const string X11Library = "libX11.so.6";

        // Import necessary X11 functions
        [DllImport(X11Library)]
        private static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport(X11Library)]
        private static extern int XCloseDisplay(IntPtr display);

        [DllImport(X11Library)]
        private static extern int XWarpPointer(IntPtr display, IntPtr srcWindow, IntPtr destWindow, int srcX, int srcY,
            uint srcWidth, uint srcHeight, int destX, int destY);

        [DllImport(X11Library)]
        private static extern int XFlush(IntPtr display);

        [DllImport(X11Library)]
        private static extern int XSelectInput(IntPtr display, IntPtr window, long event_mask);

        [DllImport(X11Library)]
        private static extern int XNextEvent(IntPtr display, out XEvent ev);

        [DllImport(X11Library)]
        private static extern IntPtr XDefaultRootWindow(IntPtr display);

        [DllImport(X11Library)]
        private static extern IntPtr XDisplayString(IntPtr display);

        // Define the event structure for handling mouse events
        [StructLayout(LayoutKind.Sequential)]
        private struct XEvent
        {
            public IntPtr type; // Event type
            public XButtonEvent xbutton; // Mouse button event structure
        }

        // Define the XButtonEvent structure for button press/release
        [StructLayout(LayoutKind.Sequential)]
        private struct XButtonEvent
        {
            public IntPtr window;    // Window where the event occurred
            public uint button;      // Button number (e.g., 1 = left click, 2 = middle, 3 = right click)
            public int x, y;         // Position of the mouse when the event occurred
            public uint state;       // Key mask (state of modifier keys)
        }

        static void Main(string[] args)
        {
            IntPtr display = XOpenDisplay(IntPtr.Zero);
            if (display == IntPtr.Zero)
            {
                Console.WriteLine("Unable to open X11 display.");
                return;
            }

            // Display some information about the X11 display connection
            string displayString = Marshal.PtrToStringAnsi(XDisplayString(display));
            Console.WriteLine($"Connected to X11 display: {displayString}");

            // Get the root window
            IntPtr rootWindow = XDefaultRootWindow(display);
            if (rootWindow == IntPtr.Zero)
            {
                Console.WriteLine("Unable to get the root window.");
                return;
            }

            // Select the input events you want to listen to (ButtonPress and ButtonRelease)
            XSelectInput(display, rootWindow, 0x500);  // 0x500 is ButtonPress | ButtonRelease
            Console.WriteLine("Listening for mouse click events (ButtonPress and ButtonRelease)...");

            // Infinite loop to capture and process events
            while (true)
            {
                XEvent ev;
                int result = XNextEvent(display, out ev);

                if (result != 0)
                {
                    Console.WriteLine("Error occurred while fetching the event.");
                    continue;
                }

                // Debugging: print out the event type
                Console.WriteLine($"Received event type: {ev.type}");

                if (ev.type == (IntPtr)4) // Button press event (type 4)
                {
                    Console.Clear();
                    Console.WriteLine($"Mouse clicked at ({ev.xbutton.x}, {ev.xbutton.y})");
                    Console.WriteLine($"Button clicked: {ev.xbutton.button}");
                    Console.WriteLine($"State of modifier keys: {ev.xbutton.state}");

                    // Example: Move the mouse to coordinates (200, 200) on left click
                    if (ev.xbutton.button == 1) // Left mouse click (Button 1)
                    {
                        XWarpPointer(display, rootWindow, rootWindow, 0, 0, 0, 0, 200, 200);
                        XFlush(display);
                        Console.WriteLine("Mouse moved to (200, 200).");
                    }
                }
                else if (ev.type == (IntPtr)5) // Button release event (type 5)
                {
                    Console.Clear();   
                    Console.WriteLine($"Mouse button released at ({ev.xbutton.x}, {ev.xbutton.y})");
                    Console.WriteLine($"Button released: {ev.xbutton.button}");
                }
                else
                {
                    // Print out other event types for debugging
                    Console.WriteLine($"Other event type detected: {ev.type}");
                }

                XFlush(display);
                Thread.Sleep(10); // Prevents blocking the main thread
            }

            XCloseDisplay(display);
        }
    }
}
