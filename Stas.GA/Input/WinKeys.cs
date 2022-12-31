namespace Stas.GA; 

[Flags]
public enum Keys {
    //
    // Summary:
    //     The bitmask to extract modifiers from a key value.
    Modifiers = -65536,
    //
    // Summary:
    //     No key pressed.
    None = 0,
    //
    // Summary:
    //     The left mouse button.
    LButton = 1,
    //
    // Summary:
    //     The right mouse button.
    RButton = 2,
    //
    // Summary:
    //     The CANCEL key.
    Cancel = 3,
    //
    // Summary:
    //     The middle mouse button (three-button mouse).
    MButton = 4,
    //
    // Summary:
    //     The first x mouse button (five-button mouse).
    XButton1 = 5,
    //
    // Summary:
    //     The second x mouse button (five-button mouse).
    XButton2 = 6,
    //
    // Summary:
    //     The BACKSPACE key.
    Back = 8,
    //
    // Summary:
    //     The TAB key.
    Tab = 9,
    //
    // Summary:
    //     The LINEFEED key.
    LineFeed = 10,
    //
    // Summary:
    //     The CLEAR key.
    Clear = 12,
    //
    // Summary:
    //     The RETURN key.
    Return = 13,
    //
    // Summary:
    //     The ENTER key.
    Enter = 13,
    //
    // Summary:
    //     The SHIFT key.
    ShiftKey = 16,
    //
    // Summary:
    //     The CTRL key.
    ControlKey = 17,
    //
    // Summary:
    //     The ALT key.
    Menu = 18,
    //
    // Summary:
    //     The PAUSE key.
    Pause = 19,
    //
    // Summary:
    //     The CAPS LOCK key.
    Capital = 20,
    //
    // Summary:
    //     The CAPS LOCK key.
    CapsLock = 20,
    //
    // Summary:
    //     The IME Kana mode key.
    KanaMode = 21,
    //
    // Summary:
    //     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
    HanguelMode = 21,
    //
    // Summary:
    //     The IME Hangul mode key.
    HangulMode = 21,
    //
    // Summary:
    //     The IME Junja mode key.
    JunjaMode = 23,
    //
    // Summary:
    //     The IME final mode key.
    FinalMode = 24,
    //
    // Summary:
    //     The IME Hanja mode key.
    HanjaMode = 25,
    //
    // Summary:
    //     The IME Kanji mode key.
    KanjiMode = 25,
    //
    // Summary:
    //     The ESC key.
    Escape = 27,
    //
    // Summary:
    //     The IME convert key.
    IMEConvert = 28,
    //
    // Summary:
    //     The IME nonconvert key.
    IMENonconvert = 29,
    //
    // Summary:
    //     The IME accept key, replaces System.Windows.Forms.Keys.IMEAceept.
    IMEAccept = 30,
    //
    // Summary:
    //     The IME accept key. Obsolete, use System.Windows.Forms.Keys.IMEAccept instead.
    IMEAceept = 30,
    //
    // Summary:
    //     The IME mode change key.
    IMEModeChange = 31,
    //
    // Summary:
    //     The SPACEBAR key.
    Space = 32,
    //
    // Summary:
    //     The PAGE UP key.
    Prior = 33,
    //
    // Summary:
    //     The PAGE UP key.
    PageUp = 33,
    //
    // Summary:
    //     The PAGE DOWN key.
    Next = 34,
    //
    // Summary:
    //     The PAGE DOWN key.
    PageDown = 34,
    //
    // Summary:
    //     The END key.
    End = 35,
    //
    // Summary:
    //     The HOME key.
    Home = 36,
    //
    // Summary:
    //     The LEFT ARROW key.
    Left = 37,
    //
    // Summary:
    //     The UP ARROW key.
    Up = 38,
    //
    // Summary:
    //     The RIGHT ARROW key.
    Right = 39,
    //
    // Summary:
    //     The DOWN ARROW key.
    Down = 40,
    //
    // Summary:
    //     The SELECT key.
    Select = 41,
    //
    // Summary:
    //     The PRINT key.
    Print = 42,
    //
    // Summary:
    //     The EXECUTE key.
    Execute = 43,
    //
    // Summary:
    //     The PRINT SCREEN key.
    Snapshot = 44,
    //
    // Summary:
    //     The PRINT SCREEN key.
    PrintScreen = 44,
    //
    // Summary:
    //     The INS key.
    Insert = 45,
    //
    // Summary:
    //     The DEL key.
    Delete = 46,
    //
    // Summary:
    //     The HELP key.
    Help = 47,
    //
    // Summary:
    //     The 0 key.
    D0 = 48,
    //
    // Summary:
    //     The 1 key.
    D1 = 49,
    //
    // Summary:
    //     The 2 key.
    D2 = 50,
    //
    // Summary:
    //     The 3 key.
    D3 = 51,
    //
    // Summary:
    //     The 4 key.
    D4 = 52,
    //
    // Summary:
    //     The 5 key.
    D5 = 53,
    //
    // Summary:
    //     The 6 key.
    D6 = 54,
    //
    // Summary:
    //     The 7 key.
    D7 = 55,
    //
    // Summary:
    //     The 8 key.
    D8 = 56,
    //
    // Summary:
    //     The 9 key.
    D9 = 57,
    //
    // Summary:
    //     The A key.
    A = 65,
    //
    // Summary:
    //     The B key.
    B = 66,
    //
    // Summary:
    //     The C key.
    C = 67,
    //
    // Summary:
    //     The D key.
    D = 68,
    //
    // Summary:
    //     The E key.
    E = 69,
    //
    // Summary:
    //     The F key.
    F = 70,
    //
    // Summary:
    //     The G key.
    G = 71,
    //
    // Summary:
    //     The H key.
    H = 72,
    //
    // Summary:
    //     The I key.
    I = 73,
    //
    // Summary:
    //     The J key.
    J = 74,
    //
    // Summary:
    //     The K key.
    K = 75,
    //
    // Summary:
    //     The L key.
    L = 76,
    //
    // Summary:
    //     The M key.
    M = 77,
    //
    // Summary:
    //     The N key.
    N = 78,
    //
    // Summary:
    //     The O key.
    O = 79,
    //
    // Summary:
    //     The P key.
    P = 80,
    //
    // Summary:
    //     The Q key.
    Q = 81,
    //
    // Summary:
    //     The R key.
    R = 82,
    //
    // Summary:
    //     The S key.
    S = 83,
    //
    // Summary:
    //     The T key.
    T = 84,
    //
    // Summary:
    //     The U key.
    U = 85,
    //
    // Summary:
    //     The V key.
    V = 86,
    //
    // Summary:
    //     The W key.
    W = 87,
    //
    // Summary:
    //     The X key.
    X = 88,
    //
    // Summary:
    //     The Y key.
    Y = 89,
    //
    // Summary:
    //     The Z key.
    Z = 90,
    //
    // Summary:
    //     The left Windows logo key (Microsoft Natural Keyboard).
    LWin = 91,
    //
    // Summary:
    //     The right Windows logo key (Microsoft Natural Keyboard).
    RWin = 92,
    //
    // Summary:
    //     The application key (Microsoft Natural Keyboard).
    Apps = 93,
    //
    // Summary:
    //     The computer sleep key.
    Sleep = 95,
    //
    // Summary:
    //     The 0 key on the numeric keypad.
    NumPad0 = 96,
    //
    // Summary:
    //     The 1 key on the numeric keypad.
    NumPad1 = 97,
    //
    // Summary:
    //     The 2 key on the numeric keypad.
    NumPad2 = 98,
    //
    // Summary:
    //     The 3 key on the numeric keypad.
    NumPad3 = 99,
    //
    // Summary:
    //     The 4 key on the numeric keypad.
    NumPad4 = 100,
    //
    // Summary:
    //     The 5 key on the numeric keypad.
    NumPad5 = 101,
    //
    // Summary:
    //     The 6 key on the numeric keypad.
    NumPad6 = 102,
    //
    // Summary:
    //     The 7 key on the numeric keypad.
    NumPad7 = 103,
    //
    // Summary:
    //     The 8 key on the numeric keypad.
    NumPad8 = 104,
    //
    // Summary:
    //     The 9 key on the numeric keypad.
    NumPad9 = 105,
    //
    // Summary:
    //     The multiply key.
    Multiply = 106,
    //
    // Summary:
    //     The add key.
    Add = 107,
    //
    // Summary:
    //     The separator key.
    Separator = 108,
    //
    // Summary:
    //     The subtract key.
    Subtract = 109,
    //
    // Summary:
    //     The decimal key.
    Decimal = 110,
    //
    // Summary:
    //     The divide key.
    Divide = 111,
    //
    // Summary:
    //     The F1 key.
    F1 = 112,
    //
    // Summary:
    //     The F2 key.
    F2 = 113,
    //
    // Summary:
    //     The F3 key.
    F3 = 114,
    //
    // Summary:
    //     The F4 key.
    F4 = 115,
    //
    // Summary:
    //     The F5 key.
    F5 = 116,
    //
    // Summary:
    //     The F6 key.
    F6 = 117,
    //
    // Summary:
    //     The F7 key.
    F7 = 118,
    //
    // Summary:
    //     The F8 key.
    F8 = 119,
    //
    // Summary:
    //     The F9 key.
    F9 = 120,
    //
    // Summary:
    //     The F10 key.
    F10 = 121,
    //
    // Summary:
    //     The F11 key.
    F11 = 122,
    //
    // Summary:
    //     The F12 key.
    F12 = 123,
    //
    // Summary:
    //     The F13 key.
    F13 = 124,
    //
    // Summary:
    //     The F14 key.
    F14 = 125,
    //
    // Summary:
    //     The F15 key.
    F15 = 126,
    //
    // Summary:
    //     The F16 key.
    F16 = 127,
    //
    // Summary:
    //     The F17 key.
    F17 = 128,
    //
    // Summary:
    //     The F18 key.
    F18 = 129,
    //
    // Summary:
    //     The F19 key.
    F19 = 130,
    //
    // Summary:
    //     The F20 key.
    F20 = 131,
    //
    // Summary:
    //     The F21 key.
    F21 = 132,
    //
    // Summary:
    //     The F22 key.
    F22 = 133,
    //
    // Summary:
    //     The F23 key.
    F23 = 134,
    //
    // Summary:
    //     The F24 key.
    F24 = 135,
    //
    // Summary:
    //     The NUM LOCK key.
    NumLock = 144,
    //
    // Summary:
    //     The SCROLL LOCK key.
    Scroll = 145,
    //
    // Summary:
    //     The left SHIFT key.
    LShiftKey = 160,
    //
    // Summary:
    //     The right SHIFT key.
    RShiftKey = 161,
    //
    // Summary:
    //     The left CTRL key.
    LControlKey = 162,
    //
    // Summary:
    //     The right CTRL key.
    RControlKey = 163,
    //
    // Summary:
    //     The left ALT key.
    LMenu = 164,
    //
    // Summary:
    //     The right ALT key.
    RMenu = 165,
    //
    // Summary:
    //     The browser back key.
    BrowserBack = 166,
    //
    // Summary:
    //     The browser forward key.
    BrowserForward = 167,
    //
    // Summary:
    //     The browser refresh key.
    BrowserRefresh = 168,
    //
    // Summary:
    //     The browser stop key.
    BrowserStop = 169,
    //
    // Summary:
    //     The browser search key.
    BrowserSearch = 170,
    //
    // Summary:
    //     The browser favorites key.
    BrowserFavorites = 171,
    //
    // Summary:
    //     The browser home key.
    BrowserHome = 172,
    //
    // Summary:
    //     The volume mute key.
    VolumeMute = 173,
    //
    // Summary:
    //     The volume down key.
    VolumeDown = 174,
    //
    // Summary:
    //     The volume up key.
    VolumeUp = 175,
    //
    // Summary:
    //     The media next track key.
    MediaNextTrack = 176,
    //
    // Summary:
    //     The media previous track key.
    MediaPreviousTrack = 177,
    //
    // Summary:
    //     The media Stop key.
    MediaStop = 178,
    //
    // Summary:
    //     The media play pause key.
    MediaPlayPause = 179,
    //
    // Summary:
    //     The launch mail key.
    LaunchMail = 180,
    //
    // Summary:
    //     The select media key.
    SelectMedia = 181,
    //
    // Summary:
    //     The start application one key.
    LaunchApplication1 = 182,
    //
    // Summary:
    //     The start application two key.
    LaunchApplication2 = 183,
    //
    // Summary:
    //     The OEM Semicolon key on a US standard keyboard.
    OemSemicolon = 186,
    //
    // Summary:
    //     The OEM 1 key.
    Oem1 = 186,
    //
    // Summary:
    //     The OEM plus key on any country/region keyboard.
    Oemplus = 187,
    //
    // Summary:
    //     The OEM comma key on any country/region keyboard.
    Oemcomma = 188,
    //
    // Summary:
    //     The OEM minus key on any country/region keyboard.
    OemMinus = 189,
    //
    // Summary:
    //     The OEM period key on any country/region keyboard.
    OemPeriod = 190,
    //
    // Summary:
    //     The OEM question mark key on a US standard keyboard.
    OemQuestion = 191,
    //
    // Summary:
    //     The OEM 2 key.
    Oem2 = 191,
    //
    // Summary:
    //     The OEM tilde key on a US standard keyboard.
    Oemtilde = 192,
    //
    // Summary:
    //     The OEM 3 key.
    Oem3 = 192,
    //
    // Summary:
    //     The OEM open bracket key on a US standard keyboard.
    OemOpenBrackets = 219,
    //
    // Summary:
    //     The OEM 4 key.
    Oem4 = 219,
    //
    // Summary:
    //     The OEM pipe key on a US standard keyboard.
    OemPipe = 220,
    //
    // Summary:
    //     The OEM 5 key.
    Oem5 = 220,
    //
    // Summary:
    //     The OEM close bracket key on a US standard keyboard.
    OemCloseBrackets = 221,
    //
    // Summary:
    //     The OEM 6 key.
    Oem6 = 221,
    //
    // Summary:
    //     The OEM singled/double quote key on a US standard keyboard.
    OemQuotes = 222,
    //
    // Summary:
    //     The OEM 7 key.
    Oem7 = 222,
    //
    // Summary:
    //     The OEM 8 key.
    Oem8 = 223,
    //
    // Summary:
    //     The OEM angle bracket or backslash key on the RT 102 key keyboard.
    OemBackslash = 226,
    //
    // Summary:
    //     The OEM 102 key.
    Oem102 = 226,
    //
    // Summary:
    //     The PROCESS KEY key.
    ProcessKey = 229,
    //
    // Summary:
    //     Used to pass Unicode characters as if they were keystrokes. The Packet key value
    //     is the low word of a 32-bit virtual-key value used for non-keyboard input methods.
    Packet = 231,
    //
    // Summary:
    //     The ATTN key.
    Attn = 246,
    //
    // Summary:
    //     The CRSEL key.
    Crsel = 247,
    //
    // Summary:
    //     The EXSEL key.
    Exsel = 248,
    //
    // Summary:
    //     The ERASE EOF key.
    EraseEof = 249,
    //
    // Summary:
    //     The PLAY key.
    Play = 250,
    //
    // Summary:
    //     The ZOOM key.
    Zoom = 251,
    //
    // Summary:
    //     A constant reserved for future use.
    NoName = 252,
    //
    // Summary:
    //     The PA1 key.
    Pa1 = 253,
    //
    // Summary:
    //     The CLEAR key.
    OemClear = 254,
    //
    // Summary:
    //     The bitmask to extract a key code from a key value.
    KeyCode = 65535,
    //
    // Summary:
    //     The SHIFT modifier key.
    Shift = 65536,
    //
    // Summary:
    //     The CTRL modifier key.
    Control = 131072,
    //
    // Summary:
    //     The ALT modifier key.
    Alt = 262144
}
//for AHK - i hope its same like win keys A is 65 = 0x41  Z = 0x5A=90
/// <summary>
/// DONT USE it - its just for test
/// </summary>
public enum VirtualKeys  : int {
    /// <summary></summary>
    LeftButton = 0x01,
    /// <summary></summary>
    RightButton = 0x02,
    /// <summary></summary>
    Cancel = 0x03,
    /// <summary></summary>
    MiddleButton = 0x04,
    /// <summary></summary>
    ExtraButton1 = 0x05,
    /// <summary></summary>
    ExtraButton2 = 0x06,
    /// <summary></summary>
    Back = 0x08,
    /// <summary></summary>
    Tab = 0x09,
    /// <summary></summary>
    Clear = 0x0C,
    /// <summary></summary>
    Return = 0x0D,
    /// <summary></summary>
    Shift = 0x10,
    /// <summary></summary>
    Control = 0x11,
    /// <summary></summary>
    Menu = 0x12,
    /// <summary></summary>
    Pause = 0x13,
    /// <summary></summary>
    CapsLock = 0x14, //=20?
    /// <summary></summary>
    Kana = 0x15,
    /// <summary></summary>
    Hangeul = 0x15,
    /// <summary></summary>
    Hangul = 0x15,
    /// <summary></summary>
    Junja = 0x17,
    /// <summary></summary>
    Final = 0x18,
    /// <summary></summary>
    Hanja = 0x19,
    /// <summary></summary>
    Kanji = 0x19,
    /// <summary></summary>
    Escape = 0x1B,
    /// <summary></summary>
    Convert = 0x1C,
    /// <summary></summary>
    NonConvert = 0x1D,
    /// <summary></summary>
    Accept = 0x1E,
    /// <summary></summary>
    ModeChange = 0x1F,
    /// <summary></summary>
    Space = 0x20,
    /// <summary></summary>
    Prior = 0x21,
    /// <summary></summary>
    Next = 0x22,
    /// <summary></summary>
    End = 0x23,
    /// <summary></summary>
    Home = 0x24,
    /// <summary></summary>
    Left = 0x25,
    /// <summary></summary>
    Up = 0x26,
    /// <summary></summary>
    Right = 0x27,
    /// <summary></summary>
    Down = 0x28,
    /// <summary></summary>
    Select = 0x29,
    /// <summary></summary>
    Print = 0x2A,
    /// <summary></summary>
    Execute = 0x2B,
    /// <summary></summary>
    Snapshot = 0x2C,
    /// <summary></summary>
    Insert = 0x2D,
    /// <summary></summary>
    Delete = 0x2E,
    /// <summary></summary>
    Help = 0x2F,
    /// <summary></summary>
    D0 = 0x30,
    /// <summary></summary>
    D1 = 0x31,
    /// <summary></summary>
    D2 = 0x32,
    /// <summary></summary>
    D3 = 0x33,
    /// <summary></summary>
    D4 = 0x34,
    /// <summary></summary>
    D5 = 0x35,
    /// <summary></summary>
    D6 = 0x36,
    /// <summary></summary>
    D7 = 0x37,
    /// <summary></summary>
    D8 = 0x38,
    /// <summary></summary>
    D9 = 0x39,
    /// <summary></summary>
    A = 0x41, //=65?
    /// <summary></summary>
    B = 0x42,
    /// <summary></summary>
    C = 0x43,
    /// <summary></summary>
    D = 0x44,
    /// <summary></summary>
    E = 0x45,
    /// <summary></summary>
    F = 0x46,
    /// <summary></summary>
    G = 0x47,
    /// <summary></summary>
    H = 0x48,
    /// <summary></summary>
    I = 0x49,
    /// <summary></summary>
    J = 0x4A,
    /// <summary></summary>
    K = 0x4B,
    /// <summary></summary>
    L = 0x4C,
    /// <summary></summary>
    M = 0x4D,
    /// <summary></summary>
    N = 0x4E,
    /// <summary></summary>
    O = 0x4F,
    /// <summary></summary>
    P = 0x50,
    /// <summary></summary>
    Q = 0x51,
    /// <summary></summary>
    R = 0x52,
    /// <summary></summary>
    S = 0x53,
    /// <summary></summary>
    T = 0x54,
    /// <summary></summary>
    U = 0x55,
    /// <summary></summary>
    V = 0x56,
    /// <summary></summary>
    W = 0x57,
    /// <summary></summary>
    X = 0x58,
    /// <summary></summary>
    Y = 0x59,
    /// <summary></summary>
    Z = 0x5A,
    /// <summary></summary>
    LeftWindows = 0x5B,
    /// <summary></summary>
    RightWindows = 0x5C,
    /// <summary></summary>
    Application = 0x5D,
    /// <summary></summary>
    Sleep = 0x5F,
    /// <summary></summary>
    Numpad0 = 0x60,
    /// <summary></summary>
    Numpad1 = 0x61,
    /// <summary></summary>
    Numpad2 = 0x62,
    /// <summary></summary>
    Numpad3 = 0x63,
    /// <summary></summary>
    Numpad4 = 0x64,
    /// <summary></summary>
    Numpad5 = 0x65,
    /// <summary></summary>
    Numpad6 = 0x66,
    /// <summary></summary>
    Numpad7 = 0x67,
    /// <summary></summary>
    Numpad8 = 0x68,
    /// <summary></summary>
    Numpad9 = 0x69,
    /// <summary></summary>
    Multiply = 0x6A,
    /// <summary></summary>
    Add = 0x6B,
    /// <summary></summary>
    Separator = 0x6C,
    /// <summary></summary>
    Subtract = 0x6D,
    /// <summary></summary>
    Decimal = 0x6E,
    /// <summary></summary>
    Divide = 0x6F,
    /// <summary></summary>
    F1 = 0x70,
    /// <summary></summary>
    F2 = 0x71,
    /// <summary></summary>
    F3 = 0x72,
    /// <summary></summary>
    F4 = 0x73,
    /// <summary></summary>
    F5 = 0x74,
    /// <summary></summary>
    F6 = 0x75,
    /// <summary></summary>
    F7 = 0x76,
    /// <summary></summary>
    F8 = 0x77,
    /// <summary></summary>
    F9 = 0x78,
    /// <summary></summary>
    F10 = 0x79,
    /// <summary></summary>
    F11 = 0x7A,
    /// <summary></summary>
    F12 = 0x7B,
    /// <summary></summary>
    F13 = 0x7C,
    /// <summary></summary>
    F14 = 0x7D,
    /// <summary></summary>
    F15 = 0x7E,
    /// <summary></summary>
    F16 = 0x7F,
    /// <summary></summary>
    F17 = 0x80,
    /// <summary></summary>
    F18 = 0x81,
    /// <summary></summary>
    F19 = 0x82,
    /// <summary></summary>
    F20 = 0x83,
    /// <summary></summary>
    F21 = 0x84,
    /// <summary></summary>
    F22 = 0x85,
    /// <summary></summary>
    F23 = 0x86,
    /// <summary></summary>
    F24 = 0x87,
    /// <summary></summary>
    NumLock = 0x90,
    /// <summary></summary>
    ScrollLock = 0x91,
    /// <summary></summary>
    NEC_Equal = 0x92,
    /// <summary></summary>
    Fujitsu_Jisho = 0x92,
    /// <summary></summary>
    Fujitsu_Masshou = 0x93,
    /// <summary></summary>
    Fujitsu_Touroku = 0x94,
    /// <summary></summary>
    Fujitsu_Loya = 0x95,
    /// <summary></summary>
    Fujitsu_Roya = 0x96,
    /// <summary></summary>
    LeftShift = 0xA0,
    /// <summary></summary>
    RightShift = 0xA1,
    /// <summary></summary>
    LeftControl = 0xA2,
    /// <summary></summary>
    RightControl = 0xA3,
    /// <summary></summary>
    LeftMenu = 0xA4,
    /// <summary></summary>
    RightMenu = 0xA5,
    /// <summary></summary>
    BrowserBack = 0xA6,
    /// <summary></summary>
    BrowserForward = 0xA7,
    /// <summary></summary>
    BrowserRefresh = 0xA8,
    /// <summary></summary>
    BrowserStop = 0xA9,
    /// <summary></summary>
    BrowserSearch = 0xAA,
    /// <summary></summary>
    BrowserFavorites = 0xAB,
    /// <summary></summary>
    BrowserHome = 0xAC,
    /// <summary></summary>
    VolumeMute = 0xAD,
    /// <summary></summary>
    VolumeDown = 0xAE,
    /// <summary></summary>
    VolumeUp = 0xAF,
    /// <summary></summary>
    MediaNextTrack = 0xB0,
    /// <summary></summary>
    MediaPrevTrack = 0xB1,
    /// <summary></summary>
    MediaStop = 0xB2,
    /// <summary></summary>
    MediaPlayPause = 0xB3,
    /// <summary></summary>
    LaunchMail = 0xB4,
    /// <summary></summary>
    LaunchMediaSelect = 0xB5,
    /// <summary></summary>
    LaunchApplication1 = 0xB6,
    /// <summary></summary>
    LaunchApplication2 = 0xB7,
    /// <summary></summary>
    OEM1 = 0xBA,
    /// <summary></summary>
    OEMPlus = 0xBB,
    /// <summary></summary>
    OEMComma = 0xBC,
    /// <summary></summary>
    OEMMinus = 0xBD,
    /// <summary></summary>
    OEMPeriod = 0xBE,
    /// <summary></summary>
    OEM2 = 0xBF,
    /// <summary></summary>
    OEM3 = 0xC0,
    /// <summary></summary>
    OEM4 = 0xDB,
    /// <summary></summary>
    OEM5 = 0xDC,
    /// <summary></summary>
    OEM6 = 0xDD,
    /// <summary></summary>
    OEM7 = 0xDE,
    /// <summary></summary>
    OEM8 = 0xDF,
    /// <summary></summary>
    OEMAX = 0xE1,
    /// <summary></summary>
    OEM102 = 0xE2,
    /// <summary></summary>
    ICOHelp = 0xE3,
    /// <summary></summary>
    ICO00 = 0xE4,
    /// <summary></summary>
    ProcessKey = 0xE5,
    /// <summary></summary>
    ICOClear = 0xE6,
    /// <summary></summary>
    Packet = 0xE7,
    /// <summary></summary>
    OEMReset = 0xE9,
    /// <summary></summary>
    OEMJump = 0xEA,
    /// <summary></summary>
    OEMPA1 = 0xEB,
    /// <summary></summary>
    OEMPA2 = 0xEC,
    /// <summary></summary>
    OEMPA3 = 0xED,
    /// <summary></summary>
    OEMWSCtrl = 0xEE,
    /// <summary></summary>
    OEMCUSel = 0xEF,
    /// <summary></summary>
    OEMATTN = 0xF0,
    /// <summary></summary>
    OEMFinish = 0xF1,
    /// <summary></summary>
    OEMCopy = 0xF2,
    /// <summary></summary>
    OEMAuto = 0xF3,
    /// <summary></summary>
    OEMENLW = 0xF4,
    /// <summary></summary>
    OEMBackTab = 0xF5,
    /// <summary></summary>
    ATTN = 0xF6,
    /// <summary></summary>
    CRSel = 0xF7,
    /// <summary></summary>
    EXSel = 0xF8,
    /// <summary></summary>
    EREOF = 0xF9,
    /// <summary></summary>
    Play = 0xFA,
    /// <summary></summary>
    Zoom = 0xFB,
    /// <summary></summary>
    Noname = 0xFC,
    /// <summary></summary>
    PA1 = 0xFD,
    /// <summary></summary>
    OEMClear = 0xFE
}
