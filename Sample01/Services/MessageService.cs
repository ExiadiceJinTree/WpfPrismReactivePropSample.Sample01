using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sample01.Services
{
    /// <summary>
    /// System.Windows.MessageBoxクラス等のメッセージ処理のラップクラス。
    /// </summary>
    public sealed class MessageService : IMessageService
    {
        public MessageBoxResult ShowDialog(string messageBoxText, MessageBoxButton button, MessageBoxImage icon)
        {
            return MessageBox.Show(messageBoxText, caption: "Message", button: button, icon: icon);
        }

        public MessageBoxResult ShowInfoDialog(string messageBoxText)
        {
            return MessageBox.Show(messageBoxText, caption: "Info.", button: MessageBoxButton.OK, icon: MessageBoxImage.Information);
        }

        public MessageBoxResult ShowQuestionDialog(string messageBoxText)
        {
            return MessageBox.Show(messageBoxText, caption: "Question", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Information);
        }

        public MessageBoxResult ShowQuestionDialog(string messageBoxText, MessageBoxButton button)
        {
            return MessageBox.Show(messageBoxText, caption: "Question", button: button, icon: MessageBoxImage.Information);
        }

        public MessageBoxResult ShowWarningDialog(string messageBoxText)
        {
            return MessageBox.Show(messageBoxText, caption: "Warning", button: MessageBoxButton.OK, icon: MessageBoxImage.Information);
        }

        public MessageBoxResult ShowErrorDialog(string messageBoxText)
        {
            return MessageBox.Show(messageBoxText, caption: "Error", button: MessageBoxButton.OK, icon: MessageBoxImage.Information);
        }

        public MessageBoxResult ShowFatalDialog(string messageBoxText)
        {
            return MessageBox.Show(messageBoxText, caption: "Fatal Error", button: MessageBoxButton.OK, icon: MessageBoxImage.Information);
        }
    }
}
