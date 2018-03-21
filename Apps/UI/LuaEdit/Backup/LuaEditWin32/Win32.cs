using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

namespace LuaEdit.Win32
{
    public static class Win32Utils
    {
        public static readonly int MAX_PATH = 260;

        /// <summary>
        /// Converts a path into a relative one
        /// </summary>
        /// <param name="relativeTo">The path from which to be relative to</param>
        /// <param name="absolutePath">The path to be converted</param>
        /// <returns>The relative path</returns>
        public static string GetRelativePath(string relativeTo, string absolutePath)
        {
            string[] absoluteDirectories = absolutePath.Split('\\');
            string[] relativeDirectories = relativeTo.Split('\\');
            
            //Get the shortest of the two paths
            int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

            //Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            //Find common root
            for (index = 0; index < length; index++)
            {
                if (absoluteDirectories[index] == relativeDirectories[index])
                    lastCommonRoot = index;
                else
                    break;
            }

            //If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
            {
                return relativeTo;
            }

            //Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            if (lastCommonRoot + 1 >= absoluteDirectories.Length - 1)
            {
                // Add on the '.'
                relativePath.Append(".\\");
            }
            else
            {
                //Add on the '..'
                for (index = lastCommonRoot + 1; index < absoluteDirectories.Length; index++)
                {
                    if (absoluteDirectories[index].Length > 0)
                        relativePath.Append("..\\");
                }
            }

            //Add on the folders
            for (index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; index++)
                relativePath.Append(relativeDirectories[index] + "\\");

            relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);

            return relativePath.ToString();        
        }

        /// <summary>
        /// Converts a path into an absolute one
        /// </summary>
        /// <param name="relativePath">The relative path to be converted</param>
        /// <param name="relativeTo">The full path from which the relative path is relative</param>
        /// <returns>The absolute path</returns>
        public static string GetAbsolutePath(string relativePath, string relativeTo)
        {
            string absolutePath = relativePath;

            if (!Path.IsPathRooted(relativePath))
            {
                string oldCurrentDir = Directory.GetCurrentDirectory();

                Directory.SetCurrentDirectory(Path.GetDirectoryName(relativeTo));
                absolutePath = Path.GetFullPath(relativePath);
                Directory.SetCurrentDirectory(oldCurrentDir);
            }

            return absolutePath;
        }

        /// <summary>
        /// Get the processes' owner user name from a process handle
        /// </summary>
        /// <param name="processHandle">The process' handle from which to retrieve the owner user name</param>
        /// <returns>The owner user name of that process</returns>
        public static string GetProcessOwner(IntPtr processHandle)
        {
            IntPtr tokenHandle = IntPtr.Zero;

            if (0 != AdvApi32.OpenProcessToken(processHandle, AdvApi32.TOKEN_QUERY, ref tokenHandle))
            {
                WindowsIdentity wi = new WindowsIdentity(tokenHandle);
                string ownerName = wi.Name;
                Kernel32.CloseHandle(tokenHandle);
                return ownerName;
            }

            return string.Empty;
        }
    }

    public static class IpHlpApi
    {
        public enum TCP_TABLE_CLASS : int
        {
            TCP_TABLE_BASIC_LISTENER,
            TCP_TABLE_BASIC_CONNECTIONS,
            TCP_TABLE_BASIC_ALL,
            TCP_TABLE_OWNER_PID_LISTENER,
            TCP_TABLE_OWNER_PID_CONNECTIONS,
            TCP_TABLE_OWNER_PID_ALL,
            TCP_TABLE_OWNER_MODULE_LISTENER,
            TCP_TABLE_OWNER_MODULE_CONNECTIONS,
            TCP_TABLE_OWNER_MODULE_ALL
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPROW_OWNER_PID
        {
            public uint state;
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public uint remoteAddr;
            public byte remotePort1;
            public byte remotePort2;
            public byte remotePort3;
            public byte remotePort4;
            public int owningPid;
            public ushort LocalPort
            {
                get { return BitConverter.ToUInt16(new byte[2] { localPort2, localPort1 }, 0); }
            }
            public ushort RemotePort
            {
                get { return BitConverter.ToUInt16(new byte[2] { remotePort2, remotePort1 }, 0); }
            }
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPTABLE_OWNER_PID
        {
            public uint dwNumEntries;
            MIB_TCPROW_OWNER_PID table;
        }
        
        [DllImport("iphlpapi.dll", SetLastError=true)]
        static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool sort, int ipVersion,
                                               TCP_TABLE_CLASS tblClass, int reserved);
        
        public static MIB_TCPROW_OWNER_PID[] GetAllTcpConnections()
        {
            MIB_TCPROW_OWNER_PID[] tTable;
            int AF_INET = 2;    // IP_v4
            int buffSize = 0;    // how much memory do we need?
            uint ret = GetExtendedTcpTable(IntPtr.Zero, ref buffSize, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
            
            if (ret != 0 && ret != 122)
            {
                // 122 insufficient buffer size
                throw new Exception("bad ret on check " + ret);
            }
            
            IntPtr buffTable = Marshal.AllocHGlobal(buffSize);
            try
            {
                ret = GetExtendedTcpTable(buffTable, ref buffSize, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
                
                if (ret != 0)
                {
                    throw new Exception("bad ret "+ ret);
                }
                
                // get the number of entries in the table
                MIB_TCPTABLE_OWNER_PID tab = (MIB_TCPTABLE_OWNER_PID)Marshal.PtrToStructure(buffTable, typeof(MIB_TCPTABLE_OWNER_PID));
                IntPtr rowPtr = (IntPtr)((long)buffTable + Marshal.SizeOf(tab.dwNumEntries));
                tTable = new MIB_TCPROW_OWNER_PID[tab.dwNumEntries];
                
                for (int i = 0; i < tab.dwNumEntries; i++)
                {
                    MIB_TCPROW_OWNER_PID tcpRow = (MIB_TCPROW_OWNER_PID)Marshal.PtrToStructure(rowPtr, typeof(MIB_TCPROW_OWNER_PID));
                    tTable[i] = tcpRow;
                    // next entry
                    rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(tcpRow));
                }
            }
            finally
            {        
                // Free the Memory
                Marshal.FreeHGlobal(buffTable);
            }
            
            return tTable;
        }

        public static int PortToPID(int port)
        {
            MIB_TCPROW_OWNER_PID[] tcpConnections = GetAllTcpConnections();

            foreach (MIB_TCPROW_OWNER_PID row in tcpConnections)
            {
                if (row.LocalPort == port)
                {
                    return row.owningPid;
                }
            }

            return -1;
        }
    }

    public static class AdvApi32
    {
        public const int TOKEN_QUERY = 0X00000008;

        [DllImport("advapi32", SetLastError = true)]
        public static extern int OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);
    }

    public static class Kernel32
    {
        public const uint PROCESS_VM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32", SetLastError=true)]
        public static extern bool CloseHandle(IntPtr handle);
    }

    public static class Shell32
    {
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000002-0000-0000-C000-000000000046")]
        public interface IMalloc
        {
            [PreserveSig]
            IntPtr Alloc([In] int cb);
            [PreserveSig]
            IntPtr Realloc([In] IntPtr pv, [In] int cb);
            [PreserveSig]
            void Free([In] IntPtr pv);
            [PreserveSig]
            int GetSize([In] IntPtr pv);
            [PreserveSig]
            int DidAlloc(IntPtr pv);
            [PreserveSig]
            void HeapMinimize();
        }

        [Flags]
        public enum BrowseInfoFlags : int
        {
            /// <summary>
            // Only return file system directories:
            /// </summary>
            BIF_RETURNONLYFSDIRS = 0x0001,
            /// <summary>
            // When looking at Network computers, don't
            // go down to the share level:
            /// </summary>
            BIF_DONTGOBELOWDOMAIN = 0x0002,
            /// <summary>
            // Show a status text box as well as a title:
            /// </summary>
            BIF_STATUSTEXT = 0x0004,
            /// <summary>
            // Gray the ok button when a non-file system 
            // item is selected:
            /// </summary>
            BIF_RETURNFSANCESTORS = 0x0008,
            /// <summary>
            // Show an Edit box to allow the user to type
            // a file name:
            /// </summary>
            BIF_EDITBOX = 0x0010,
            /// <summary>
            // Cause the Browse Folder to callback when 
            // the user clicks OK but invalid text is in
            // the edit box:
            /// </summary>
            BIF_VALIDATE = 0x0020,
            /// <summary>
            // Make the dialog appear with the new UI style:
            // a resizable dialog with drag-drop, ordering,
            // shortcut menus, new folder button, ability to
            // delete folders.
            /// </summary>
            BIF_NEWDIALOGSTYLE = 0x0040,
            /// <summary>
            /// Allow URLs to be selected.  BIF_NEWDIALOGSTYLE, BIF_BROWSEINCLUDEFILES and 
            /// BIF_EDITBOX must also be set for this to work.
            /// </summary>
            BIF_BROWSEINCLUDEURLS = 0x0080,
            /// <summary>
            /// Replace the text box with a user interface usage hint (only if BIF_EDITBOX not set)
            /// </summary>
            BIF_UAHINT = 0x0100,
            /// <summary>
            /// Don't show the New Folder button when the new dialog style is being used:
            /// </summary>
            BIF_NONEWFOLDERBUTTON = 0x0200,
            /// <summary>
            /// Browse for computers; if anything else selected, the dialog can't be OKed.
            /// </summary>
            BIF_BROWSEFORCOMPUTER = 0x1000,
            /// <summary>
            /// Browse for printers; if anything else selected, the dialog can't be OKed.
            /// </summary>
            BIF_BROWSEFORPRINTER = 0x2000,
            /// <summary>
            /// Include files in a New Style dialog
            /// </summary>
            BIF_BROWSEINCLUDEFILES = 0x4000,
            /// <summary>
            /// Shareable resources on remote systems are
            /// </summary>
            BIF_SHAREABLE = 0x8000,
        }

        /// <summary>
        /// Enum of CSIDLs identifying standard shell folders.
        /// </summary>
        public enum FolderID
        {
            Desktop = 0x0000,
            Printers = 0x0004,
            MyDocuments = 0x0005,
            Favorites = 0x0006,
            Recent = 0x0008,
            SendTo = 0x0009,
            StartMenu = 0x000b,
            MyComputer = 0x0011,
            NetworkNeighborhood = 0x0012,
            Templates = 0x0015,
            MyPictures = 0x0027,
            NetAndDialUpConnections = 0x0031,
        }

        // Delegate type used in BROWSEINFO.lpfn field.
        public delegate int BFFCALLBACK(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData);

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszTitle;
            public int ulFlags;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public BFFCALLBACK lpfn;
            public IntPtr lParam;
            public int iImage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public const int NAMESIZE = 80;
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAMESIZE)]
            public string szTypeName;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public int wFunc;
            public string pFrom;
            public string pTo;
            public UInt16 fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public String lpszProgressTitle;
        }

        public const uint SHGFI_ICON = 0x000000100;     // get icon
        public const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
        public const uint SHGFI_TYPENAME = 0x000000400;     // get type name
        public const uint SHGFI_ATTRIBUTES = 0x000000800;     // get attributes
        public const uint SHGFI_ICONLOCATION = 0x000001000;     // get icon location
        public const uint SHGFI_EXETYPE = 0x000002000;     // return exe type
        public const uint SHGFI_SYSICONINDEX = 0x000004000;     // get system icon index
        public const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
        public const uint SHGFI_SELECTED = 0x000010000;     // show icon in selected state
        public const uint SHGFI_ATTR_SPECIFIED = 0x000020000;     // get only specified attributes
        public const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
        public const uint SHGFI_SMALLICON = 0x000000001;     // get small icon
        public const uint SHGFI_OPENICON = 0x000000002;     // get open icon
        public const uint SHGFI_SHELLICONSIZE = 0x000000004;     // get shell size icon
        public const uint SHGFI_PIDL = 0x000000008;     // pszPath is a pidl
        public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute
        public const uint SHGFI_ADDOVERLAYS = 0x000000020;     // apply the appropriate overlays
        public const uint SHGFI_OVERLAYINDEX = 0x000000040;     // Get the index of the overlay

        public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;  
        public const int  MAX_PATH = 260;

        public const int FO_DELETE = 3;
        public const int FOF_ALLOWUNDO = 0x40;
        public const int FOF_NOCONFIRMATION = 0x10;

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO bi);

        [DllImport("Shell32.DLL")]
        public static extern int SHGetMalloc(out IMalloc ppMalloc);

        [DllImport("Shell32.DLL")]
        public static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, int nFolder, out IntPtr ppidl);

        [DllImport("Shell32.DLL")]
        public static extern int SHGetPathFromIDList(IntPtr pidl, IntPtr Path);

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

    }


    public static class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct NMHDR
        {
            public IntPtr hwndFrom;
            public UIntPtr idFrom;
            public int code;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLINFO
        {
            public int cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        public enum ScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }

        [Flags]
        public enum ScrollInfoMask
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS
        }

        [Flags()]
        public enum MOUSEEVENTF
        {
            MOVE = 0x0001,  // mouse move 
            LEFTDOWN = 0x0002,  // left button down
            LEFTUP = 0x0004,  // left button up
            RIGHTDOWN = 0x0008,  // right button down
            RIGHTUP = 0x0010,  // right button up
            MIDDLEDOWN = 0x0020,  // middle button down
            MIDDLEUP = 0x0040,  // middle button up
            XDOWN = 0x0080,  // x button down 
            XUP = 0x0100,  // x button down
            WHEEL = 0x0800,  // wheel button rolled
            VIRTUALDESK = 0x4000,  // map to entire virtual desktop
            ABSOLUTE = 0x8000,  // absolute move
        }

        [Flags()]
        public enum KEYEVENTF
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }


        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        public struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        public struct INPUT
        {
            public int type;
            public MOUSEKEYBDHARDWAREINPUT mkhi;
        }

        public enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }

        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_UNICODE = 0x0004;
        public const uint KEYEVENTF_SCANCODE = 0x0008;
        public const uint XBUTTON1 = 0x0001;
        public const uint XBUTTON2 = 0x0002;
        public const uint MOUSEEVENTF_MOVE = 0x0001;
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const uint MOUSEEVENTF_XDOWN = 0x0080;
        public const uint MOUSEEVENTF_XUP = 0x0100;
        public const uint MOUSEEVENTF_WHEEL = 0x0800;
        public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
        public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        public const uint FLASHW_STOP = 0;
        public const uint FLASHW_ALL = 3;
        public const uint FLASHW_TIMERNOFG = 12;

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetMessageExtraInfo();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int DestroyIcon(IntPtr hIcon);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDCEx(IntPtr hwnd, IntPtr hrgnclip, uint fdwOptions);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO ScrollInfo);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool CreateCaret(IntPtr hWnd, IntPtr hBmp, int w, int h);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetCaretPos(int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowCaret(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyCaret();
    }

    public enum VK : ushort
    {
        //
        // Virtual Keys, Standard Set
        //
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_CANCEL = 0x03,
        VK_MBUTTON = 0x04,    // NOT contiguous with L & RBUTTON

        VK_XBUTTON1 = 0x05,    // NOT contiguous with L & RBUTTON
        VK_XBUTTON2 = 0x06,    // NOT contiguous with L & RBUTTON

        // 0x07 : unassigned

        VK_BACK = 0x08,
        VK_TAB = 0x09,

        // 0x0A - 0x0B : reserved

        VK_CLEAR = 0x0C,
        VK_RETURN = 0x0D,

        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_PAUSE = 0x13,
        VK_CAPITAL = 0x14,

        VK_KANA = 0x15,
        VK_HANGEUL = 0x15,  // old name - should be here for compatibility
        VK_HANGUL = 0x15,
        VK_JUNJA = 0x17,
        VK_FINAL = 0x18,
        VK_HANJA = 0x19,
        VK_KANJI = 0x19,

        VK_ESCAPE = 0x1B,

        VK_CONVERT = 0x1C,
        VK_NONCONVERT = 0x1D,
        VK_ACCEPT = 0x1E,
        VK_MODECHANGE = 0x1F,

        VK_SPACE = 0x20,
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_SELECT = 0x29,
        VK_PRINT = 0x2A,
        VK_EXECUTE = 0x2B,
        VK_SNAPSHOT = 0x2C,
        VK_INSERT = 0x2D,
        VK_DELETE = 0x2E,
        VK_HELP = 0x2F,

        //
        // VK_0 - VK_9 are the same as ASCII '0' - '9' (0x30 - 0x39)
        // 0x40 : unassigned
        // VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A)
        //

        VK_LWIN = 0x5B,
        VK_RWIN = 0x5C,
        VK_APPS = 0x5D,

        //
        // 0x5E : reserved
        //

        VK_SLEEP = 0x5F,

        VK_NUMPAD0 = 0x60,
        VK_NUMPAD1 = 0x61,
        VK_NUMPAD2 = 0x62,
        VK_NUMPAD3 = 0x63,
        VK_NUMPAD4 = 0x64,
        VK_NUMPAD5 = 0x65,
        VK_NUMPAD6 = 0x66,
        VK_NUMPAD7 = 0x67,
        VK_NUMPAD8 = 0x68,
        VK_NUMPAD9 = 0x69,
        VK_MULTIPLY = 0x6A,
        VK_ADD = 0x6B,
        VK_SEPARATOR = 0x6C,
        VK_SUBTRACT = 0x6D,
        VK_DECIMAL = 0x6E,
        VK_DIVIDE = 0x6F,
        VK_F1 = 0x70,
        VK_F2 = 0x71,
        VK_F3 = 0x72,
        VK_F4 = 0x73,
        VK_F5 = 0x74,
        VK_F6 = 0x75,
        VK_F7 = 0x76,
        VK_F8 = 0x77,
        VK_F9 = 0x78,
        VK_F10 = 0x79,
        VK_F11 = 0x7A,
        VK_F12 = 0x7B,
        VK_F13 = 0x7C,
        VK_F14 = 0x7D,
        VK_F15 = 0x7E,
        VK_F16 = 0x7F,
        VK_F17 = 0x80,
        VK_F18 = 0x81,
        VK_F19 = 0x82,
        VK_F20 = 0x83,
        VK_F21 = 0x84,
        VK_F22 = 0x85,
        VK_F23 = 0x86,
        VK_F24 = 0x87,

        //
        // 0x88 - 0x8F : unassigned
        //

        VK_NUMLOCK = 0x90,
        VK_SCROLL = 0x91,

        //
        // VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys.
        // Used only as parameters to GetAsyncKeyState() and GetKeyState().
        // No other API or message will distinguish left and right keys in this way.
        //
        VK_LSHIFT = 0xA0,
        VK_RSHIFT = 0xA1,
        VK_LCONTROL = 0xA2,
        VK_RCONTROL = 0xA3,
        VK_LMENU = 0xA4,
        VK_RMENU = 0xA5,

        VK_BROWSER_BACK = 0xA6,
        VK_BROWSER_FORWARD = 0xA7,
        VK_BROWSER_REFRESH = 0xA8,
        VK_BROWSER_STOP = 0xA9,
        VK_BROWSER_SEARCH = 0xAA,
        VK_BROWSER_FAVORITES = 0xAB,
        VK_BROWSER_HOME = 0xAC,

        VK_VOLUME_MUTE = 0xAD,
        VK_VOLUME_DOWN = 0xAE,
        VK_VOLUME_UP = 0xAF,
        VK_MEDIA_NEXT_TRACK = 0xB0,
        VK_MEDIA_PREV_TRACK = 0xB1,
        VK_MEDIA_STOP = 0xB2,
        VK_MEDIA_PLAY_PAUSE = 0xB3,
        VK_LAUNCH_MAIL = 0xB4,
        VK_LAUNCH_MEDIA_SELECT = 0xB5,
        VK_LAUNCH_APP1 = 0xB6,
        VK_LAUNCH_APP2 = 0xB7,

        //
        // 0xB8 - 0xB9 : reserved
        //

        VK_OEM_1 = 0xBA,   // ';:' for US
        VK_OEM_PLUS = 0xBB,   // '+' any country
        VK_OEM_COMMA = 0xBC,   // ',' any country
        VK_OEM_MINUS = 0xBD,   // '-' any country
        VK_OEM_PERIOD = 0xBE,   // '.' any country
        VK_OEM_2 = 0xBF,   // '/?' for US
        VK_OEM_3 = 0xC0,   // '`~' for US

        //
        // 0xC1 - 0xD7 : reserved
        //

        //
        // 0xD8 - 0xDA : unassigned
        //

        VK_OEM_4 = 0xDB,  //  '[{' for US
        VK_OEM_5 = 0xDC,  //  '\|' for US
        VK_OEM_6 = 0xDD,  //  ']}' for US
        VK_OEM_7 = 0xDE,  //  ''"' for US
        VK_OEM_8 = 0xDF

        //
        // 0xE0 : reserved
        //
    }

    public enum WindowMessages
    {
        WM_USER = 0x0400,

        WM_ERASEBKGND = 0x0014,
        WM_NCPAINT = 0x85,
        WM_PAINT = 0x000F,
        WM_CLICK = 0x00F5,
        WM_REFLECT = WM_USER + 0x1C00,
        WM_NOTIFY = 0x004E,
        WM_KEYDOWN = 0x0100,
        WM_HSCROLL = 0x114,
        WM_VSCROLL = 0x115,
        LVM_HITTEST = (0x1000 + 18)
    }

    public class DbgHelp
    {
        [Flags]
        public enum SymOpt : uint
        {
            CASE_INSENSITIVE = 0x00000001,
            UNDNAME = 0x00000002,
            DEFERRED_LOADS = 0x00000004,
            NO_CPP = 0x00000008,
            LOAD_LINES = 0x00000010,
            OMAP_FIND_NEAREST = 0x00000020,
            LOAD_ANYTHING = 0x00000040,
            IGNORE_CVREC = 0x00000080,
            NO_UNQUALIFIED_LOADS = 0x00000100,
            FAIL_CRITICAL_ERRORS = 0x00000200,
            EXACT_SYMBOLS = 0x00000400,
            ALLOW_ABSOLUTE_SYMBOLS = 0x00000800,
            IGNORE_NT_SYMPATH = 0x00001000,
            INCLUDE_32BIT_MODULES = 0x00002000,
            PUBLICS_ONLY = 0x00004000,
            NO_PUBLICS = 0x00008000,
            AUTO_PUBLICS = 0x00010000,
            NO_IMAGE_SEARCH = 0x00020000,
            SECURE = 0x00040000,
            SYMOPT_DEBUG = 0x80000000
        };

        [Flags]
        public enum SymFlag : uint
        {
            VALUEPRESENT = 0x00000001,
            REGISTER = 0x00000008,
            REGREL = 0x00000010,
            FRAMEREL = 0x00000020,
            PARAMETER = 0x00000040,
            LOCAL = 0x00000080,
            CONSTANT = 0x00000100,
            EXPORT = 0x00000200,
            FORWARDER = 0x00000400,
            FUNCTION = 0x00000800,
            VIRTUAL = 0x00001000,
            THUNK = 0x00002000,
            TLSREL = 0x00004000,
        }

        [Flags]
        public enum SLMFlags : uint
        {
            SLMFLAG_NO_SYMBOLS = 0x4,
            SLMFLAG_VIRTUAL = 0x1,
        }

        [Flags]
        public enum SymTagEnum : uint
        {
            Null,
            Exe,
            Compiland,
            CompilandDetails,
            CompilandEnv,
            Function,
            Block,
            Data,
            Annotation,
            Label,
            PublicSymbol,
            UDT,
            Enum,
            FunctionType,
            PointerType,
            ArrayType,
            BaseType,
            Typedef,
            BaseClass,
            Friend,
            FunctionArgType,
            FuncDebugStart,
            FuncDebugEnd,
            UsingNamespace,
            VTableShape,
            VTable,
            Custom,
            Thunk,
            CustomType,
            ManagedType,
            Dimension
        };

        public enum IMAGEHLP_SYMBOL_TYPE_INFO : uint
        {
          TI_GET_SYMTAG,
          TI_GET_SYMNAME,
          TI_GET_LENGTH,
          TI_GET_TYPE,
          TI_GET_TYPEID,
          TI_GET_BASETYPE,
          TI_GET_ARRAYINDEXTYPEID,
          TI_FINDCHILDREN,
          TI_GET_DATAKIND,
          TI_GET_ADDRESSOFFSET,
          TI_GET_OFFSET,
          TI_GET_VALUE,
          TI_GET_COUNT,
          TI_GET_CHILDRENCOUNT,
          TI_GET_BITPOSITION,
          TI_GET_VIRTUALBASECLASS,
          TI_GET_VIRTUALTABLESHAPEID,
          TI_GET_VIRTUALBASEPOINTEROFFSET,
          TI_GET_CLASSPARENTID,
          TI_GET_NESTED,
          TI_GET_SYMINDEX,
          TI_GET_LEXICALPARENT,
          TI_GET_ADDRESS,
          TI_GET_THISADJUST,
          TI_GET_UDTKIND,
          TI_IS_EQUIV_TO,
          TI_GET_CALLING_CONVENTION,
          TI_IS_CLOSE_EQUIV_TO,
          TI_GTIEX_REQS_VALID,
          TI_GET_VIRTUALBASEOFFSET,
          TI_GET_VIRTUALBASEDISPINDEX,
          TI_GET_IS_REFERENCE,
          TI_GET_INDIRECTVIRTUALBASECLASS
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SYMBOL_INFO
        {
            public uint SizeOfStruct;
            public uint TypeIndex;
            public ulong Reserved1;
            public ulong Reserved2;
            public uint Reserved3;
            public uint Size;
            public ulong ModBase;
            public SymFlag Flags;
            public ulong Value;
            public ulong Address;
            public uint Register;
            public uint Scope;
            public SymTagEnum Tag;
            public int NameLen;
            public int MaxNameLen;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string Name;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct _IMAGEHLP_LINE64
        {
            public uint SizeOfStruct;
            public uint Key;
            public uint LineNumber;
            public IntPtr FileName;
            public ulong Address;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct _MODLOAD_DATA
        {
            uint ssize;
            uint ssig;
            IntPtr data;
            uint size;
            uint flags;
        };

        public delegate bool SymEnumSymbolsProc(ref SYMBOL_INFO pSymInfo, uint SymbolSize, IntPtr UserContext);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern bool SymInitialize(IntPtr hProcess, string UserSearchPath, bool fInvadeProcess);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern uint SymSetOptions(SymOpt SymOptions);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern ulong SymLoadModuleEx(IntPtr hProcess, IntPtr hFile, string ImageName, string ModuleName, ulong BaseOfDll, uint DllSize, ref _MODLOAD_DATA Data, SLMFlags Flags);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern bool SymEnumSymbols(IntPtr hProcess, ulong BaseOfDll, string Mask, SymEnumSymbolsProc EnumSymbolsCallback, IntPtr UserContext);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern bool SymGetTypeInfo(IntPtr hProcess, ulong ModBase, uint TypeId, uint GetType, ref IntPtr pInfo);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern bool SymGetLineFromAddr64(IntPtr hProcess, ulong dwAddr, ref uint pdwDisplacement, ref _IMAGEHLP_LINE64 Line);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern bool SymFromAddr(IntPtr hProcess, ulong dwAddr, ref ulong pdwDisplacement, ref SYMBOL_INFO symbolInfo);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern bool SymEnumSymbolsForAddr(IntPtr hProcess, ulong Address, SymEnumSymbolsProc EnumSymbolsCallback, IntPtr UserContext);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern bool SymUnloadModule64(IntPtr hProcess, ulong BaseOfDll);

        [DllImport("dbghelp.dll", SetLastError = true)]
        public static extern bool SymCleanup(IntPtr hProcess);
    }
}
