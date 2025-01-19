using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace HelloApp;

public class MouseTracker{
    [DllImport("libX11.so")]
    private static extern IntPtr XOpenDisplay(string display);

    [DllImport("libX11.so")]
    private static extern IntPtr XDefaultRootWindow(IntPtr display);

    [DllImport("libX11.so")]
    private static extern int XGrabPointer(
        IntPtr display, IntPtr grabWindow, bool ownerEvents,
        uint eventMask, int pointerMode, int keyboardMode,
        IntPtr confineTo, IntPtr cursor, uint time
    );
    

    [DllImport("libX11.so")]
    private static extern int XUngrabPointer(IntPtr display, uint time);

    [DllImport("libX11.so")]
    private static extern int XQueryPointer(
        IntPtr display, IntPtr window,
        out IntPtr rootReturn, out IntPtr childReturn,
        out int rootX, out int rootY,
        out int winX, out int winY,
        out uint maskReturn
    );

    [DllImport("libX11.so")]
    private static extern int XCloseDisplay(IntPtr display);

    private const uint ButtonPressMask = 1 << 2;

    private IntPtr display;
    private IntPtr rootWindow;

    public MouseTracker(){
        display = XOpenDisplay(null);
        if (display == IntPtr.Zero){
            throw new InvalidOperationException("Unable to open display. Are you running a graphical environment?");
        }

        rootWindow = XDefaultRootWindow(display);
    }

    public void StartTracking(NetworkStream stream){
        Console.WriteLine("Tracking mouse position. Press Ctrl+C to exit.");
        while (true){
            if (XQueryPointer(display, rootWindow, out _, out _, out int rootX, out int rootY, out _, out _, out _) !=
                0){
                string positionMessage = $"cur:{rootX},{rootY}";
                Console.WriteLine(positionMessage);

                if (stream.CanWrite){
                    byte[] messageBytes = Encoding.ASCII.GetBytes(positionMessage + "\n");
                    stream.Write(messageBytes, 0, messageBytes.Length);
                }
            }

            Thread.Sleep(500);
        }
    }

    // public void StartTracking(){
    //     Console.WriteLine("Mouse clicks are now blocked. Press Ctrl+C to exit.");
    //
    //     // Grab the pointer to block mouse clicks
    //     int grabResult = XGrabPointer(
    //         display,
    //         rootWindow,
    //         false,
    //         ButtonPressMask,
    //         1,
    //         1,
    //         IntPtr.Zero,
    //         IntPtr.Zero,
    //         0
    //     );
    //
    //     if (grabResult != 0){
    //         Console.WriteLine("Failed to grab the pointer. Exiting.");
    //         XCloseDisplay(display);
    //         return;
    //     }
    //
    //     // Infinite loop to track cursor position and block clicks
    //     while (true){
    //         if (XQueryPointer(display, rootWindow, out _, out _, out int rootX, out int rootY, out _, out _, out _) != 0){
    //             Console.Clear();
    //             Console.WriteLine($"Mouse Position: X = {rootX}, Y = {rootY}");
    //             Console.WriteLine("Mouse clicks are blocked. Press Ctrl+C to exit.");
    //         }
    //
    //         Thread.Sleep(100);
    //     }
    // }

    public void StopTracking(){
        // Ungrab the pointer when done
        XUngrabPointer(display, 0);
        XCloseDisplay(display);
    }
}