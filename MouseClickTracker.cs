using System;
using System.Runtime.InteropServices;

namespace HelloApp{
    public class MouseClickTracker{
        [DllImport("libX11.so")]
        private static extern IntPtr XOpenDisplay(string display);

        [DllImport("libX11.so")]
        private static extern int XSelectInput(IntPtr display, IntPtr window, long eventMask);

        [DllImport("libX11.so")]
        private static extern int XNextEvent(IntPtr display, out XEvent ev);

        [DllImport("libX11.so")]
        private static extern int XCloseDisplay(IntPtr display);

        private const long ButtonPressMask = 1 << 2;

        [StructLayout(LayoutKind.Sequential)]
        private struct XEvent{
            public int type;
            public XButtonEvent buttonEvent;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct XButtonEvent{
            public IntPtr window;
            public int x, y;
        }

        public void StartTracking(IntPtr targetWindow){
            IntPtr display = XOpenDisplay(null);
            if (display == IntPtr.Zero){
                Console.WriteLine("Failed to open X display. Exiting.");
                return;
            }

            if (XSelectInput(display, targetWindow, ButtonPressMask) != 0){
                Console.WriteLine("Failed to select input on the target window.");
                XCloseDisplay(display);
                return;
            }

            Console.WriteLine("Tracking mouse clicks. Press Ctrl+C to exit.");
            while (true){
                XEvent ev;
                if (XNextEvent(display, out ev) == 0 && ev.type == 4) // ButtonPress
                {
                    Console.WriteLine($"Mouse Click Detected at ({ev.buttonEvent.x}, {ev.buttonEvent.y}).");
                }
            }
        }
    }
}