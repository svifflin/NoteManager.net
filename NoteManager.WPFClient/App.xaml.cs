using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NoteManagerUI.ViewModel;

namespace NoteManager.WPFClient
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private LoginViewModel loginViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.loginViewModel = new LoginViewModel();
            this.loginViewModel.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (this.loginViewModel != null)
            {
                this.loginViewModel.Dispose();
            }
        }  
    }
}
