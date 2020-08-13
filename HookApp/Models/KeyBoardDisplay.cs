using System;
using System.Collections.Generic;
using System.Text;

namespace HookApp.Models
{
    /// <summary>
    /// キーボード表示についてのロジックを持ちます。
    /// </summary>
    class KeyBoardDisplay
    {
        public KeyBoardDisplay()
        {
            //キー情報の初期化

        }


    }

    /// <summary>
    /// 画面に表示されている1つのキー情報を表します。
    /// </summary>
    class DisplayKeyInfo
    {
        /// <summary>
        /// このキー画像の幅。
        /// </summary>
        public int picWidth { get; }

        /// <summary>
        /// このキー画像の高さ。
        /// </summary>
        public int picHeight { get; }

        /// <summary>
        /// このキーがプッシュ状態ならTrue。そうでなければFalse。
        /// </summary>
        public bool IsPress { get; }

        /// <summary>
        /// このキーボードのキー。
        /// </summary>
        public KeyKind Key { get; }

       
    }

    /// <summary>
    /// キーボードのキー種別を表します。
    /// </summary>
    enum KeyKind
    {
        Escape      ,
        F1          ,
        F2          ,
        F3          ,
        F4          ,
        F5          ,
        F6          ,
        F7          ,
        F8          ,
        F9          ,
        F10         ,
        F11         ,
        F12         ,

        ZenHan      ,
        One         ,
        Two         ,
        Three       ,
        Four        ,
        Five        ,
        Six         ,
        Seven       ,
        Eight       ,
        Nine        ,
        Zero        ,
        Hyphen      ,
        Tilda       ,
        UpperYen    ,
        BackSpace   ,

        Tab         ,
        Q           ,
        W           ,
        E           ,
        R           ,
        T           ,
        Y           ,
        U           ,
        I           ,
        O           ,
        P           ,
        AtMark      ,
        LeftBlacket ,
        Enter       ,
        CapsLock    ,
        A           ,
        S           ,
        D           ,
        F           ,
        G           ,
        H           ,
        J           ,
        K           ,
        L           ,
        SemiColon   ,
        Colon       ,
        RightBlacket,

        LeftShift   ,
        Z           ,
        X           ,
        C           ,
        V           ,
        B           ,
        N           ,
        M           ,
        Comma       ,
        Period      ,
        Slash       ,
        LowerYen    ,
        RightShift  ,
        LeftControl ,
        LeftWindows ,
        LeftAlt     ,
        Muhenkan    ,
        Space       ,
        Henkan      ,
        KanaHira    ,
        RightAlt    ,
        RightWindows,
        Application ,
        RightControl,


    }

    /// <summary>
    /// キーのオーバーレイ表示イメージ種別を表します。
    /// </summary>
    enum OverLayImageKind
    {

    }

}
