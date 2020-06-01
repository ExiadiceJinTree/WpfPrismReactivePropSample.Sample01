using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Sample01.Views;
using System;

namespace Sample01.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IRegionManager _regionManager;
        private IDialogService _dialogService;

        private string _title = "Prism Sample01 App";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _systemDateString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        public string SystemDateString
        {
            get { return _systemDateString; }
            set { SetProperty(ref _systemDateString, value); }
        }

        public DelegateCommand UpdateSystemDateStrCmd { get; }

        public DelegateCommand ShowViewUsrCtrlACmd { get; }

        public DelegateCommand ShowViewUsrCtrlAWithParamCmd { get; }

        public DelegateCommand ShowViewUsrCtrlBCmd { get; }

        public DelegateCommand ShowViewUsrCtrlBWithParamCmd { get; }

        public DelegateCommand ShowViewUsrCtrlCCmd { get; }


        // 引数: IRegionManager, IDialogService ... 引数に指定すればPrismが自動的にコンストラクタに与えてくれる。
        // - IRegionManager: Region・ナビゲートによる画面遷移に用いる。
        // - IDialogService: ポップアップによる画面遷移に用いる。
        public MainWindowViewModel(IRegionManager regionManager, IDialogService dialogService)
        //public MainWindowViewModel()
        {
            _regionManager = regionManager;
            _dialogService = dialogService;

            this.UpdateSystemDateStrCmd = new DelegateCommand(UpdateSystemDateStr);
            this.ShowViewUsrCtrlACmd = new DelegateCommand(ShowViewUsrCtrlA);
            this.ShowViewUsrCtrlAWithParamCmd = new DelegateCommand(ShowViewUsrCtrlAWithParam);
            this.ShowViewUsrCtrlBCmd = new DelegateCommand(ShowViewUsrCtrlB);
            this.ShowViewUsrCtrlBWithParamCmd = new DelegateCommand(ShowViewUsrCtrlBWithParam);
            this.ShowViewUsrCtrlCCmd = new DelegateCommand(ShowViewUsrCtrlC);
        }


        private void UpdateSystemDateStr()
        {
            this.SystemDateString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        // Region・ナビゲートによる画面遷移
        private void ShowViewUsrCtrlA()
        {
            // XAMLの<ContentControl prism:RegionManager.RegionName="～" />部分に遷移先ビューが表示される。
            // * App.xaml.csのRegisterTypesメソッド内で、containerRegistry.RegisterForNavigationメソッドで遷移先ビューを登録しておかないと画面遷移・表示されない。
            _regionManager.RequestNavigate(
                regionName: "ContentRegion",  // XAMLで<ContentControl prism:RegionManager.RegionName="～" />と指定した部分のRegionName。
                source: nameof(ViewUsrCtrlA),  // 表示するView名
                navigationCallback: (navResult) => { }  // ナビゲーション完了後に実行されるナビゲーションコールバック(Action<NavigationResult>型)
            );
        }

        // Region・ナビゲートによる画面遷移(画面遷移パラメータあり)
        private void ShowViewUsrCtrlAWithParam()
        {
            // 画面遷移先ビューに渡すパラメータを生成
            var navParams = new NavigationParameters();
            navParams.Add(key: nameof(ViewUsrCtrlAViewModel.ParamALabel), value: this.SystemDateString);

            // XAMLの<ContentControl prism:RegionManager.RegionName="～" />部分に遷移先ビューが表示される。
            // * App.xaml.csのRegisterTypesメソッド内で、containerRegistry.RegisterForNavigationメソッドで遷移先ビューを登録しておかないと画面遷移・表示されない。
            _regionManager.RequestNavigate(
                regionName: "ContentRegion",  // XAMLで<ContentControl prism:RegionManager.RegionName="～" />と指定した部分のRegionName。
                target: nameof(ViewUsrCtrlA),  // 表示するView名
                navigationCallback: (navResult) => { },  // ナビゲーション完了後に実行されるナビゲーションコールバック(Action<NavigationResult>型)
                navigationParameters: navParams  // 画面遷移先ビューに渡すパラメータ
            );
        }

        // ポップアップによる画面遷移
        private void ShowViewUsrCtrlB()
        {
            // モーダルダイアログ表示
            // * App.xaml.csのRegisterTypesメソッド内で、containerRegistry.RegisterForNavigationメソッドで遷移先ビューを登録しておかないと実行時例外が発生する。
            _dialogService.ShowDialog(
                name: nameof(ViewUsrCtrlB),  // 表示するView名
                parameters: null,  // 画面遷移先ビューに渡すパラメータ
                callback: null  // 遷移先画面が閉じた時に実行すべきアクション(Action<IDialogResult>型)
            );
        }

        // ポップアップによる画面遷移(画面遷移パラメータあり、結果も受け取る)
        private void ShowViewUsrCtrlBWithParam()
        {
            // 画面遷移先ビューに渡すパラメータを生成
            var dialogParams = new DialogParameters();
            dialogParams.Add(key: nameof(ViewUsrCtrlBViewModel.ATextBoxText), value: this.SystemDateString);

            // モーダルダイアログ表示
            // * App.xaml.csのRegisterTypesメソッド内で、containerRegistry.RegisterForNavigationメソッドで遷移先ビューを登録しておかないと実行時例外が発生する。
            _dialogService.ShowDialog(
                name: nameof(ViewUsrCtrlB),  // 表示するView名
                parameters: dialogParams,  // 画面遷移先ビューに渡すパラメータ
                callback: OnViewUsrCtrlBClosed  // 遷移先画面が閉じた時に実行すべきアクション(Action<IDialogResult>型)
            );
        }

        // ViewUsrCtrlBビューを閉じた際に呼ばれるコールバックアクション。
        private void OnViewUsrCtrlBClosed(IDialogResult dialogResult)
        {
            // 遷移先ダイアログから返された結果パラメータを受け取る。
            if (dialogResult.Result == ButtonResult.OK)  // OKボタンが押されてダイアログが閉じた場合のみ
            {
                string aTextBoxTextKeyVal = dialogResult.Parameters.GetValue<string>(key: nameof(ViewUsrCtrlBViewModel.ATextBoxText));
                this.SystemDateString = aTextBoxTextKeyVal;
            }
        }

        private void ShowViewUsrCtrlC()
        {
            // XAMLの<ContentControl prism:RegionManager.RegionName="～" />部分に遷移先ビューが表示される。
            // * App.xaml.csのRegisterTypesメソッド内で、containerRegistry.RegisterForNavigationメソッドで遷移先ビューを登録しておかないと画面遷移・表示されない。
            _regionManager.RequestNavigate(
                regionName: "ContentRegion",  // XAMLで<ContentControl prism:RegionManager.RegionName="～" />と指定した部分のRegionName。
                source: nameof(ViewUsrCtrlC),  // 表示するView名
                navigationCallback: (navResult) => { }  // ナビゲーション完了後に実行されるナビゲーションコールバック(Action<NavigationResult>型)
            );
        }
    }
}
