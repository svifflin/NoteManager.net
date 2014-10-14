using System;
using System.ServiceModel;
using System.Windows;
using NoteManagerUI.Commands;
using NoteManagerUI.Common;
using NoteManagerUI.NoteManagerService;
using NoteManagerUI.View;
using NoteManagerUI.View.ViewInterfaces;

namespace NoteManagerUI.ViewModel
{
    public class AddEditUserViewModel : BaseViewModel<IDialogView>, IDisposable
    {
        #region Messages

        private const string EDIT_MODE_TITLE = "Edition du compte";
        private const string ADD_MODE_TITLE = "Ajout d'un compte";
        private const string SAVE_ERROR_MESSAGE = "Saving operation failed! Internal error has occured. Exception message:\r\n";
        private const string ADD_SUCCESS_MESSAGE = "Inscription réussi!\r\n\nVous pouvez dès à présent vous connecter";
        private const string EDIT_SUCCESS_MESSAGE = "Modification(s) effectuée(s)";
        private const string SAVE_ERROR_TITLE = "Erreur";
        private const string SAVE_SUCCESS_TITLE = "Confirmation";

        #endregion

        #region Fields

        private readonly NoteManagerServiceClient api;
        private readonly AddEditUserViewModel context;
        private string title;
        private int? currentUserId;
        private UserData currentUser;
        private RelayCommand command;
        private bool disposed;

        #endregion

        #region Properties

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

        public UserData CurrentUser
        {
            get
            {
                return this.currentUser;
            }
            private set
            {
                this.currentUser = value;
            }
        }

        public bool? OperationIsSuccessful
        {
            get
            {
                return this.View.DialogResult;
            }
            private set
            {
                this.View.DialogResult = value;
            }
        }

        #endregion

        #region Constructors

        public AddEditUserViewModel(int? currentUserId) : base(new AddEditUserWindow())
        {
            this.context = this;
            this.api = new NoteManagerServiceClient();
            this.currentUser = new UserData();
            this.currentUserId = currentUserId;
            this.CurrentUser.Email = string.Empty;
            this.CurrentUser.Password = string.Empty;
            this.InitializeCurrentUserAndTitle();
        }

        #endregion

        #region Commands

        public RelayCommand SaveCommand
        {
            get
            {
                this.command = new RelayCommand(this.Save, this.CanSave);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                this.command = new RelayCommand(this.Cancel);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        #endregion

        #region Methods

        public bool? ShowDialog()
        {
            return this.View.ShowDialog();
        }

        private void InitializeCurrentUserAndTitle()
        {
            if (this.currentUserId.HasValue)
            {
                //The view model is in Edit mode
                this.CurrentUser = this.api.GetUser(this.currentUserId.Value);
                this.Title = EDIT_MODE_TITLE;
            }
            else
            {
                //The view model is in Add mode
                //this.CurrentUser = new UserData();
                this.Title = ADD_MODE_TITLE;
            }
        }

        private bool CanSave()
        {
            return (Helpers.IsEmailValid(this.CurrentUser.Email.Trim()) && this.CurrentUser.Password.Trim().Length > 0);
        }

        private void Save()
        {
            try
            {
                if (this.currentUserId.HasValue == false)
                {
                    //Add the new user to the context
                    this.api.AddUser(this.CurrentUser);
                    MessageBox.Show(ADD_SUCCESS_MESSAGE, SAVE_SUCCESS_TITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //Update the current user to the context
                    this.api.UpdateUser(this.CurrentUser);
                    MessageBox.Show(EDIT_SUCCESS_MESSAGE, SAVE_SUCCESS_TITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                this.OperationIsSuccessful = true;
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

        private void Cancel()
        {
            this.View.Close();
        }

        private void HandleException(string exceptionMessage)
        {
            MessageBox.Show(SAVE_ERROR_MESSAGE + exceptionMessage, SAVE_ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
            this.OperationIsSuccessful = null;
            //Cause binding refresh
            this.RaisePropertyChanged("CurrentUser");
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
