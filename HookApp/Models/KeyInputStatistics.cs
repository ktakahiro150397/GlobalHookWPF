using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace HookApp.Models
{

    /// <summary>
    /// キー入力の統計情報を保持するクラスです。
    /// </summary>
    class KeyInputStatistics : INotifyPropertyChanged
    {

        #region "privateフィールド"
        private double _currentKPM;
        private double _maxKPM;
        private int _keyDownSum;
        private DateTime _startUpTime;
        private TimeSpan _elapsedTime;
        private System.Diagnostics.Stopwatch Stopwatch;
        private List<KeyDownInfo> _keyDownInfoList;

        private int kpmMilisec = 2000;
        #endregion

        #region "プロパティ"

        /// <summary>
        /// 現在のKPM値
        /// </summary>
        public double CurrentKPM
        {
            get
            {
                return _currentKPM;
            }
            set
            {
                this._currentKPM = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 最高KPM値
        /// </summary>
        public double MaxKPM
        {
            get
            {
                return _maxKPM;
            }
            set
            {
                this._maxKPM = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// キー合計打鍵数
        /// </summary>
        public int KeyDownSum
        {
            get
            {
                return _keyDownSum;
            }
            set
            {
                this._keyDownSum = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// アプリケーションが開始した時刻。
        /// </summary>
        public DateTime StartUpTime
        {
            get
            {
                return _startUpTime;
            }
        }

        /// <summary>
        /// アプリケーションが開始してから経過した時間。
        /// </summary>
        public string ElapsedTimeString
        {
            get
            {
                return _elapsedTime.ToString(@"\経\過\時\間\ \ \:\ \ hh\時\間mm\分ss\秒");
            }
        }

        public TimeSpan ElapsedTime
        {
            private get
            {
                return _elapsedTime;
            }
            set
            {
                //経過時間が更新されたとき、時間文字列のプロパティ変更を通知する
                _elapsedTime = value;
                this.OnPropertyChanged(nameof(ElapsedTimeString));
            }
        }
        #endregion

        #region "IPropertyChanged"
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string info = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion

        public KeyInputStatistics()
        {
            this._keyDownSum = 0;
            this._startUpTime = DateTime.Now;

            _currentKPM = 0D;
            _maxKPM = 0D;
            _keyDownSum = 0;
            _startUpTime = DateTime.Now;
            _keyDownInfoList = new List<KeyDownInfo>();

            //起動時間測定のためのDispatcherTimerの設定・起動
            Stopwatch = new Stopwatch();
            //Stopwatch.Tick += _timer_Tick;
            //Stopwatch.Interval = new TimeSpan(0, 0, 1);
            Stopwatch.Start();
        }

        /// <summary>
        /// DispatcherTimerのtickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick(object sender, EventArgs e)
        {
            //現在時刻 - 起動時刻で時間差を算出
            TimeSpan diff = DateTime.Now - this.StartUpTime;
            this.ElapsedTime = diff;

            //KPMの算出
            //過去500msのデータを除外する
            _keyDownInfoList = _keyDownInfoList.Where(item => item.inputTime > ElapsedTime - (new TimeSpan(0, 0, 0, 0, kpmMilisec))).ToList();
            //KPM値を算出
            this.CurrentKPM = (double)_keyDownInfoList.Count / (double)kpmMilisec * 1000 * 60;
            //最高KPMを更新する
            if (this.MaxKPM < this.CurrentKPM)
            {
                this.MaxKPM = this.CurrentKPM;
            }
        }

        public void KeyDownCount(object sender, KeyboardUtil.HookKeyEventArgs e)
        {
            //総打鍵数を加算
            this.KeyDownSum++;

            //キーダウン時間の格納
            _keyDownInfoList.Add(new KeyDownInfo(this.ElapsedTime));

            
        }

        private class KeyDownInfo
        {
            /// <summary>
            /// このキーがプッシュされた、起動時間からの差。
            /// </summary>
            internal TimeSpan inputTime { get; }

            public KeyDownInfo(TimeSpan time)
            {
                inputTime = time;
            }
        }

    }
}
