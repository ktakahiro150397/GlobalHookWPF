﻿using System;
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

        public event EventHandler<HookKeyEventArgs> KeyHookAltKeyDown;
        public event EventHandler<HookKeyEventArgs> KeyHookAltKeyUp;

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

            if(inputCode == KeyboardUtilConstants.VirtualKeyCode.CapsLock ||
               inputCode == KeyboardUtilConstants.VirtualKeyCode.HiraganaKatakana)
            {
                //CapsLock・ひらがなかたかなは処理しない
            }
            else
            {
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
                    case KeyEvents.SKeyDown:
                        //Altキーダウンイベント発生
                        KeyHookAltKeyDown(this, e);
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
                            //一般キーアップイベントを発生
                            KeyHookKeyUp(this, e);
                        }
                        break;
                    case KeyEvents.SKeyUp:
                        KeyHookAltKeyUp(this, e);
                        break;
                }
            }

            

            //フック処理の終了
            return CallNextHookEx(HookId, Code, W, L);
        }
    }

    /// <summary>
    /// キー表記に関する定数を持つ静的クラス。
    /// </summary>
    public static class KeyboardUtilConstants
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
            Hyphen = 0xBD,
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

        //public static Dictionary<VirtualKeyCode, string> smallKeyNameDictionary;
        //public static Dictionary<VirtualKeyCode, string> bigKeyNameDictionary;

        public static List<KeyData> smallKeyNameDictionary;
        public static List<KeyData> bigKeyNameDictionary;

        /// <summary>
        /// 設定ファイルのKeyNameとキーコードの対応を持ちます。
        /// </summary>
        public static Dictionary<string, VirtualKeyCode> keyNameKeyCodeDictionary;

        /// <summary>
        /// ディクショナリの初期化。
        /// </summary>
        static KeyboardUtilConstants()
        {
            //キーコードから文字に変換するDictionaryを初期化する
            smallKeyNameDictionary = new List<KeyData>() {

                //数値
                new KeyData(VirtualKeyCode.Zero,           "0"),
                new KeyData(VirtualKeyCode.One,            "1"),
                new KeyData(VirtualKeyCode.Two,            "2"),
                new KeyData(VirtualKeyCode.Three,          "3"),
                new KeyData(VirtualKeyCode.Four,           "4"),
                new KeyData(VirtualKeyCode.Five,           "5"),
                new KeyData(VirtualKeyCode.Six,            "6"),
                new KeyData(VirtualKeyCode.Seven,          "7"),
                new KeyData(VirtualKeyCode.Eight,          "8"),
                new KeyData(VirtualKeyCode.Nine,           "9"),

                //アルファベット
                new KeyData(VirtualKeyCode.A,              "a"),
                new KeyData(VirtualKeyCode.B,              "b"),
                new KeyData(VirtualKeyCode.C,              "c"),
                new KeyData(VirtualKeyCode.D,              "d"),
                new KeyData(VirtualKeyCode.E,              "e"),
                new KeyData(VirtualKeyCode.F,              "f"),
                new KeyData(VirtualKeyCode.G,              "g"),
                new KeyData(VirtualKeyCode.H,              "h"),
                new KeyData(VirtualKeyCode.I,              "i"),
                new KeyData(VirtualKeyCode.J,              "j"),
                new KeyData(VirtualKeyCode.K,              "k"),
                new KeyData(VirtualKeyCode.L,              "l"),
                new KeyData(VirtualKeyCode.M,              "m"),
                new KeyData(VirtualKeyCode.N,              "n"),
                new KeyData(VirtualKeyCode.O,              "o"),
                new KeyData(VirtualKeyCode.P,              "p"),
                new KeyData(VirtualKeyCode.Q,              "q"),
                new KeyData(VirtualKeyCode.R,              "r"),
                new KeyData(VirtualKeyCode.S,              "s"),
                new KeyData(VirtualKeyCode.T,              "t"),
                new KeyData(VirtualKeyCode.U,              "u"),
                new KeyData(VirtualKeyCode.V,              "v"),
                new KeyData(VirtualKeyCode.W,              "w"),
                new KeyData(VirtualKeyCode.X,              "x"),
                new KeyData(VirtualKeyCode.Y,              "y"),
                new KeyData(VirtualKeyCode.Z,              "z"),

                ////テンキー
                new KeyData(VirtualKeyCode.NumZero,      "0"),
                new KeyData(VirtualKeyCode.NumOne,       "1"),
                new KeyData(VirtualKeyCode.NumTwo,       "2"),
                new KeyData(VirtualKeyCode.NumThree,     "3"),
                new KeyData(VirtualKeyCode.NumFour,      "4"),
                new KeyData(VirtualKeyCode.NumFive,      "5"),
                new KeyData(VirtualKeyCode.NumSix,       "6"),
                new KeyData(VirtualKeyCode.NumSeven,     "7"),
                new KeyData(VirtualKeyCode.NumEight,     "8"),
                new KeyData(VirtualKeyCode.NumNine,      "9"),
                new KeyData(VirtualKeyCode.Multiply,     "*"),
                new KeyData(VirtualKeyCode.Add,          "+"),
                new KeyData(VirtualKeyCode.Separate,     ","),
                new KeyData(VirtualKeyCode.Subtract,     "-"),
                new KeyData(VirtualKeyCode.Decimal,      "."),
                new KeyData(VirtualKeyCode.Devide,       "/"),

                //ファンクションキー
                new KeyData(VirtualKeyCode.F1            ,"[F1]"),
                new KeyData(VirtualKeyCode.F2            ,"[F2]"),
                new KeyData(VirtualKeyCode.F3            ,"[F3]"),
                new KeyData(VirtualKeyCode.F4            ,"[F4]"),
                new KeyData(VirtualKeyCode.F5            ,"[F5]"),
                new KeyData(VirtualKeyCode.F6            ,"[F6]"),
                new KeyData(VirtualKeyCode.F7            ,"[F7]"),
                new KeyData(VirtualKeyCode.F8            ,"[F8]"),
                new KeyData(VirtualKeyCode.F9            ,"[F9]"),
                new KeyData(VirtualKeyCode.F10           ,"[F10]"),
                new KeyData(VirtualKeyCode.F11           ,"[F11]"),
                new KeyData(VirtualKeyCode.F12           ,"[F12]"),
                new KeyData(VirtualKeyCode.F13           ,"[F13]"),
                new KeyData(VirtualKeyCode.F14           ,"[F14]"),
                new KeyData(VirtualKeyCode.F15           ,"[F15]"),
                new KeyData(VirtualKeyCode.F16           ,"[F16]"),
                new KeyData(VirtualKeyCode.F17           ,"[F17]"),
                new KeyData(VirtualKeyCode.F18           ,"[F18]"),
                new KeyData(VirtualKeyCode.F19           ,"[F19]"),
                new KeyData(VirtualKeyCode.F20           ,"[F20]"),
                new KeyData(VirtualKeyCode.F21           ,"[F21]"),
                new KeyData(VirtualKeyCode.F22           ,"[F22]"),
                new KeyData(VirtualKeyCode.F23           ,"[F23]"),
                new KeyData(VirtualKeyCode.F24           ,"[F24]"),

                //記号・特殊キー
                new KeyData(VirtualKeyCode.BackSpace,          "[BS]"),
                new KeyData(VirtualKeyCode.Tab,                "[Tab]"),
                new KeyData(VirtualKeyCode.Return,             "[Enter]"),
                new KeyData(VirtualKeyCode.Shift,              "[Shift]"),
                new KeyData(VirtualKeyCode.Control,            "[Ctrl]"),
                new KeyData(VirtualKeyCode.Alt,                "[Alt]"),
                new KeyData(VirtualKeyCode.Kana,               "[Kana]"),
                new KeyData(VirtualKeyCode.Kanji,              "[Kanji]"),
                new KeyData(VirtualKeyCode.Escape,             "[Esc]"),
                new KeyData(VirtualKeyCode.Space,              "[Space]"),
                new KeyData(VirtualKeyCode.PageUp,             "[PageUp]"),
                new KeyData(VirtualKeyCode.PageDown,           "[PageDown]"),
                new KeyData(VirtualKeyCode.End,                "[End]"),
                new KeyData(VirtualKeyCode.Home,               "[Home]"),
                new KeyData(VirtualKeyCode.LeftArrow,          "[←]"),
                new KeyData(VirtualKeyCode.UpArrow,            "[↑]"),
                new KeyData(VirtualKeyCode.RightArrow,         "[→]"),
                new KeyData(VirtualKeyCode.DownArrow,          "[↓]"),
                new KeyData(VirtualKeyCode.PrintScreen,        "[PrintScreen]"),
                new KeyData(VirtualKeyCode.Insert,             "[Insert]"),
                new KeyData(VirtualKeyCode.Delete,             "[Delete]"),
                new KeyData(VirtualKeyCode.LeftWin,            "[Win]"),
                new KeyData(VirtualKeyCode.RightWin,           "[Win]"),
                new KeyData(VirtualKeyCode.AppKey,             "[App]"),
                new KeyData(VirtualKeyCode.NumLock             ,"[NumLock]"),
                new KeyData(VirtualKeyCode.Scroll              ,"[ScrollLock]"),
                new KeyData(VirtualKeyCode.LeftShift           ,"[LShift]"),
                new KeyData(VirtualKeyCode.RightShift          ,"[RShift]"),
                new KeyData(VirtualKeyCode.LeftControl         ,"[LCtrl]"),
                new KeyData(VirtualKeyCode.RightControl        ,"[RCtrl]"),
                new KeyData(VirtualKeyCode.Pause               ,"[Pause]" ),
                new KeyData(VirtualKeyCode.Henkan              ,"[変換]" ),
                new KeyData(VirtualKeyCode.Muhenkan            ,"[無変換]" ),
                new KeyData(VirtualKeyCode.LeftAlt             ,"[LAlt]" ),
                new KeyData(VirtualKeyCode.RightAlt            ,"[RAlt]" ),
                new KeyData(VirtualKeyCode.Colon               ,":" ),
                new KeyData(VirtualKeyCode.SemiColon           ,";" ),
                new KeyData(VirtualKeyCode.Comma               ,"," ),
                new KeyData(VirtualKeyCode.Hyphen              ,"-" ),
                new KeyData(VirtualKeyCode.Period              ,"." ),
                new KeyData(VirtualKeyCode.Slash               ,"/" ),
                new KeyData(VirtualKeyCode.AMark               ,"@" ),
                new KeyData(VirtualKeyCode.LeftBlacket         ,"[" ),
                new KeyData(VirtualKeyCode.Yen                 ,"\\" ),
                new KeyData(VirtualKeyCode.RightBlacket        ,"]" ),
                new KeyData(VirtualKeyCode.Caret               ,"^" ),
                new KeyData(VirtualKeyCode.YenUnder            ,"\\" ),
                new KeyData(VirtualKeyCode.CapsLock            ,"[CapsLock]" ),
                new KeyData(VirtualKeyCode.HiraganaKatakana    ,"[ひらがな/かたかな]" ),
                new KeyData(VirtualKeyCode.FullToHalf          ,"[半角切替]" ),
                new KeyData(VirtualKeyCode.HalfToFull          ,"[全角切替]" ),
            };
            bigKeyNameDictionary = new List<KeyData>() {

                //数値
                new KeyData(VirtualKeyCode.Zero,           ""),
                new KeyData(VirtualKeyCode.One,            "!"),
                new KeyData(VirtualKeyCode.Two,            @""""),
                new KeyData(VirtualKeyCode.Three,          "#"),
                new KeyData(VirtualKeyCode.Four,           "$"),
                new KeyData(VirtualKeyCode.Five,           "%"),
                new KeyData(VirtualKeyCode.Six,            "&"),
                new KeyData(VirtualKeyCode.Seven,          "'"),
                new KeyData(VirtualKeyCode.Eight,          "("),
                new KeyData(VirtualKeyCode.Nine,           ")"),

                //アルファベット
                new KeyData(VirtualKeyCode.A,              "A"),
                new KeyData(VirtualKeyCode.B,              "B"),
                new KeyData(VirtualKeyCode.C,              "C"),
                new KeyData(VirtualKeyCode.D,              "D"),
                new KeyData(VirtualKeyCode.E,              "E"),
                new KeyData(VirtualKeyCode.F,              "F"),
                new KeyData(VirtualKeyCode.G,              "G"),
                new KeyData(VirtualKeyCode.H,              "H"),
                new KeyData(VirtualKeyCode.I,              "I"),
                new KeyData(VirtualKeyCode.J,              "J"),
                new KeyData(VirtualKeyCode.K,              "K"),
                new KeyData(VirtualKeyCode.L,              "L"),
                new KeyData(VirtualKeyCode.M,              "M"),
                new KeyData(VirtualKeyCode.N,              "N"),
                new KeyData(VirtualKeyCode.O,              "O"),
                new KeyData(VirtualKeyCode.P,              "P"),
                new KeyData(VirtualKeyCode.Q,              "Q"),
                new KeyData(VirtualKeyCode.R,              "R"),
                new KeyData(VirtualKeyCode.S,              "S"),
                new KeyData(VirtualKeyCode.T,              "T"),
                new KeyData(VirtualKeyCode.U,              "U"),
                new KeyData(VirtualKeyCode.V,              "V"),
                new KeyData(VirtualKeyCode.W,              "W"),
                new KeyData(VirtualKeyCode.X,              "X"),
                new KeyData(VirtualKeyCode.Y,              "Y"),
                new KeyData(VirtualKeyCode.Z,              "Z"),

                ////テンキー
                new KeyData(VirtualKeyCode.NumZero,      "0"),
                new KeyData(VirtualKeyCode.NumOne,       "1"),
                new KeyData(VirtualKeyCode.NumTwo,       "2"),
                new KeyData(VirtualKeyCode.NumThree,     "3"),
                new KeyData(VirtualKeyCode.NumFour,      "4"),
                new KeyData(VirtualKeyCode.NumFive,      "5"),
                new KeyData(VirtualKeyCode.NumSix,       "6"),
                new KeyData(VirtualKeyCode.NumSeven,     "7"),
                new KeyData(VirtualKeyCode.NumEight,     "8"),
                new KeyData(VirtualKeyCode.NumNine,      "9"),
                new KeyData(VirtualKeyCode.Multiply,     "*"),
                new KeyData(VirtualKeyCode.Add,          "+"),
                new KeyData(VirtualKeyCode.Separate,     ","),
                new KeyData(VirtualKeyCode.Subtract,     "-"),
                new KeyData(VirtualKeyCode.Decimal,      "."),
                new KeyData(VirtualKeyCode.Devide,       "/"),

                //ファンクションキー
                new KeyData(VirtualKeyCode.F1            ,"[F1]"),
                new KeyData(VirtualKeyCode.F2            ,"[F2]"),
                new KeyData(VirtualKeyCode.F3            ,"[F3]"),
                new KeyData(VirtualKeyCode.F4            ,"[F4]"),
                new KeyData(VirtualKeyCode.F5            ,"[F5]"),
                new KeyData(VirtualKeyCode.F6            ,"[F6]"),
                new KeyData(VirtualKeyCode.F7            ,"[F7]"),
                new KeyData(VirtualKeyCode.F8            ,"[F8]"),
                new KeyData(VirtualKeyCode.F9            ,"[F9]"),
                new KeyData(VirtualKeyCode.F10           ,"[F10]"),
                new KeyData(VirtualKeyCode.F11           ,"[F11]"),
                new KeyData(VirtualKeyCode.F12           ,"[F12]"),
                new KeyData(VirtualKeyCode.F13           ,"[F13]"),
                new KeyData(VirtualKeyCode.F14           ,"[F14]"),
                new KeyData(VirtualKeyCode.F15           ,"[F15]"),
                new KeyData(VirtualKeyCode.F16           ,"[F16]"),
                new KeyData(VirtualKeyCode.F17           ,"[F17]"),
                new KeyData(VirtualKeyCode.F18           ,"[F18]"),
                new KeyData(VirtualKeyCode.F19           ,"[F19]"),
                new KeyData(VirtualKeyCode.F20           ,"[F20]"),
                new KeyData(VirtualKeyCode.F21           ,"[F21]"),
                new KeyData(VirtualKeyCode.F22           ,"[F22]"),
                new KeyData(VirtualKeyCode.F23           ,"[F23]"),
                new KeyData(VirtualKeyCode.F24           ,"[F24]"),

                //記号・特殊キー
                new KeyData(VirtualKeyCode.BackSpace,          "[BS]"),
                new KeyData(VirtualKeyCode.Tab,                "[Tab]"),
                new KeyData(VirtualKeyCode.Return,             "[Enter]"),
                new KeyData(VirtualKeyCode.Shift,              "[Shift]"),
                new KeyData(VirtualKeyCode.Control,            "[Ctrl]"),
                new KeyData(VirtualKeyCode.Alt,                "[Alt]"),
                new KeyData(VirtualKeyCode.Kana,               "[Kana]"),
                new KeyData(VirtualKeyCode.Kanji,              "[Kanji]"),
                new KeyData(VirtualKeyCode.Escape,             "[Esc]"),
                new KeyData(VirtualKeyCode.Space,              "[Space]"),
                new KeyData(VirtualKeyCode.PageUp,             "[PageUp]"),
                new KeyData(VirtualKeyCode.PageDown,           "[PageDown]"),
                new KeyData(VirtualKeyCode.End,                "[End]"),
                new KeyData(VirtualKeyCode.Home,               "[Home]"),
                new KeyData(VirtualKeyCode.LeftArrow,          "[←]"),
                new KeyData(VirtualKeyCode.UpArrow,            "[↑]"),
                new KeyData(VirtualKeyCode.RightArrow,         "[→]"),
                new KeyData(VirtualKeyCode.DownArrow,          "[↓]"),
                new KeyData(VirtualKeyCode.PrintScreen,        "[PrintScreen]"),
                new KeyData(VirtualKeyCode.Insert,             "[Insert]"),
                new KeyData(VirtualKeyCode.Delete,             "[Delete]"),
                new KeyData(VirtualKeyCode.LeftWin,            "[Win]"),
                new KeyData(VirtualKeyCode.RightWin,           "[Win]"),
                new KeyData(VirtualKeyCode.AppKey,             "[App]"),
                new KeyData(VirtualKeyCode.NumLock             ,"[NumLock]"),
                new KeyData(VirtualKeyCode.Scroll              ,"[ScrollLock]"),
                new KeyData(VirtualKeyCode.LeftShift           ,"[LShift]"),
                new KeyData(VirtualKeyCode.RightShift          ,"[RShift]"),
                new KeyData(VirtualKeyCode.LeftControl         ,"[LCtrl]"),
                new KeyData(VirtualKeyCode.RightControl        ,"[RCtrl]"),
                new KeyData(VirtualKeyCode.Pause               ,"[Pause]" ),
                new KeyData(VirtualKeyCode.Henkan              ,"[変換]" ),
                new KeyData(VirtualKeyCode.Muhenkan            ,"[無変換]" ),
                new KeyData(VirtualKeyCode.LeftAlt             ,"[LAlt]" ),
                new KeyData(VirtualKeyCode.RightAlt            ,"[RAlt]" ),
                new KeyData(VirtualKeyCode.Colon               ,"*" ),
                new KeyData(VirtualKeyCode.SemiColon           ,"+" ),
                new KeyData(VirtualKeyCode.Comma               ,"<" ),
                new KeyData(VirtualKeyCode.Hyphen              ,"=" ),
                new KeyData(VirtualKeyCode.Period              ,">" ),
                new KeyData(VirtualKeyCode.Slash               ,"?" ),
                new KeyData(VirtualKeyCode.AMark               ,"`" ),
                new KeyData(VirtualKeyCode.LeftBlacket         ,"new KeyData(" ),
                new KeyData(VirtualKeyCode.Yen                 ,"|" ),
                new KeyData(VirtualKeyCode.RightBlacket        ,"}" ),
                new KeyData(VirtualKeyCode.Caret               ,"~" ),
                new KeyData(VirtualKeyCode.YenUnder            ,"_" ),
                new KeyData(VirtualKeyCode.CapsLock            ,"[CapsLock]" ),
                new KeyData(VirtualKeyCode.HiraganaKatakana    ,"[ひらがな/かたかな]" ),
                new KeyData(VirtualKeyCode.FullToHalf          ,"[半角切替]" ),
                new KeyData(VirtualKeyCode.HalfToFull          ,"[全角切替]" ),
            };

            //設定ファイルのKeyNameとキーコードの対応初期化
            keyNameKeyCodeDictionary = new Dictionary<string, VirtualKeyCode>()
            {
                {"Escape",VirtualKeyCode.Escape },
                {"F1",VirtualKeyCode.F1 },
                {"F2",VirtualKeyCode.F2 },
            };
        }
    }

    /// <summary>
    /// キーコードと表示文字列の対応を持つクラスです。
    /// </summary>
    public class KeyData
    {
        //TODO : 大文字・小文字をどちらも持つようにする

        public KeyboardUtilConstants.VirtualKeyCode KeyCode { get; }
        public string DisplayString { get; }


        public KeyData(KeyboardUtilConstants.VirtualKeyCode keyCode, string displayString)
        {
            KeyCode = keyCode;
            DisplayString = displayString;
        }
    }
}
