using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample01.ViewModels
{
    public class ViewUsrCtrlBViewModel : BindableBase, IDialogAware  // ポップアップによる画面遷移先ViewModelにはIDialogAwareを実装する
    {
        private string _aTextBoxText = "XXX";
        public string ATextBoxText
        {
            get { return _aTextBoxText; }
            set { SetProperty(ref _aTextBoxText, value); }
        }

        public DelegateCommand OkCmd { get; }


        public ViewUsrCtrlBViewModel()
        {
            this.OkCmd = new DelegateCommand(ExecOk);
        }


        private void ExecOk()
        {
            //====================================================================================================
            // ダイアログを閉じる。その際に遷移元に結果を返す。
            //====================================================================================================
            // 遷移元に返す結果パラメータを生成
            var dialogParams = new DialogParameters();
            dialogParams.Add(key: nameof(this.ATextBoxText), value: this.ATextBoxText);

            // IDialogAware実装のRequestCloseアクションを利用してダイアログを閉じる。
            // * RequestCloseがnullかもしれないので?付ける。
            this.RequestClose?.Invoke(new DialogResult(ButtonResult.OK, dialogParams));
        }

        // ポップアップによる画面遷移に用いる
        #region IDialogAware実装

        public string Title => "ViewUsrCtrlBView";
        //public string Title => throw new NotImplementedException();

        public event Action<IDialogResult> RequestClose;

        // 画面を閉じれるかどうかを指定。
        public bool CanCloseDialog()
        {
            //throw new NotImplementedException();
            return true;
        }

        // 画面が閉じる際に実行される。
        // 終了処理などで利用。
        public void OnDialogClosed()
        {
            //throw new NotImplementedException();
        }

        // 画面が開く際に実行される。
        // 初期化処理、画面遷移パラメータ受け取り処理などで利用。
        public void OnDialogOpened(IDialogParameters parameters)
        {
            //throw new NotImplementedException();
            // 画面遷移元からのパラメータを受け取る
            if (parameters == null) return;
            string aTextBoxTextKeyVal = parameters.GetValue<string>(key: nameof(this.ATextBoxText));
            this.ATextBoxText = aTextBoxTextKeyVal;
        }

        #endregion  // IDialogAware実装
    }
}
