using System;
using System.ServiceModel;
using System.Windows;
using NoteManagerUI.Commands;
using NoteManagerUI.NoteManagerService;
using NoteManagerUI.View;
using NoteManagerUI.View.ViewInterfaces;

namespace NoteManagerUI.ViewModel
{
    public class AddEditNoteViewModel : BaseViewModel<IDialogView>, IDisposable
    {
        #region Messages

        private const string EDIT_MODE_TITLE = "Edition d'une note";
        private const string ADD_MODE_TITLE = "Ajout d'une note";
        private const string SAVE_ERROR_MESSAGE = "Saving operation failed! Internal error has occured. Exception message:\r\n";
        private const string SAVE_ERROR_TITLE = "Erreur";

        #endregion

        #region Fields

        private readonly NoteManagerServiceClient api;
        private readonly AddEditNoteViewModel context;
        private string title;
        private string content;
        private int? currentNoteId;
        private NoteData currentNote;
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

        public string Content
        {
            get
            {
                return this.content;
            }
            private set
            {
                this.content = value.Trim();
            }
        }

        public NoteData CurrentNote
        {
            get
            {
                return this.currentNote;
            }
            private set
            {
                this.currentNote = value;
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

        public AddEditNoteViewModel(int? currentNoteId) : base(new AddEditWindow())
        {
            this.context = this;
            this.api = new NoteManagerServiceClient();
            this.currentNoteId = currentNoteId;
            this.InitializeCurrentNoteAndTitle();
        }

        #endregion

        #region Commands

        public RelayCommand SaveCommand
        {
            get
            {
                this.command = new RelayCommand(this.Save);
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

        private void InitializeCurrentNoteAndTitle()
        {
            if (this.currentNoteId.HasValue)
            {
                //The view model is in Edit mode
                this.CurrentNote = this.api.GetNote(this.currentNoteId.Value);
                this.Title = EDIT_MODE_TITLE;
            }
            else
            {
                //The view model is in Add mode
                this.CurrentNote = new NoteData();
                this.CurrentNote.UserID = 1;
                this.Title = ADD_MODE_TITLE;
            }
        }

        private void Save()
        {
            try
            {
                if (this.currentNoteId.HasValue == false)
                {
                    //Add the new note to the context
                    this.api.AddNote(this.CurrentNote);
                }
                else
                {
                    //Update the currnt note to the context
                    this.api.UpdateNote(this.CurrentNote);
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
            this.RaisePropertyChanged("CurrentNote");
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
