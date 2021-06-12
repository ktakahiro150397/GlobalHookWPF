using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using HookApp.Models;
using HookApp.Lib;
using System.Windows.Input;
using System.Windows.Controls;
using HookApp.ViewModels.Commands;
using HookApp.ViewModels.Consts;
using HookApp.ViewModels.Interfaces;

namespace HookApp.ViewModels
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// プロパティ変更をUI側へ通知するイベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティ変更の通知メソッド。
        /// </summary>
        /// <param name="info"></param>
        protected void OnPropertyChanged(string info)
        {
            //イベントはnullを許容
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        /// <summary>
        /// <see cref="ICommand"/>で使用するアクションクラスのリストを用意します。
        /// </summary>
        private Dictionary<CommandActionType, Action> CommandClasses { get; set; } 

        public void AddCommandAction(CommandActionType actionType, Action commandClass)
        {
            CommandClasses.Add(actionType, commandClass);
        }

        /// <summary>
        /// キーに紐づくアクションクラスのインスタンスを返します。
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public Action GetCommandAction(CommandActionType actionType)
        {
            try
            {
                return CommandClasses[actionType];
            }catch(KeyNotFoundException keyEx)
            {
                //キーが存在しない
                throw new ApplicationException($"指定されたキー「{actionType}」に紐づくActionが存在しません。設定してから使用する必要があります。", keyEx);
            }
        }
    }
}
