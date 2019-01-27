using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BadgerBadgerBadger
{
    public static class NativeMethods
    {
        #region Const

        /// <summary>
        /// Public Constants for <see cref="FindWindow(string, string)"/>
        /// </summary>
        public struct NAMES
        {
            /// <summary>
            /// Class of the Taskbar Window
            /// </summary>
            public const string Taskbar_Class = "Shell_TrayWnd";
            /// <summary>
            /// Name of the taskbar Window
            /// </summary>
            public const string Taskbar_Window = null;
        }

        /// <summary>
        /// Child Id for WinEventDelegate
        /// </summary>
        private const int CHILDID_SELF = 0;
        /// <summary>
        /// Set text of a control
        /// </summary>
        private const int WM_SETTEXT = 0xC;
        /// <summary>
        /// Get text of a control
        /// </summary>
        private const uint WM_GETTEXT = 0xD;
        /// <summary>
        /// Get text length (in chars, not bytes) of a control
        /// </summary>
        private const uint WM_GETTEXTLENGTH = 0xE;
        /// <summary>
        /// Indicates an active control
        /// </summary>
        private const uint WS_ACTIVECAPTION = 0x1;

        #endregion

        #region Events

        /// <summary>
        /// Handler for Event that has a Pointer
        /// </summary>
        /// <param name="ptr">Pointer</param>
        /// <remarks>The meaning of the pointer is event specific</remarks>
        public delegate void IntPtrHandler(IntPtr ptr);

        /// <summary>
        /// Hook function for the Windows Global Event Hook
        /// </summary>
        /// <param name="hWinEventHook">Event Hook Handle</param>
        /// <param name="eventType">Event Type</param>
        /// <param name="hwnd">Event specific Handle</param>
        /// <param name="idObject">Id associated with an Object</param>
        /// <param name="idChild">
        /// Identifies whether the event was triggered by an object
        /// or a child element of the object.
        /// If this value is CHILDID_SELF, the event was triggered by the object;
        /// otherwise, this value is the child ID of the element that triggered the event.
        /// </param>
        /// <param name="dwEventThread">ID of the thread that generated the event</param>
        /// <param name="dwmsEventTime">Specifies the time, in milliseconds, that the event was generated.</param>
        private delegate void WinEventDelegate(IntPtr hWinEventHook, EventConst eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        /// <summary>
        /// Foreground Window Change Event
        /// </summary>
        public static event IntPtrHandler ForegroundWindowChange;
        /// <summary>
        /// Focused Control Change Event
        /// </summary>
        public static event IntPtrHandler FocusChange;
        /// <summary>
        /// Control Value Change Event
        /// </summary>
        public static event IntPtrHandler ValueChange;

        #endregion

        #region Enums

        /// <summary>
        /// Contains event constants. Some can be flagged, some can't
        /// </summary>
        private enum EventConst : uint
        {
            /// <summary>
            /// Event is async
            /// </summary>
            WINEVENT_OUTOFCONTEXT = 0x0000,
            /// <summary>
            /// Don't call back for events on installer's thread
            /// </summary>
            WINEVENT_SKIPOWNTHREAD = 0x0001,
            /// <summary>
            /// Don't call back for events on installer's process
            /// </summary>
            WINEVENT_SKIPOWNPROCESS = 0x0002,
            /// <summary>
            /// Events are SYNC, this causes your dll to be injected into every process
            /// </summary>
            WINEVENT_INCONTEXT = 0x0004,
            /// <summary>
            /// Minimum possible event Id
            /// </summary>
            EVENT_MIN = 0x00000001,
            /// <summary>
            /// Maximum possible event Id
            /// </summary>
            EVENT_MAX = 0x7FFFFFFF,
            /// <summary>
            /// System sound trigger
            /// </summary>
            EVENT_SYSTEM_SOUND = 0x0001,
            /// <summary>
            /// System alert trigger
            /// </summary>
            EVENT_SYSTEM_ALERT = 0x0002,
            /// <summary>
            /// Foreground change
            /// </summary>
            EVENT_SYSTEM_FOREGROUND = 0x0003,
            /// <summary>
            /// Menu start event (Not startmenu event)
            /// </summary>
            EVENT_SYSTEM_MENUSTART = 0x0004,
            /// <summary>
            /// Menu end event
            /// </summary>
            EVENT_SYSTEM_MENUEND = 0x0005,
            /// <summary>
            /// Popup menu start
            /// </summary>
            EVENT_SYSTEM_MENUPOPUPSTART = 0x0006,
            /// <summary>
            /// Popup menu end
            /// </summary>
            EVENT_SYSTEM_MENUPOPUPEND = 0x0007,
            /// <summary>
            /// Capture start
            /// </summary>
            EVENT_SYSTEM_CAPTURESTART = 0x0008,
            /// <summary>
            /// Capture end
            /// </summary>
            EVENT_SYSTEM_CAPTUREEND = 0x0009,
            /// <summary>
            /// Move or resize start
            /// </summary>
            EVENT_SYSTEM_MOVESIZESTART = 0x000A,
            /// <summary>
            /// Move or resize end
            /// </summary>
            EVENT_SYSTEM_MOVESIZEEND = 0x000B,
            /// <summary>
            /// Context help started
            /// </summary>
            EVENT_SYSTEM_CONTEXTHELPSTART = 0x000C,
            /// <summary>
            /// Context help ended
            /// </summary>
            EVENT_SYSTEM_CONTEXTHELPEND = 0x000D,
            /// <summary>
            /// Drag and drop operation started
            /// </summary>
            EVENT_SYSTEM_DRAGDROPSTART = 0x000E,
            /// <summary>
            /// Drag and drop operation ended
            /// </summary>
            EVENT_SYSTEM_DRAGDROPEND = 0x000F,
            /// <summary>
            /// Dialog started
            /// </summary>
            EVENT_SYSTEM_DIALOGSTART = 0x0010,
            /// <summary>
            /// Dialog ended
            /// </summary>
            EVENT_SYSTEM_DIALOGEND = 0x0011,
            /// <summary>
            /// Scrolling started
            /// </summary>
            EVENT_SYSTEM_SCROLLINGSTART = 0x0012,
            /// <summary>
            /// Scrolling ended
            /// </summary>
            EVENT_SYSTEM_SCROLLINGEND = 0x0013,
            /// <summary>
            /// ?
            /// </summary>
            EVENT_SYSTEM_SWITCHSTART = 0x0014,
            /// <summary>
            /// ?
            /// </summary>
            EVENT_SYSTEM_SWITCHEND = 0x0015,
            /// <summary>
            /// Minimized
            /// </summary>
            EVENT_SYSTEM_MINIMIZESTART = 0x0016,
            /// <summary>
            /// Minimized
            /// </summary>
            EVENT_SYSTEM_MINIMIZEEND = 0x0017,
            /// <summary>
            /// Active desktop switch
            /// </summary>
            EVENT_SYSTEM_DESKTOPSWITCH = 0x0020,
            /// <summary>
            /// End of system defined event list
            /// </summary>
            EVENT_SYSTEM_END = 0x00FF,
            /// <summary>
            /// First Id for OEM events
            /// </summary>
            EVENT_OEM_DEFINED_START = 0x0101,
            /// <summary>
            /// Last Id for OEM events
            /// </summary>
            EVENT_OEM_DEFINED_END = 0x01FF,
            /// <summary>
            /// First ID for UIA events
            /// </summary>
            EVENT_UIA_EVENTID_START = 0x4E00,
            /// <summary>
            /// Last ID for UIA events
            /// </summary>
            EVENT_UIA_EVENTID_END = 0x4EFF,
            /// <summary>
            /// First Id for UIA proprty
            /// </summary>
            EVENT_UIA_PROPID_START = 0x7500,
            /// <summary>
            /// Last Id for UIA property
            /// </summary>
            EVENT_UIA_PROPID_END = 0x75FF,
            /// <summary>
            /// Console caret change
            /// </summary>
            EVENT_CONSOLE_CARET = 0x4001,
            /// <summary>
            /// Console region updated
            /// </summary>
            EVENT_CONSOLE_UPDATE_REGION = 0x4002,
            /// <summary>
            /// Simple console update
            /// </summary>
            EVENT_CONSOLE_UPDATE_SIMPLE = 0x4003,
            /// <summary>
            /// Console scrolled
            /// </summary>
            EVENT_CONSOLE_UPDATE_SCROLL = 0x4004,
            /// <summary>
            /// Console layout change
            /// </summary>
            EVENT_CONSOLE_LAYOUT = 0x4005,
            /// <summary>
            /// Console application started
            /// </summary>
            EVENT_CONSOLE_START_APPLICATION = 0x4006,
            /// <summary>
            /// Console application ended
            /// </summary>
            EVENT_CONSOLE_END_APPLICATION = 0x4007,
            /// <summary>
            /// Console event end
            /// </summary>
            EVENT_CONSOLE_END = 0x40FF,
            /// <summary>
            /// Object created
            /// </summary>
            /// <remarks>hwnd ID idChild is created item</remarks>
            EVENT_OBJECT_CREATE = 0x8000,
            /// <summary>
            /// Object destroyed
            /// </summary>
            /// <remarks>hwnd ID idChild is destroyed item</remarks>
            EVENT_OBJECT_DESTROY = 0x8001,
            /// <summary>
            /// Object shown
            /// </summary>
            /// <remarks>hwnd ID idChild is shown item</remarks>
            EVENT_OBJECT_SHOW = 0x8002,
            /// <summary>
            /// Object hidden
            /// </summary>
            /// <remarks>hwnd ID idChild is hidden item</remarks>
            EVENT_OBJECT_HIDE = 0x8003,
            /// <summary>
            /// Object z-order change
            /// </summary>
            /// <remarks>hwnd ID idChild is parent of zordering children</remarks>
            EVENT_OBJECT_REORDER = 0x8004,
            /// <summary>
            /// Object focus change
            /// </summary>
            /// <remarks>hwnd ID idChild is focused item</remarks>
            EVENT_OBJECT_FOCUS = 0x8005,
            /// <summary>
            /// Object selection changed
            /// </summary>
            /// <remarks>hwnd ID idChild is selected item (if only one), or idChild is OBJID_WINDOW if complex</remarks>
            EVENT_OBJECT_SELECTION = 0x8006,
            /// <summary>
            /// Item added to selection
            /// </summary>
            /// <remarks>hwnd ID idChild is item added</remarks>
            EVENT_OBJECT_SELECTIONADD = 0x8007,
            /// <summary>
            /// Item removed from selection
            /// </summary>
            /// <remarks>hwnd ID idChild is item removed</remarks>
            EVENT_OBJECT_SELECTIONREMOVE = 0x8008,
            /// <summary>
            /// Selected items changed
            /// </summary>
            /// <remarks>hwnd ID idChild is parent of changed selected items</remarks>
            EVENT_OBJECT_SELECTIONWITHIN = 0x8009,
            /// <summary>
            /// Item state changed
            /// </summary>
            /// <remarks>hwnd ID idChild is item w/ state change</remarks>
            EVENT_OBJECT_STATECHANGE = 0x800A,
            /// <summary>
            /// Item position/size change
            /// </summary>
            /// <remarks>hwnd ID idChild is moved/sized item</remarks>
            EVENT_OBJECT_LOCATIONCHANGE = 0x800B,
            /// <summary>
            /// Name of item changed
            /// </summary>
            /// <remarks>hwnd ID idChild is item w/ name change</remarks>
            EVENT_OBJECT_NAMECHANGE = 0x800C,
            /// <summary>
            /// Description of item changed
            /// </summary>
            /// <remarks>hwnd ID idChild is item w/ desc change</remarks>
            EVENT_OBJECT_DESCRIPTIONCHANGE = 0x800D,
            /// <summary>
            /// Object value changed
            /// </summary>
            /// <remarks>hwnd ID idChild is item w/ value change</remarks>
            EVENT_OBJECT_VALUECHANGE = 0x800E,
            /// <summary>
            /// Parent of item changed
            /// </summary>
            /// <remarks>hwnd ID idChild is item w/ new parent</remarks>
            EVENT_OBJECT_PARENTCHANGE = 0x800F,
            /// <summary>
            /// Help on item changed
            /// </summary>
            /// <remarks>hwnd ID idChild is item w/ help change</remarks>
            EVENT_OBJECT_HELPCHANGE = 0x8010,
            /// <summary>
            /// Default action changed
            /// </summary>
            /// <remarks>hwnd ID idChild is item w/ def action change</remarks>
            EVENT_OBJECT_DEFACTIONCHANGE = 0x8011,
            /// <summary>
            /// Keyboard accelerator changed
            /// </summary>
            /// <remarks>hwnd ID idChild is item w/ keybd accel change</remarks>
            EVENT_OBJECT_ACCELERATORCHANGE = 0x8012,
            /// <summary>
            /// Item invoked
            /// </summary>
            /// <remarks>hwnd ID idChild is item invoked</remarks>
            EVENT_OBJECT_INVOKED = 0x8013,
            /// <summary>
            /// Text selection changed
            /// </summary>
            /// <remarks>hwnd ID idChild is item w/ test selection change</remarks>
            EVENT_OBJECT_TEXTSELECTIONCHANGED = 0x8014,
            /// <summary>
            /// Content scrolled
            /// </summary>
            EVENT_OBJECT_CONTENTSCROLLED = 0x8015,
            /// <summary>
            /// ?
            /// </summary>
            EVENT_SYSTEM_ARRANGMENTPREVIEW = 0x8016,
            /// <summary>
            /// End of system defined object IDs
            /// </summary>
            EVENT_OBJECT_END = 0x80FF,
            /// <summary>
            /// Start of custom AIA Ids
            /// </summary>
            EVENT_AIA_START = 0xA000,
            /// <summary>
            /// End of custom AIA Ids
            /// </summary>
            EVENT_AIA_END = 0xAFFF
        }

        /// <summary>
        /// GetWindow Commands
        /// </summary>
        private enum GetWindowCommand : uint
        {
            /// <summary>
            /// First object match
            /// </summary>
            GW_HWNDFIRST = 0,
            /// <summary>
            /// Last object match
            /// </summary>
            GW_HWNDLAST = 1,
            /// <summary>
            /// Next object match
            /// </summary>
            GW_HWNDNEXT = 2,
            /// <summary>
            /// Previous object match
            /// </summary>
            GW_HWNDPREV = 3,
            /// <summary>
            /// Get Owner
            /// </summary>
            GW_OWNER = 4,
            /// <summary>
            /// Get child
            /// </summary>
            GW_CHILD = 5,
            /// <summary>
            /// Get enabled popup window
            /// </summary>
            GW_ENABLEDPOPUP = 6
        }

        /// <summary>
        /// GetAncestor Command
        /// </summary>
        public enum GetAncestorCommand : uint
        {
            /// <summary>
            /// Retrieves the parent window.
            /// This does not include the owner,
            /// as it does with the GetParent function.
            /// </summary>
            GA_PARENT = 1,
            /// <summary>
            /// Retrieves the root window by walking the chain of parent windows.
            /// </summary>
            GA_ROOT = 2,
            /// <summary>
            /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
            /// </summary>
            GA_ROOTOWNER = 3
        }

        #endregion

        #region Imports

        /// <summary>
        /// Gets the class name of a window or control
        /// </summary>
        /// <param name="hWnd">Handle to window/control</param>
        /// <param name="StrPtr">Pointer to allocated memory</param>
        /// <param name="MaxCount">Size of Memory</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, IntPtr StrPtr, int MaxCount);

        /// <summary>
        /// Find a window
        /// </summary>
        /// <param name="lpClassName">Window Class name</param>
        /// <param name="lpWindowName">Window Name (if null is ignored)</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowW(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpClassName,
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpWindowName);

        /// <summary>
        /// Gets the owner of a control/window
        /// </summary>
        /// <param name="hWnd">Control/Window handle</param>
        /// <returns>Owner Handle</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetOwner(IntPtr hWnd);

        /// <summary>
        /// Gets ancestor of a window/control
        /// </summary>
        /// <param name="hWnd">Window/Control Handle</param>
        /// <param name="cmd">Ancestor Command</param>
        /// <returns>Ancestor Handle</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetAncestor(IntPtr hWnd, GetAncestorCommand cmd);

        /// <summary>
        /// Gets the Thread Id of the Message Loop
        /// </summary>
        /// <param name="hWnd">Window Handle</param>
        /// <param name="ProcessId">Process Id (Scans all processes if 0)</param>
        /// <returns>Thread Id</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        /// <summary>
        /// Attach custom function as Thread Input
        /// </summary>
        /// <param name="idAttach">Thread Id to attach</param>
        /// <param name="idAttachTo">Thread Id to attach to</param>
        /// <param name="fAttach">Attach/Remove</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, bool fAttach);

        /// <summary>
        /// Gets the currently focused control
        /// </summary>
        /// <returns>Focused control handle</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetFocus();

        /// <summary>
        /// Finds a Window
        /// </summary>
        /// <param name="hWndParent">Parent Window, 0=all</param>
        /// <param name="hWndChildAfter">Finds Child window after the given window, 0=first</param>
        /// <param name="lpszClass">Window Type, null=any</param>
        /// <param name="lpszWindow">Window Title, null=any</param>
        /// <returns>Window Handle, 0=error</returns>
        [DllImport("User32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// Gets currently active window of the entire user session
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Sends a message to another application
        /// </summary>
        /// <param name="hWnd">Main Window Handle</param>
        /// <param name="Msg">Message to send</param>
        /// <param name="wParam">Message Param 1</param>
        /// <param name="lParam">Message Param 2</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sets a global event hook
        /// </summary>
        /// <param name="eventMin">Minimum Event Id to capture</param>
        /// <param name="eventMax">Maximum Event Id to capture</param>
        /// <param name="hmodWinEventProc">DLL Handle (0 if WINEVENT_OUTOFCONTEXT is used in last param)</param>
        /// <param name="lpfnWinEventProc">Function to capture Events</param>
        /// <param name="idProcess">Process Id to capture events of (0=All)</param>
        /// <param name="idThread">Thread Id to capture events of (0=All)</param>
        /// <param name="dwFlags">Capture Flags</param>
        /// <returns>Hook Handle (needed to unhook)</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(EventConst eventMin, EventConst eventMax,
            IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
            uint idThread, EventConst dwFlags);

        #endregion

        /// <summary>
        /// Find a window by class and/or window name
        /// </summary>
        /// <param name="ClassName">Class name</param>
        /// <param name="WindowName">Window name</param>
        /// <returns></returns>
        public static IntPtr FindWindow(string ClassName, string WindowName = null)
        {
            return FindWindowW(ClassName, WindowName);
        }

        /// <summary>
        /// Gest currently active window
        /// </summary>
        /// <returns>Window Handle</returns>
        public static IntPtr GetActiveWindow()
        {
            return GetForegroundWindow();
        }

        /// <summary>
        /// Gets class name of Window or Control handle
        /// </summary>
        /// <param name="hWnd">Handle</param>
        /// <returns>Class name (up to 512 characters)</returns>
        public static string GetHandleClassName(IntPtr hWnd)
        {
            using (var Ptr = new SafeMemoryHandle(512 * 2 + 2))
            {
                int ret = GetClassNameW(hWnd, Ptr.DangerousGetHandle(), 512);
                if (ret > 0)
                {
                    return Encoding.Unicode.GetString(Ptr.GetBytes(), 0, ret * 2);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets owner of a control/window
        /// </summary>
        /// <param name="hWnd">Control/Window handle</param>
        /// <returns>Handle of Owner. If Zero or same as passed in handle, no owner exists</returns>
        public static IntPtr GetControlOwner(IntPtr hWnd)
        {
            return GetAncestor(hWnd, GetAncestorCommand.GA_ROOTOWNER);
        }

        /// <summary>
        /// Gets the title/text of a control
        /// </summary>
        /// <param name="Window">Handle to control</param>
        /// <returns>Text of control</returns>
        public static string GetControlText(IntPtr Window)
        {
            var Len = SendMessage(Window, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
            if (Len < 1)
            {
                //No text in this control
                return string.Empty;
            }
            //Accomodate Null terminator
            ++Len;
            //Double up for unicode
            Len *= 2;
            //Allocate buffer for control text in C# and unmanaged memory
            var Dest = new byte[Len];
            using (var Ptr = new SafeMemoryHandle(Len))
            {
                //The only way this can be invalid is "out of memory" errors
                if (!Ptr.IsInvalid)
                {
                    //SendMessage returns number of CHARACTERS copied, not bytes.
                    //With unicode, this applies a factor of 2
                    //We also remove the last two bytes of Len to strip the null terminator
                    var Length = Math.Min(SendMessage(Window, WM_GETTEXT, (IntPtr)Len, Ptr.DangerousGetHandle()) * 2, Len - 2);
                    //Don't attempt to copy if no text received
                    if (Length > 0)
                    {
                        var Data = Ptr.GetBytes();
                        return Encoding.Unicode.GetString(Data, 0, Length);
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Sets the title/text of a control
        /// </summary>
        /// <param name="hWnd">Control/Window handle</param>
        /// <param name="Text">Text to set</param>
        public static void SetControlText(IntPtr hWnd, string Text)
        {
            if (Text == null)
            {
                Text = string.Empty;
            }
            var Data = Encoding.Unicode.GetBytes(Text + "\0");
            using (var Ptr = new SafeMemoryHandle(Data.Length))
            {
                Ptr.SetBytes(Data, 0, Data.Length);
                SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, Ptr.DangerousGetHandle());
            }
        }

        /// <summary>
        /// Gets a Child Control Handle
        /// </summary>
        /// <param name="hWnd">Parent Handle</param>
        /// <param name="PreviousControl">Previous Control,0=first</param>
        /// <returns></returns>
        public static IntPtr GetChild(IntPtr hWnd, IntPtr PreviousControl)
        {
            return FindWindowEx(hWnd, PreviousControl, null, null);
        }

        /*
        public static IntPtr GetFocusedControl(IntPtr SelfHandle)
        {
            return GetFocusedControl(SelfHandle, IntPtr.Zero);
        }

        public static IntPtr GetFocusedControl(IntPtr SelfHandle, IntPtr RemoteHandle)
        {
            IntPtr activeWindowHandle = RemoteHandle == IntPtr.Zero ? GetForegroundWindow() : RemoteHandle;

            IntPtr activeWindowThread = GetWindowThreadProcessId(activeWindowHandle, IntPtr.Zero);
            IntPtr thisWindowThread = GetWindowThreadProcessId(SelfHandle, IntPtr.Zero);

            AttachThreadInput(activeWindowThread, thisWindowThread, true);
            IntPtr focusedControlHandle = GetFocus();
            AttachThreadInput(activeWindowThread, thisWindowThread, false);
            //SetForegroundWindow(RemoteHandle);
            return focusedControlHandle;
        }
        //*/

        /// <summary>
        /// Enumerates all children of a control/window
        /// </summary>
        /// <param name="Parent">Main Handle</param>
        /// <param name="Recursive">Recursively evaluate handles</param>
        /// <returns>Handle list</returns>
        public static IEnumerable<IntPtr> GetAllChildren(IntPtr Parent, bool Recursive = false)
        {
            IntPtr Current = GetChild(Parent, IntPtr.Zero);
            while (Current != IntPtr.Zero)
            {
                yield return Current;
                if (Recursive)
                {
                    foreach (var ctrl in GetAllChildren(Current, true))
                    {
                        yield return ctrl;
                    }
                }
                Current = GetChild(Parent, Current);
            }
        }

        private static WinEventDelegate ForegroundChangeCallback;
        private static WinEventDelegate ObjectFocusCallback;
        private static WinEventDelegate ObjectValueChangeCallback;

        /// <summary>
        /// Static initializer for event hooks
        /// </summary>
        static NativeMethods()
        {
            ForegroundChangeCallback = new WinEventDelegate(WinEventProcCallback);
            ObjectFocusCallback = new WinEventDelegate(WinEventProcCallback);
            ObjectValueChangeCallback = new WinEventDelegate(WinEventProcCallback);

            //Active application window change
            SetWinEventHook(EventConst.EVENT_SYSTEM_FOREGROUND,
                EventConst.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero,
                ForegroundChangeCallback, 0, 0,
                EventConst.WINEVENT_OUTOFCONTEXT | EventConst.WINEVENT_SKIPOWNPROCESS);
            //Focus change handler
            SetWinEventHook(EventConst.EVENT_SYSTEM_FOREGROUND,
                EventConst.EVENT_OBJECT_FOCUS, IntPtr.Zero,
                ObjectFocusCallback, 0, 0,
                EventConst.WINEVENT_OUTOFCONTEXT | EventConst.WINEVENT_SKIPOWNPROCESS);
            //Value change handler
            SetWinEventHook(EventConst.EVENT_SYSTEM_FOREGROUND,
                EventConst.EVENT_OBJECT_VALUECHANGE, IntPtr.Zero,
                ObjectValueChangeCallback, 0, 0,
                EventConst.WINEVENT_OUTOFCONTEXT | EventConst.WINEVENT_SKIPOWNPROCESS);
        }

        /// <summary>
        /// Internal event hook processor.
        /// Params: <see cref="WinEventDelegate"/>
        /// </summary>
        private static void WinEventProcCallback(IntPtr hWinEventHook, EventConst eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (eventType == EventConst.EVENT_SYSTEM_FOREGROUND)
            {
                ForegroundWindowChange?.Invoke(hwnd);
            }
            else if (eventType == EventConst.EVENT_OBJECT_FOCUS)
            {
                FocusChange?.Invoke(hwnd);
            }
            else if (eventType == EventConst.EVENT_OBJECT_VALUECHANGE)
            {
                ValueChange?.Invoke(hwnd);
            }
        }
    }
}
