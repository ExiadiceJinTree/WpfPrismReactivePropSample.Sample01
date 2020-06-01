using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Sample01.Services;
using Sample01.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Sample01.ViewModels
{
    public class ViewUsrCtrlAViewModel : BindableBase, INavigationAware  // Region・ナビゲートによる画面遷移先ViewModelにはINavigationAwareを実装する
    //public class ViewUsrCtrlAViewModel : BindableBase
    {
        private IDialogService _dialogService;

        private IMessageService _messageService;

        private string _paramALabel = string.Empty;
        public string ParamALabel
        {
            get { return _paramALabel; }
            set { SetProperty(ref _paramALabel, value); }
        }

        public DelegateCommand OkCmd { get; }
        
        public DelegateCommand Ok2Cmd { get; }


        // 単体テスト以外用コンストラクタ
        public ViewUsrCtrlAViewModel(IDialogService dialogService)
            : this(dialogService, new MessageService())
        {
        }

        // 単体テスト用コンストラクタ
        // * 本コンストラクタViewUsrCtrlAViewModel(dialogService, messageService)ではPrismがIMessageServiceの引数を理解できないため、
        //   別のコンストラクタViewUsrCtrlAViewModel(dialogService)を用意し、messageServiceはそちらで生成する。
        public ViewUsrCtrlAViewModel(IDialogService dialogService, IMessageService messageService)
        //public ViewUsrCtrlAViewModel(IDialogService dialogService)
        //public ViewUsrCtrlAViewModel()
        {
            _dialogService = dialogService;
            _messageService = messageService;

            this.OkCmd = new DelegateCommand(ExecOk);
            this.Ok2Cmd = new DelegateCommand(ExecOk2);
        }


        private void ExecOk()
        {
            //[メッセージボックス表示対応1]=======================================================================
            // ViewModelで普通のMessageBoxを使用できるが、単体テストで実行するとメッセージボックスが表示されてしまうので、そこで止まってしまう。
            // 表示されたメッセージボックスのボタンを押して閉じないと、単体テストが進まない。これでは単体テストを自動化できない。
            //====================================================================================================
            //MessageBox.Show("Saveします。");

            //[メッセージボックス表示対応2A&2B]===================================================================
            // MessageBox用に作成したビューをIDialogServiceで表示する。
            // テストメソッド側で、本ViewModelインスタンスを生成して本メソッドを実行することになるが、
            // IDialogServiceを使うので本ViewModelコンストラクタ引数にIDialogServiceを渡す必要がある。
            // テストメソッドで_dialogService.ShowDialogで止まらないために、MOQライブラリを利用して
            // IDialogServiceを実装したMockインスタンスを生成して、本ViewModelコンストラクタ引数に渡す。
            //====================================================================================================
            //[メッセージボックス表示対応2A]----------------------------------------------------------------------
            // IDialogServiceを実装したMockインスタンスでShowDialogの実装をしていなければ、
            // _dialogService.ShowDialogコードがスルーされるので、テストが止まることはない。
            // 但し、_dialogService.ShowDialog自体の結果をテストできない。
            //----------------------------------------------------------------------------------------------------
            //[メッセージボックス表示対応2B]----------------------------------------------------------------------
            // IDialogServiceを実装したMockインスタンスでShowDialogの実装をすれば、
            // _dialogService.ShowDialog自体の結果もテストでき、テストが止まることもない。
            //----------------------------------------------------------------------------------------------------
            var dialogParams = new DialogParameters();
            dialogParams.Add(nameof(ViewUsrCtrlBViewModel.ATextBoxText), "Saveします。");
            _dialogService.ShowDialog(nameof(ViewUsrCtrlB), dialogParams, null);
        }
        private void ExecOk2()
        {
            //[メッセージボックス表示対応3]=======================================================================
            // 普通のMessageBoxのラッパーメソッドを利用する。
            // メッセージ処理をインターフェース化することで、本番時はインターフェース実装クラス処理を、単体テスト時にはmock実装処理を、行うことができる。
            // * 本ViewModelコンストラクタの引数にメッセージサービスインターフェース実装を渡すようにし、内部でそれを利用する。
            // テストメソッド側で、本ViewModelインスタンス生成時にメッセージサービスインターフェース実装を渡す時には、
            // 自動テストがメッセージボックス表示処理で止まらないために、メッセージサービスインターフェース実装をMOQライブラリを利用して実装する。
            //====================================================================================================
            //[メッセージボックス表示対応3A]----------------------------------------------------------------------
            // メッセージサービスインターフェースのMockインスタンスで実装をしていなければ、
            // メッセージサービスインターフェースメソッドのコードがスルーされるので、テストが止まることはない。
            // 但し、メッセージメソッド自体の結果をテストできない。
            //----------------------------------------------------------------------------------------------------
            //[メッセージボックス表示対応3B]----------------------------------------------------------------------
            // メッセージサービスインターフェースのMockインスタンスで実装をすれば、
            // メッセージメソッド自体の結果もテストでき、テストが止まることもない。
            //----------------------------------------------------------------------------------------------------
            if (_messageService.ShowQuestionDialog("Saveしますか？") == MessageBoxResult.Yes)
            {
                _messageService.ShowInfoDialog("Saveしました。");
            }
        }

        // Region・ナビゲートによる画面遷移に用いる
        #region INavigationAware実装

        // 本ビューが画面遷移先として呼ばれた際に実行される。
        // 初期化処理、画面遷移パラメータ受け取り処理などで利用。
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            // 画面遷移元からのパラメータを受け取る
            string paramALabelKeyVal = navigationContext.Parameters.GetValue<string>(key: nameof(this.ParamALabel));
            this.ParamALabel = paramALabelKeyVal;
        }

        // 本ビューを表示したり非表示にしたりする中で、 本ビューのインスタンスを使い回すかどうかを決める。
        // 戻り値がtrue : 毎回同じインスタンスを使い回す場合。前の値が表示されたまま等が可能。
        // 戻り値がfalse: 毎回新しいインスタンスにする場合。
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            //return false;
            return true;
        }

        // 本ビューから別のビューに画面遷移する際に実行される。
        // 終了処理などで利用。
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        #endregion // INavigationAware実装
    }
}
