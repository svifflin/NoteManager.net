using System.Windows;
using NoteManager.WPFClient;

namespace NoteManagerUI.View
{
    public class BaseDialogWindow : Window
    {
        public BaseDialogWindow()
        {
            this.Owner = Application.Current.MainWindow;
            this.ShowInTaskbar = false;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
        }
    }
}
