using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prism.Services.Dialogs;
using Sample01.Services;
using Sample01.ViewModels;
using Sample01.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sample01.ViewModels.Tests
{
    [TestClass()]
    public class ViewUsrCtrlAViewModelTests
    {
        [TestMethod]
        public void OkCmdTest()
        {
            //[メッセージボックス表示対応1]=======================================================================
            // ViewModelで普通のMessageBoxを使用できるが、単体テストで実行するとメッセージボックスが表示されてしまうので、そこで止まってしまう。
            // 表示されたメッセージボックスのボタンを押して閉じないと、単体テストが進まない。これでは単体テストを自動化できない。
            //====================================================================================================
            //var vm = new ViewUsrCtrlAViewModel();
            //vm.OkCmd.Execute();

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
            //var dialogSvcMoq = new Mock<IDialogService>();
            //var vm = new ViewUsrCtrlAViewModel(dialogSvcMoq.Object);
            //vm.OkCmd.Execute();

            //[メッセージボックス表示対応2B]----------------------------------------------------------------------
            // IDialogServiceを実装したMockインスタンスでShowDialogの実装をすれば、
            // _dialogService.ShowDialog自体の結果もテストでき、テストが止まることもない。
            //----------------------------------------------------------------------------------------------------
            var dialogSvcMoq = new Mock<IDialogService>();
            dialogSvcMoq.Setup(x => x.ShowDialog(
                    It.IsAny<string>(),  // It.IsAny<T>は、T型引数なら値は何でもいいということ
                    It.IsAny<IDialogParameters>(),
                    It.IsAny<Action<IDialogResult>>()
                )).Callback<string, IDialogParameters, Action<IDialogResult>>((viewName, dialogParams, callback) =>
                {
                    Assert.AreEqual(nameof(ViewUsrCtrlB), viewName);
                    Assert.AreEqual("Saveします。", dialogParams.GetValue<string>(nameof(ViewUsrCtrlBViewModel.ATextBoxText)));
                    Assert.AreEqual(null, callback);
                });

            var vm = new ViewUsrCtrlAViewModel(dialogSvcMoq.Object);
            vm.OkCmd.Execute();
        }
        [TestMethod]
        public void Ok2CmdTest()
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
            //var dialogSvcMoq = new Mock<IDialogService>();
            //var messageSvcMoq = new Mock<IMessageService>();
            //var vm = new ViewUsrCtrlAViewModel(dialogSvcMoq.Object, messageSvcMoq.Object);
            //vm.Ok2Cmd.Execute();

            //[メッセージボックス表示対応3B]----------------------------------------------------------------------
            // メッセージサービスインターフェースのMockインスタンスで実装をすれば、
            // メッセージメソッド自体の結果もテストでき、テストが止まることもない。
            //----------------------------------------------------------------------------------------------------
            var dialogSvcMoq = new Mock<IDialogService>();
            var messageSvcMoq = new Mock<IMessageService>();
            messageSvcMoq.Setup(x => x.ShowQuestionDialog(It.IsAny<string>()))
                .Callback<string>((messageBoxText) =>
                {
                    Debug.WriteLine(messageBoxText);
                    Assert.AreEqual("Saveしますか？", messageBoxText);
                })
                .Returns(MessageBoxResult.Yes);
            string actualInfoDialogMessage = string.Empty;
            messageSvcMoq.Setup(x => x.ShowInfoDialog(It.IsAny<string>()))
                .Callback<string>((messageBoxText) =>
                {
                    Debug.WriteLine(messageBoxText);
                    actualInfoDialogMessage = messageBoxText;
                })
                .Returns(MessageBoxResult.OK);

            var vm = new ViewUsrCtrlAViewModel(dialogSvcMoq.Object, messageSvcMoq.Object);
            vm.Ok2Cmd.Execute();

            messageSvcMoq.VerifyAll();  // MOQ実装した処理を通ってないテストがあるかどうか検証する。
            Assert.AreEqual("Saveしました。", actualInfoDialogMessage);
        }


        [TestMethod()]
        public void ViewUsrCtrlAViewModelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void OnNavigatedToTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsNavigationTargetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void OnNavigatedFromTest()
        {
            Assert.Fail();
        }
    }
}