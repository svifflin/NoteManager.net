using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Windows;
using NoteManagerUI.Commands;
using NoteManagerUI.Common;
using NoteManagerUI.NoteManagerService;
using NoteManagerUI.View;
using NoteManagerUI.View.ViewInterfaces;

namespace NoteManagerUI.ViewModel
{
    /// <summary>
    /// This is the VM for the Login Window and Registration Window.
    /// This class contains properties that the linked Windows can data bind to.
    /// </summary>
    public class LoginViewModel : BaseViewModel<IMainView>, IDisposable
    {
        #region Messages

        private const string SAVE_ERROR_MESSAGE = "Saving operation failed! Internal error has occured. Exception message:\r\n";
        private const string ERROR_TITLE = "Erreur";
        private const string ERROR_CONNECT_MESSAGE = "Internal error has occured. Exception message:\r\n";
        private const string LOGIN_SUCCESS = "Connexion réussi";
        private const string LOGIN_ERROR = "Email ou mot de passe non reconnu";

        #endregion

        #region Fields

        private System.Windows.Window window;
        private NotesViewModel mainViewModel;
        private NoteManagerServiceClient api;
        private string status;
        private string title;
        private string email;
        private string password;
        private bool isConnected;
        private int currentUserId;
        private UserData currentUser;
        private RelayCommand command;
        private bool disposed;

        #endregion

        #region Properties

        public string Status
        {
            get
            {
                return this.status;
            }
            private set
            {
                this.status = value.Trim();
                this.RaisePropertyChanged("Status");
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            private set
            {
                this.title = value.Trim();
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            private set
            {
                this.email = value.Trim();
                this.RaisePropertyChanged("Email");
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            private set
            {
                this.password = value.Trim();
                this.RaisePropertyChanged("Password");
            }
        }

        public UserData CurrentUser
        {
            get
            {
                return this.currentUser;
            }
            set
            {
                this.currentUser = value;
            }
        }

        #endregion

        #region Constructors

        public LoginViewModel() : base(new Login())
        {
            this.window = Utilities.GetWindowRef("LoginView");
            this.api = new NoteManagerServiceClient();
            this.Status = string.Empty;
            this.email = string.Empty;
            this.password = string.Empty;
            this.isConnected = false;
            Utilities.setCurrentMainWindow(this.window);
        }

        #endregion

        #region Commands

        public RelayCommand LoginCommand
        {
            get
            {
                this.command = new RelayCommand(this.Connect, this.CanLogin);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public RelayCommand AddUserCommand
        {
            get
            {
                this.command = new RelayCommand(this.AddUser);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        #endregion

        #region Methods

        public void Show()
        {
            this.View.Show();
        }

        private void AddUser()
        {
            bool? operationIsSuccessful;
            using (AddEditUserViewModel addModel = new AddEditUserViewModel(null))
            {
                operationIsSuccessful = addModel.ShowDialog();
            }
            if (operationIsSuccessful == true)
            {
                //
            }
        }

        private void Connect()
        {
            try
            {
                this.isConnected = this.api.Login(this.Email, this.Password);
                if (this.IsConnected())
                {
                    this.Status = LOGIN_SUCCESS;
                    this.currentUser = this.api.GetUserByEmail(this.Email);
                    this.currentUserId = this.CurrentUser.userId;
                    this.mainViewModel = new NotesViewModel(this.currentUserId);

                    //Ferme la page d'identification
                    this.window.Close();

                    this.mainViewModel.Show();
                }
                else
                {
                    this.Status = LOGIN_ERROR;
                }
            }
            catch (FaultException<DatabaseFault> ex)
            {
                MessageBox.Show(ex.Detail.DbReason + "\r\n\n" + ex.Detail.DbMessage, ex.Detail.DbOperation, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (FaultException<SystemFault> ex)
            {
                MessageBox.Show(ex.Detail.SystemReason + "\r\n\n" + ex.Detail.SystemMessage, ex.Detail.SystemOperation, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception ex)
            {
                this.HandleException(ex.Message);
                return;
            }
        }

        private void HandleException(string exceptionMessage)
        {
            MessageBox.Show(ERROR_CONNECT_MESSAGE + exceptionMessage, ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// la méthode qui va servir de test pour la connection de l'utilisateur
        /// </summary>
        private bool CanLogin()
        {
            return (Helpers.IsEmailValid(this.Email) && this.Password.Length > 0);
        }

        private bool IsConnected()
        {
            return this.isConnected;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed ressources
                }
            }
            //dispose unmanaged ressources
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
