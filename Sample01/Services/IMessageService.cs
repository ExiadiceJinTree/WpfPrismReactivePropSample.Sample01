using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sample01.Services
{
    /// <summary>
    /// メッセージ処理をインターフェース化することで、本番時はインターフェース実装クラス処理を、単体テスト時にはmock実装処理を、行うことができる。
    /// * 本ViewModelコンストラクタの引数にメッセージサービスインターフェース実装を渡すようにし、内部でそれを利用する。
    /// テストメソッド側で、本ViewModelインスタンス生成時にメッセージサービスインターフェース実装を渡す時には、
    /// 自動テストがメッセージボックス表示処理で止まらないために、メッセージサービスインターフェース実装をMOQライブラリを利用して実装する。
    /// </summary>
    public interface IMessageService
    {
        MessageBoxResult ShowDialog(string messageBoxText, MessageBoxButton button, MessageBoxImage icon);

        MessageBoxResult ShowInfoDialog(string messageBoxText);

        MessageBoxResult ShowQuestionDialog(string messageBoxText);

        MessageBoxResult ShowQuestionDialog(string messageBoxText, MessageBoxButton button);

        MessageBoxResult ShowWarningDialog(string messageBoxText);

        MessageBoxResult ShowErrorDialog(string messageBoxText);

        MessageBoxResult ShowFatalDialog(string messageBoxText);
    }
}
