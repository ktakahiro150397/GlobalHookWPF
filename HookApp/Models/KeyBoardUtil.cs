using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HookApp.ViewModels;

namespace HookApp.Models
{
    /// <summary>
    /// キーボードのグローバルフックを行い、キーイベントを通知します。
    /// </summary>
    class KeyboardUtil : IDisposable
    {
        /// <summary>
        /// イベント処理を行うデリゲート。
        /// </summary>
        /// <param name="sender">このイベントが発生したインスタンス。</param>
        /// <param name="e">キー押下情報が格納されているイベント引数。</param>
        delegate void HookKeyEventHandler(object sender, HookKeyEventArgs e);

        /// <summary>
        /// キーボード入力のイベント引数。
        /// </summary>
        public class HookKeyEventArgs : EventArgs
        {
            /// <summary>
            /// このイベントの種類。
            /// </summary>
            public KeyEvents Event { get; set; }

            /// <summary>
            /// このキー入力についての生データ。
            /// </summary>
            public KBDLLHookStruct rawData { get; set; }

            /// <summary>
            /// このキーのコード。
            /// </summary>
            public KeyboardUtilConstants.VirtualKeyCode vkCode { get; set; }

            public HookKeyEventArgs(KeyEvents Event, KBDLLHookStruct data, KeyboardUtilConstants.VirtualKeyCode vkCode)
            {
                this.Event = Event;
                this.rawData = data;
                this.vkCode = vkCode;
            }
        }

        /// <summary>
        /// キーボード入力イベント(ダウン)。
        /// </summary>
        public event EventHandler<HookKeyEventArgs> KeyHookKeyDown;

        /// <summary>
        /// キーボード入力イベント(アップ)。
        /// </summary>
        public event EventHandler<HookKeyEventArgs> KeyHookKeyUp;

        /// <summary>
        /// シフトキー入力イベント(ダウン)。
        /// </summary>
        public event EventHandler<HookKeyEventArgs> KeyHookShiftKeyDown;

        /// <summary>
        /// シフトキー入力イベント(アップ)。
        /// </summary>
        public event EventHandler<HookKeyEventArgs> KeyHookShiftKeyUp;

        //デリゲート・列挙体定義

        /// <summary>
        /// キー入力のコールバック処理を担当するデリゲート。
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="w"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        public delegate int HookCallback(int Code, IntPtr w, IntPtr L);

        /// <summary>
        /// キーボードフック時の構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHookStruct
        {
            public Int32 vkCode;
            public Int32 scanCode;
            public Int32 flags;
            public Int32 time;
            public Int32 dwExtraInfo;
        }

        /// <summary>
        /// フックタイプ
        /// </summary>
        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        /// <summary>
        /// キーイベント
        /// </summary>
        public enum KeyEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        //Win32API群
        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(HookType idHook, HookCallback lpfn, IntPtr hInstance, uint threadId);
        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        public static extern int UnhookWindowsHookEx(int idHook);
        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetCurrentThreadId();

        //プライベート変数群
        private int HookId = 0;
        private HookCallback CallbackFunc = null;

        /// <summary>
        /// グローバルフック初期化
        /// </summary>
        /// <param name="callback">フックプロシージャ</param>
        public KeyboardUtil(MainWindowViewModel vm)
        {
            this.IsShiftPress = false;
            this.CallbackFunc = KeyboardHookProc;

            //フックIDの取得
            this.HookId = SetWindowsHookEx(HookType.WH_KEYBOARD_LL,
                                           this.CallbackFunc,
                                           IntPtr.Zero,
                                           0);

        }

        /// <summary>
        /// このインスタンスが破棄されているかどうかを表すフラグ。
        /// </summary>
        private bool isFinalized = false;

        /// <summary>
        /// フックを解放します。
        /// </summary>
        ~KeyboardUtil()
        {
            //デストラクタ呼び出し時、フックを解放
            if (!isFinalized)
            {
                UnhookWindowsHookEx(this.HookId);
                isFinalized = true;
            }
        }

        /// <summary>
        /// フックを解放します。
        /// </summary>
        public void Dispose()
        {
            //呼び出し時、フックを解放
            if (!isFinalized)
            {
                UnhookWindowsHookEx(this.HookId);
                isFinalized = true;
            }
        }

        public bool IsShiftPress { get; }

        /// <summary>
        /// キーボードフックのプロシージャ
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="W"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        public int KeyboardHookProc(int Code, IntPtr W, IntPtr L)
        {
            if (Code < 0)
            {
                //Codeが0より小さい場合、次のフックを呼び出す
                return CallNextHookEx(HookId, Code, W, L);
            }

            //キー入力を取得
            KeyEvents procEvent = (KeyEvents)W;
            var kbStruct = Marshal.PtrToStructure<KBDLLHookStruct>(L);

            //入力文字の取得
            var inputCode = (KeyboardUtilConstants.VirtualKeyCode)kbStruct.vkCode;

            HookKeyEventArgs e = new HookKeyEventArgs(procEvent, kbStruct, inputCode);

            switch (procEvent)
            {
                case KeyEvents.KeyDown:
                    if (inputCode == KeyboardUtilConstants.VirtualKeyCode.Shift ||
                       inputCode == KeyboardUtilConstants.VirtualKeyCode.LeftShift ||
                       inputCode == KeyboardUtilConstants.VirtualKeyCode.RightShift
                        )
                    {
                        //シフトキーダウンイベントを発生
                        KeyHookShiftKeyDown(this, e);
                    }
                    else
                    {
                        //一般キーダウンイベントを発生
                        KeyHookKeyDown(this, e);
                    }
                    break;

                case KeyEvents.KeyUp:
                    if (inputCode == KeyboardUtilConstants.VirtualKeyCode.Shift ||
                       inputCode == KeyboardUtilConstants.VirtualKeyCode.LeftShift ||
                       inputCode == KeyboardUtilConstants.VirtualKeyCode.RightShift
                        )
                    {
                        //シフトキーダウンイベントを発生
                        KeyHookShiftKeyUp(this, e);
                    }
                    else
                    {
                        //一般キーダウンイベントを発生
                        KeyHookKeyUp(this, e);
                    }
                    break;


            }

            //フック処理の終了
            return CallNextHookEx(HookId, Code, W, L);
        }
    }

    /// <summary>
    /// キー表記に関する定数を持つ静的クラス。
    /// </summary>
    static class KeyboardUtilConstants
    {
        /// <summary>
        /// 仮想キーコード
        /// </summary>
        public enum VirtualKeyCode
        {
            //記号・特殊キー
            BackSpace = 0x08,
            Tab = 0x09,
            Return = 0x0D,
            Shift = 0x10,
            Control = 0x11,
            Alt = 0x12,
            Pause = 0x13,
            Kana = 0x15,
            Kanji = 0x19,
            Escape = 0x1B,
            Henkan = 0x1C,
            Muhenkan = 0x1D,
            Space = 0x20,
            PageUp = 0x21,
            PageDown = 0x22,
            End = 0x23,
            Home = 0x24,
            LeftArrow = 0x25,
            UpArrow = 0x26,
            RightArrow = 0x27,
            DownArrow = 0x28,
            PrintScreen = 0x2C,
            Insert = 0x2D,
            Delete = 0x2E,

            //数値
            Zero = 0x30,
            One = 0x31,
            Two = 0x32,
            Three = 0x33,
            Four = 0x34,
            Five = 0x35,
            Six = 0x36,
            Seven = 0x37,
            Eight = 0x38,
            Nine = 0x39,

            //アルファベット
            A = 0x41,
            B = 0x42,
            C = 0x43,
            D = 0x44,
            E = 0x45,
            F = 0x46,
            G = 0x47,
            H = 0x48,
            I = 0x49,
            J = 0x4A,
            K = 0x4B,
            L = 0x4C,
            M = 0x4D,
            N = 0x4E,
            O = 0x4F,
            P = 0x50,
            Q = 0x51,
            R = 0x52,
            S = 0x53,
            T = 0x54,
            U = 0x55,
            V = 0x56,
            W = 0x57,
            X = 0x58,
            Y = 0x59,
            Z = 0x5A,

            //特殊キー
            LeftWin = 0x5B,
            RightWin = 0x5C,
            AppKey = 0x5D,

            //テンキー
            NumZero = 0x60,
            NumOne = 0x61,
            NumTwo = 0x62,
            NumThree = 0x63,
            NumFour = 0x64,
            NumFive = 0x65,
            NumSix = 0x66,
            NumSeven = 0x67,
            NumEight = 0x68,
            NumNine = 0x69,
            Multiply = 0x6A,
            Add = 0x6B,
            Separate = 0x6C,
            Subtract = 0x6D,
            Decimal = 0x6E,
            Devide = 0x6F,

            //ファンクションキー
            F1 = 0x70,
            F2 = 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 0x7B,
            F13 = 0x7C,
            F14 = 0x7D,
            F15 = 0x7E,
            F16 = 0x7F,
            F17 = 0x80,
            F18 = 0x81,
            F19 = 0x82,
            F20 = 0x83,
            F21 = 0x84,
            F22 = 0x85,
            F23 = 0x86,
            F24 = 0x87,

            //特殊キー
            NumLock = 0x90,
            Scroll = 0x91,
            LeftShift = 0xA0,
            RightShift = 0xA1,
            LeftControl = 0xA2,
            RightControl = 0xA3,
            LeftAlt = 0xA4,
            RightAlt = 0xA5,

            //記号
            Colon = 0xBA,
            SemiColon = 0xBB,
            Comma = 0xBC,
            hyphen = 0xBD,
            Period = 0xBE,
            Slash = 0xBF,
            AMark = 0xC0,
            LeftBlacket = 0xDB,
            Yen = 0xDC,
            RightBlacket = 0xDD,
            Caret = 0xDE,
            YenUnder = 0xE2,
            CapsLock = 0xF0,
            HiraganaKatakana = 0xF2,
            FullToHalf = 0xF3,
            HalfToFull = 0xF4
        }

        public static Dictionary<VirtualKeyCode, string> smallKeyNameDictionary;
        public static Dictionary<VirtualKeyCode, string> bigKeyNameDictionary;

        /// <summary>
        /// ディクショナリの初期化。
        /// </summary>
        static KeyboardUtilConstants()
        {
            //キーコードから文字に変換するDictionaryを初期化する
            smallKeyNameDictionary = new Dictionary<VirtualKeyCode, string>() {

                //数値
                {VirtualKeyCode.Zero,           "0"},
                {VirtualKeyCode.One,            "1"},
                {VirtualKeyCode.Two,            "2"},
                {VirtualKeyCode.Three,          "3"},
                {VirtualKeyCode.Four,           "4"},
                {VirtualKeyCode.Five,           "5"},
                {VirtualKeyCode.Six,            "6"},
                {VirtualKeyCode.Seven,          "7"},
                {VirtualKeyCode.Eight,          "8"},
                {VirtualKeyCode.Nine,           "9"},

                //アルファベット
                {VirtualKeyCode.A,              "a"},
                {VirtualKeyCode.B,              "b"},
                {VirtualKeyCode.C,              "c"},
                {VirtualKeyCode.D,              "d"},
                {VirtualKeyCode.E,              "e"},
                {VirtualKeyCode.F,              "f"},
                {VirtualKeyCode.G,              "g"},
                {VirtualKeyCode.H,              "h"},
                {VirtualKeyCode.I,              "i"},
                {VirtualKeyCode.J,              "j"},
                {VirtualKeyCode.K,              "k"},
                {VirtualKeyCode.L,              "l"},
                {VirtualKeyCode.M,              "m"},
                {VirtualKeyCode.N,              "n"},
                {VirtualKeyCode.O,              "o"},
                {VirtualKeyCode.P,              "p"},
                {VirtualKeyCode.Q,              "q"},
                {VirtualKeyCode.R,              "r"},
                {VirtualKeyCode.S,              "s"},
                {VirtualKeyCode.T,              "t"},
                {VirtualKeyCode.U,              "u"},
                {VirtualKeyCode.V,              "v"},
                {VirtualKeyCode.W,              "w"},
                {VirtualKeyCode.X,              "x"},
                {VirtualKeyCode.Y,              "y"},
                {VirtualKeyCode.Z,              "z"},

                ////テンキー
                {VirtualKeyCode.NumZero,      "0"},
                {VirtualKeyCode.NumOne,       "1"},
                {VirtualKeyCode.NumTwo,       "2"},
                {VirtualKeyCode.NumThree,     "3"},
                {VirtualKeyCode.NumFour,      "4"},
                {VirtualKeyCode.NumFive,      "5"},
                {VirtualKeyCode.NumSix,       "6"},
                {VirtualKeyCode.NumSeven,     "7"},
                {VirtualKeyCode.NumEight,     "8"},
                {VirtualKeyCode.NumNine,      "9"},
                {VirtualKeyCode.Multiply,     "*"},
                {VirtualKeyCode.Add,          "+"},
                {VirtualKeyCode.Separate,     ","},
                {VirtualKeyCode.Subtract,     "-"},
                {VirtualKeyCode.Decimal,      "."},
                {VirtualKeyCode.Devide,       "/"},

                //ファンクションキー
                {VirtualKeyCode.F1            ,"[F1]"},
                {VirtualKeyCode.F2            ,"[F2]"},
                {VirtualKeyCode.F3            ,"[F3]"},
                {VirtualKeyCode.F4            ,"[F4]"},
                {VirtualKeyCode.F5            ,"[F5]"},
                {VirtualKeyCode.F6            ,"[F6]"},
                {VirtualKeyCode.F7            ,"[F7]"},
                {VirtualKeyCode.F8            ,"[F8]"},
                {VirtualKeyCode.F9            ,"[F9]"},
                {VirtualKeyCode.F10           ,"[F10]"},
                {VirtualKeyCode.F11           ,"[F11]"},
                {VirtualKeyCode.F12           ,"[F12]"},
                {VirtualKeyCode.F13           ,"[F13]"},
                {VirtualKeyCode.F14           ,"[F14]"},
                {VirtualKeyCode.F15           ,"[F15]"},
                {VirtualKeyCode.F16           ,"[F16]"},
                {VirtualKeyCode.F17           ,"[F17]"},
                {VirtualKeyCode.F18           ,"[F18]"},
                {VirtualKeyCode.F19           ,"[F19]"},
                {VirtualKeyCode.F20           ,"[F20]"},
                {VirtualKeyCode.F21           ,"[F21]"},
                {VirtualKeyCode.F22           ,"[F22]"},
                {VirtualKeyCode.F23           ,"[F23]"},
                {VirtualKeyCode.F24           ,"[F24]"},

                //記号・特殊キー
                {VirtualKeyCode.BackSpace,          "[BS]"},
                {VirtualKeyCode.Tab,                "[Tab]"},
                {VirtualKeyCode.Return,             "[Enter]"},
                {VirtualKeyCode.Shift,              "[Shift]"},
                {VirtualKeyCode.Control,            "[Ctrl]"},
                {VirtualKeyCode.Alt,                "[Alt]"},
                {VirtualKeyCode.Kana,               "[Kana]"},
                {VirtualKeyCode.Kanji,              "[Kanji]"},
                {VirtualKeyCode.Escape,             "[Esc]"},
                {VirtualKeyCode.Space,              "[Space]"},
                {VirtualKeyCode.PageUp,             "[PageUp]"},
                {VirtualKeyCode.PageDown,           "[PageDown]"},
                {VirtualKeyCode.End,                "[End]"},
                {VirtualKeyCode.Home,               "[Home]"},
                {VirtualKeyCode.LeftArrow,          "[←]"},
                {VirtualKeyCode.UpArrow,            "[↑]"},
                {VirtualKeyCode.RightArrow,         "[→]"},
                {VirtualKeyCode.DownArrow,          "[↓]"},
                {VirtualKeyCode.PrintScreen,        "[PrintScreen]"},
                {VirtualKeyCode.Insert,             "[Insert]"},
                {VirtualKeyCode.Delete,             "[Delete]"},
                {VirtualKeyCode.LeftWin,            "[Win]"},
                {VirtualKeyCode.RightWin,           "[Win]"},
                {VirtualKeyCode.AppKey,             "[App]"},
                {VirtualKeyCode.NumLock             ,"[NumLock]"},
                {VirtualKeyCode.Scroll              ,"[ScrollLock]"},
                {VirtualKeyCode.LeftShift           ,"[LShift]"},
                {VirtualKeyCode.RightShift          ,"[RShift]"},
                {VirtualKeyCode.LeftControl         ,"[LCtrl]"},
                {VirtualKeyCode.RightControl        ,"[RCtrl]"},
                {VirtualKeyCode.Pause               ,"[Pause]" },
                {VirtualKeyCode.Henkan              ,"[変換]" },
                {VirtualKeyCode.Muhenkan            ,"[無変換]" },
                {VirtualKeyCode.LeftAlt             ,"[LAlt]" },
                {VirtualKeyCode.RightAlt            ,"[RAlt]" },
                {VirtualKeyCode.Colon               ,":" },
                {VirtualKeyCode.SemiColon           ,";" },
                {VirtualKeyCode.Comma               ,"," },
                {VirtualKeyCode.hyphen              ,"-" },
                {VirtualKeyCode.Period              ,"." },
                {VirtualKeyCode.Slash               ,"/" },
                {VirtualKeyCode.AMark               ,"@" },
                {VirtualKeyCode.LeftBlacket         ,"[" },
                {VirtualKeyCode.Yen                 ,"\\" },
                {VirtualKeyCode.RightBlacket        ,"]" },
                {VirtualKeyCode.Caret               ,"^" },
                {VirtualKeyCode.YenUnder            ,"\\" },
                {VirtualKeyCode.CapsLock            ,"[CapsLock]" },
                {VirtualKeyCode.HiraganaKatakana    ,"[ひらがな/かたかな]" },
                {VirtualKeyCode.FullToHalf          ,"[半角切替]" },
                {VirtualKeyCode.HalfToFull          ,"[全角切替]" },
            };

            bigKeyNameDictionary = new Dictionary<VirtualKeyCode, string>() {

                //数値
                {VirtualKeyCode.Zero,           ""},
                {VirtualKeyCode.One,            "!"},
                {VirtualKeyCode.Two,            @""""},
                {VirtualKeyCode.Three,          "#"},
                {VirtualKeyCode.Four,           "$"},
                {VirtualKeyCode.Five,           "%"},
                {VirtualKeyCode.Six,            "&"},
                {VirtualKeyCode.Seven,          "'"},
                {VirtualKeyCode.Eight,          "("},
                {VirtualKeyCode.Nine,           ")"},

                //アルファベット
                {VirtualKeyCode.A,              "A"},
                {VirtualKeyCode.B,              "B"},
                {VirtualKeyCode.C,              "C"},
                {VirtualKeyCode.D,              "D"},
                {VirtualKeyCode.E,              "E"},
                {VirtualKeyCode.F,              "F"},
                {VirtualKeyCode.G,              "G"},
                {VirtualKeyCode.H,              "H"},
                {VirtualKeyCode.I,              "I"},
                {VirtualKeyCode.J,              "J"},
                {VirtualKeyCode.K,              "K"},
                {VirtualKeyCode.L,              "L"},
                {VirtualKeyCode.M,              "M"},
                {VirtualKeyCode.N,              "N"},
                {VirtualKeyCode.O,              "O"},
                {VirtualKeyCode.P,              "P"},
                {VirtualKeyCode.Q,              "Q"},
                {VirtualKeyCode.R,              "R"},
                {VirtualKeyCode.S,              "S"},
                {VirtualKeyCode.T,              "T"},
                {VirtualKeyCode.U,              "U"},
                {VirtualKeyCode.V,              "V"},
                {VirtualKeyCode.W,              "W"},
                {VirtualKeyCode.X,              "X"},
                {VirtualKeyCode.Y,              "Y"},
                {VirtualKeyCode.Z,              "Z"},

                ////テンキー
                {VirtualKeyCode.NumZero,      "0"},
                {VirtualKeyCode.NumOne,       "1"},
                {VirtualKeyCode.NumTwo,       "2"},
                {VirtualKeyCode.NumThree,     "3"},
                {VirtualKeyCode.NumFour,      "4"},
                {VirtualKeyCode.NumFive,      "5"},
                {VirtualKeyCode.NumSix,       "6"},
                {VirtualKeyCode.NumSeven,     "7"},
                {VirtualKeyCode.NumEight,     "8"},
                {VirtualKeyCode.NumNine,      "9"},
                {VirtualKeyCode.Multiply,     "*"},
                {VirtualKeyCode.Add,          "+"},
                {VirtualKeyCode.Separate,     ","},
                {VirtualKeyCode.Subtract,     "-"},
                {VirtualKeyCode.Decimal,      "."},
                {VirtualKeyCode.Devide,       "/"},

                //ファンクションキー
                {VirtualKeyCode.F1            ,"[F1]"},
                {VirtualKeyCode.F2            ,"[F2]"},
                {VirtualKeyCode.F3            ,"[F3]"},
                {VirtualKeyCode.F4            ,"[F4]"},
                {VirtualKeyCode.F5            ,"[F5]"},
                {VirtualKeyCode.F6            ,"[F6]"},
                {VirtualKeyCode.F7            ,"[F7]"},
                {VirtualKeyCode.F8            ,"[F8]"},
                {VirtualKeyCode.F9            ,"[F9]"},
                {VirtualKeyCode.F10           ,"[F10]"},
                {VirtualKeyCode.F11           ,"[F11]"},
                {VirtualKeyCode.F12           ,"[F12]"},
                {VirtualKeyCode.F13           ,"[F13]"},
                {VirtualKeyCode.F14           ,"[F14]"},
                {VirtualKeyCode.F15           ,"[F15]"},
                {VirtualKeyCode.F16           ,"[F16]"},
                {VirtualKeyCode.F17           ,"[F17]"},
                {VirtualKeyCode.F18           ,"[F18]"},
                {VirtualKeyCode.F19           ,"[F19]"},
                {VirtualKeyCode.F20           ,"[F20]"},
                {VirtualKeyCode.F21           ,"[F21]"},
                {VirtualKeyCode.F22           ,"[F22]"},
                {VirtualKeyCode.F23           ,"[F23]"},
                {VirtualKeyCode.F24           ,"[F24]"},

                //記号・特殊キー
                {VirtualKeyCode.BackSpace,          "[BS]"},
                {VirtualKeyCode.Tab,                "[Tab]"},
                {VirtualKeyCode.Return,             "[Enter]"},
                {VirtualKeyCode.Shift,              "[Shift]"},
                {VirtualKeyCode.Control,            "[Ctrl]"},
                {VirtualKeyCode.Alt,                "[Alt]"},
                {VirtualKeyCode.Kana,               "[Kana]"},
                {VirtualKeyCode.Kanji,              "[Kanji]"},
                {VirtualKeyCode.Escape,             "[Esc]"},
                {VirtualKeyCode.Space,              "[Space]"},
                {VirtualKeyCode.PageUp,             "[PageUp]"},
                {VirtualKeyCode.PageDown,           "[PageDown]"},
                {VirtualKeyCode.End,                "[End]"},
                {VirtualKeyCode.Home,               "[Home]"},
                {VirtualKeyCode.LeftArrow,          "[←]"},
                {VirtualKeyCode.UpArrow,            "[↑]"},
                {VirtualKeyCode.RightArrow,         "[→]"},
                {VirtualKeyCode.DownArrow,          "[↓]"},
                {VirtualKeyCode.PrintScreen,        "[PrintScreen]"},
                {VirtualKeyCode.Insert,             "[Insert]"},
                {VirtualKeyCode.Delete,             "[Delete]"},
                {VirtualKeyCode.LeftWin,            "[Win]"},
                {VirtualKeyCode.RightWin,           "[Win]"},
                {VirtualKeyCode.AppKey,             "[App]"},
                {VirtualKeyCode.NumLock             ,"[NumLock]"},
                {VirtualKeyCode.Scroll              ,"[ScrollLock]"},
                {VirtualKeyCode.LeftShift           ,"[LShift]"},
                {VirtualKeyCode.RightShift          ,"[RShift]"},
                {VirtualKeyCode.LeftControl         ,"[LCtrl]"},
                {VirtualKeyCode.RightControl        ,"[RCtrl]"},
                {VirtualKeyCode.Pause               ,"[Pause]" },
                {VirtualKeyCode.Henkan              ,"[変換]" },
                {VirtualKeyCode.Muhenkan            ,"[無変換]" },
                {VirtualKeyCode.LeftAlt             ,"[LAlt]" },
                {VirtualKeyCode.RightAlt            ,"[RAlt]" },
                {VirtualKeyCode.Colon               ,"*" },
                {VirtualKeyCode.SemiColon           ,"+" },
                {VirtualKeyCode.Comma               ,"<" },
                {VirtualKeyCode.hyphen              ,"=" },
                {VirtualKeyCode.Period              ,">" },
                {VirtualKeyCode.Slash               ,"?" },
                {VirtualKeyCode.AMark               ,"`" },
                {VirtualKeyCode.LeftBlacket         ,"{" },
                {VirtualKeyCode.Yen                 ,"|" },
                {VirtualKeyCode.RightBlacket        ,"}" },
                {VirtualKeyCode.Caret               ,"~" },
                {VirtualKeyCode.YenUnder            ,"_" },
                {VirtualKeyCode.CapsLock            ,"[CapsLock]" },
                {VirtualKeyCode.HiraganaKatakana    ,"[ひらがな/かたかな]" },
                {VirtualKeyCode.FullToHalf          ,"[半角切替]" },
                {VirtualKeyCode.HalfToFull          ,"[全角切替]" },
            };
        }
    }
}
