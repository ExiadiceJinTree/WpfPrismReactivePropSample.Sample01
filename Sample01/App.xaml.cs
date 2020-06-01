using Sample01.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using Sample01.ViewModels;

namespace Sample01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // アプリケーションのどこからでも(子画面からでも)メイン画面にアクセスし操作するために、
            // メイン画面はアプリケーションで1つのみと設定しておく(MainWindowViewModelをシングルトンインスタンスとして登録する)。
            // こうすると、子画面ViewModelのコンストラクタインジェクションによりMainWindowViewModelにアクセスできる。
            containerRegistry.RegisterSingleton<MainWindowViewModel>();

            // Region・ナビゲートによる画面遷移で、ナビゲーション対象とするビューを登録しておかないと画面遷移・表示されない。
            containerRegistry.RegisterForNavigation<ViewUsrCtrlA>();
            containerRegistry.RegisterForNavigation<ViewUsrCtrlC>();

            // ポップアップによる画面遷移で、IDialogServiceでダイアログとして用いるビューを登録しておかないと画面遷移・表示されない(実行時例外になる)。
            // また、ポップアップによる画面遷移先ViewModelにはIDialogAwareを実装しておかないと実行時例外になる。
            containerRegistry.RegisterDialog<ViewUsrCtrlB>();
        }
    }
}
