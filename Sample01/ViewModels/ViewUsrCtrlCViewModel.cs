using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Sample01.Models;
using Sample01.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Sample01.ViewModels
{
    public class ViewUsrCtrlCViewModel : BindableBase, IConfirmNavigationRequest
    //public class ViewUsrCtrlCViewModel : BindableBase
    {
        private IMessageService _messageService;

        private ObservableCollection<string> _myListBox = new ObservableCollection<string>();
        public ObservableCollection<string> MyListBox
        {
            get { return _myListBox; }
            set { SetProperty(ref _myListBox, value); }
        }

        private ObservableCollection<AreaData> _areas = new ObservableCollection<AreaData>();
        public ObservableCollection<AreaData> Areas
        {
            get { return _areas; }
            set { SetProperty(ref _areas, value); }
        }

        private string _selectedMyAListBoxItem;
        public string SelectedMyAListBoxItem
        {
            get { return _selectedMyAListBoxItem; }
            set { SetProperty(ref _selectedMyAListBoxItem, value); }
        }

        private string _selectedMyBListBoxItem;
        public string SelectedMyBListBoxItem
        {
            get { return _selectedMyBListBoxItem; }
            set { SetProperty(ref _selectedMyBListBoxItem, value); }
        }

        private string _selectedMyCListBoxItem;
        public string SelectedMyCListBoxItem
        {
            get { return _selectedMyCListBoxItem; }
            set { SetProperty(ref _selectedMyCListBoxItem, value); }
        }

        private AreaData _selectedAArea;
        public AreaData SelectedAArea
        {
            get { return _selectedAArea; }
            set { SetProperty(ref _selectedAArea, value); }
        }

        private string _selectedAAreaInfo;
        public string SelectedAAreaInfo
        {
            get { return _selectedAAreaInfo; }
            set { SetProperty(ref _selectedAAreaInfo, value); }
        }

        private AreaData _selectedBArea;
        public AreaData SelectedBArea
        {
            get { return _selectedBArea; }
            set { SetProperty(ref _selectedBArea, value); }
        }

        // prism:InvokeCommandActionのCommandプロパティでバインドした場合は、DelegateCommand<T>のパラメータにはイベントパラメータが渡ってきて利用できる。
        public DelegateCommand<SelectionChangedEventArgs> AAreasComboBox_SelectionChangedCmd { get; }

        public DelegateCommand<object> TestACmd { get; }
        public DelegateCommand<RoutedEventArgs> TestBCmd { get; }


        // 単体テスト以外用コンストラクタ
        public ViewUsrCtrlCViewModel()
            : this(new MessageService())
        {
        }

        // 単体テスト用コンストラクタ
        public ViewUsrCtrlCViewModel(IMessageService messageService)
        //public ViewUsrCtrlCViewModel()
        {
            _messageService = messageService;

            this.AAreasComboBox_SelectionChangedCmd = new DelegateCommand<SelectionChangedEventArgs>(AAreasComboBox_SelectionChanged);
            this.TestACmd = new DelegateCommand<object>(TestA);
            this.TestBCmd = new DelegateCommand<RoutedEventArgs>(TestB);

            this.MyListBox.Add("abcdefg");
            this.MyListBox.Add("12345");
            this.MyListBox.Add("QWERT");
            this.MyListBox.Add("XYZ");

            //this.SelectedMyAListBoxItem = this.MyListBox[0];  // 既定値を設定
            this.SelectedMyBListBoxItem = this.MyListBox[1];  // 既定値を設定
            this.SelectedMyCListBoxItem = this.MyListBox[2];  // 既定値を設定


            this.Areas.Add(new AreaData(1, "沖縄"));
            this.Areas.Add(new AreaData(2, "北海道"));
            this.Areas.Add(new AreaData(3, "愛知"));

            //this.SelectedAArea = this.Areas[0];  // 既定値を設定
            this.SelectedBArea = this.Areas[1];  // 既定値を設定
        }


        // prism:InvokeCommandActionのCommandプロパティでバインドした場合は、DelegateCommand<T>のパラメータにはイベントパラメータが渡ってきて利用できる。
        private void AAreasComboBox_SelectionChanged(SelectionChangedEventArgs e)
        {
            ComboBox sender = (ComboBox)e.Source;  // イベント発火元コントロールはEventArgsのSourceプロパティで取得可能。
            AreaData selectedAArea = (AreaData)e.AddedItems[0];

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("・EventSender's Name from EventArgs: " + sender.Name);
            sb.AppendLine("・SelectedArea's Value&Name from EventArgs: " + selectedAArea.Value + "," + selectedAArea.Name);
            sb.AppendLine("・SelectedArea's Value&Name from BindingProp: " + this.SelectedAArea.Value + "," + this.SelectedAArea.Name);
            this.SelectedAAreaInfo = sb.ToString();
        }

        // Buttonでも、普通にCommandプロパティでバインドした場合は、DelegateCommand<T> のパラメータはnullになってしまいイベントパラメータは利用できない。
        private void TestA(object param)
        {
        }

        // Buttonでも、prism:InvokeCommandActionのCommandプロパティでバインドした場合は、DelegateCommand<T>のパラメータにはイベントパラメータが渡ってきて利用できる。
        private void TestB(RoutedEventArgs e)
        {
            Button sender = (Button)e.Source;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("・EventSender's Name from EventArgs: " + sender.Name);
            sb.AppendLine("・EventSender's Content from EventArgs: " + sender.Content);
            _messageService.ShowInfoDialog(sb.ToString());
        }

        // Region・ナビゲートによる画面遷移に用いる
        #region IConfirmNavigationRequest実装

        // IConfirmNavigationRequestインターフェース独自
        // 本ビューから画面遷移する際に実行され、画面遷移するかどうかを決定する。
        // * 何も実装しなければ画面遷移しないままとなる。continuationCallback引数アクションのbool引数で画面遷移するか指定する。
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            // 画面遷移先が同じ画面ならスルーする。
            if (navigationContext.Uri.OriginalString == nameof(Sample01.Views.ViewUsrCtrlC))
            {
                return;
            }

            // 画面遷移前に本当に画面遷移するか確認するメッセージを表示する。
            // 例えば、本画面で編集し未保存のデータがある場合に、警告メッセージを出したい場合などに使える。
            if(_messageService.ShowQuestionDialog("未保存のデータがあります。別画面に遷移すると未保存のデータは失われますがよろしいですか？")
                == System.Windows.MessageBoxResult.Yes)
            {
                continuationCallback(true);
            }
        }

        // INavigationAwareインターフェス同名メソッドと同じ。
        // 本ビューを表示したり非表示にしたりする中で、 本ビューのインスタンスを使い回すかどうかを決める。
        // 戻り値がtrue : 毎回同じインスタンスを使い回す場合。前の値が表示されたまま等が可能。
        // 戻り値がfalse: 毎回新しいインスタンスにする場合。
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        // INavigationAwareインターフェス同名メソッドと同じ。
        // * ConfirmNavigationRequestメソッドにより画面遷移することに決定した後に呼ばれ、画面遷移しないことに決定した場合は呼ばれない。
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        // INavigationAwareインターフェス同名メソッドと同じ。
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        #endregion // IConfirmNavigationRequest実装
    }
}
