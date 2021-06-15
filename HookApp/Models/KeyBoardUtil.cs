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

}
