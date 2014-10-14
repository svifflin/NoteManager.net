using System.Windows;

namespace NoteManagerUI.Common
{
    public class Utilities
    {
        /// <summary>
        /// Retourner la fenetre parente
        /// </summary>
        public static Window GetWindowRef(string WindowName)
        {
            Window retVal = null;
            foreach (Window window in Application.Current.Windows)
            {
                // Le nom de la fenetre est définie dans le XAML
                if (window.Name.Trim().ToLower() == WindowName.Trim().ToLower())
                {
                    retVal = window;
                    break;
                }
            }

            return retVal;
        }

        public static void setCurrentMainWindow(System.Windows.Window window)
        {
            Application.Current.MainWindow = window;
        }
    }
}
