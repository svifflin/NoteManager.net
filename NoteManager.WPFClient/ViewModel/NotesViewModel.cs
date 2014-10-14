using System;
using System.Collections.Generic;
using NoteManagerUI.View.ViewInterfaces;
using NoteManagerUI.Commands;
using NoteManagerUI.NoteManagerService;
using NoteManagerUI.View;
using System.Windows;
using System.ServiceModel;
using NoteManagerUI.Common;

namespace NoteManagerUI.ViewModel
{
    public class NotesViewModel : BaseViewModel<IMainView>, IDisposable
    {
        #region Messages

        private const string STATUS_MESSAGE = "{0} notes récupérées de la base de données.";
        private const string CONFIRM_DELETE_MESSAGE = "Vous allez supprimer la note \"{0}\".\r\n\nEtes-vous sûr ?";
        private const string CONFIRM_DELETE_TITLE = "Confirmez la suppression.";
        private const string DELETE_ERROR_RELATED_ENTRY_MESSAGE = "Impossible de supprimer cette note parce que d'autres entrées liées existent dans la base de données. Exception message:\r\n";
        private const string DELETE_ERROR_MESSAGE = "Erreur interne. Exception message:\r\n";
        private const string DELETE_ERROR_TITLE = "Erreur de suppression!";
        private const string CONFIRM_LOGOUT_MESSAGE = "Êtes-vous sûr de vouloir vous déconnecter ?";
        private const string CONFIRM_LOGOUT_TITLE = "Déconnexion";
        private const string ERROR_TITLE = "Erreur";

        #endregion

        #region Fields

        private readonly NoteManagerServiceClient api;
        private LoginViewModel loginViewModel;
        private System.Windows.Window window;
        private string notesMakerFilter;
        private bool isFilteringActive;
        private string status;
        private int currentUserID;
        private List<NoteData> notesToDisplay;
        private NoteData selectedNote;
        private RelayCommand command;
        private bool disposed;

        #endregion

        #region Properties

        public string NotesMakerFilter
        {
            get
            {
                return this.notesMakerFilter;
            }
            set
            {
                this.notesMakerFilter = value.Trim();
                this.RaisePropertyChanged("NotesMakerFilter");
            }
        }

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

        public List<NoteData> NotesToDisplay
        {
            get
            {
                return this.notesToDisplay;
            }
            private set
            {
                this.notesToDisplay = value;
                this.RaisePropertyChanged("NotesToDisplay");
            }
        }

        public NoteData SelectedNote
        {
            get
            {
                return this.selectedNote;
            }
            set
            {
                this.selectedNote = value;
                this.RaisePropertyChanged("SelectedNote");
            }
        }

        #endregion

        #region Constructors

        public NotesViewModel(int userID) : base(new MainWindow())
        {
            this.window = Utilities.GetWindowRef("NotesWindow");
            this.api = new NoteManagerServiceClient();
            this.NotesMakerFilter = string.Empty;
            this.isFilteringActive = false;
            this.Status = string.Empty;
            this.currentUserID = userID;
            Utilities.setCurrentMainWindow(this.window);
            this.RetrieveNotesToDisplay();
        }

        #endregion

        #region Commands

        public RelayCommand FilterNotesCommand
        {
            get
            {
                this.command = new RelayCommand(this.FilterNotes);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public RelayCommand ResetFilterCommand
        {
            get
            {
                this.command = new RelayCommand(this.ResetFilter, this.CanResetFilter);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                this.command = new RelayCommand(this.Refresh);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public RelayCommand DeleteNoteCommand
        {
            get
            {
                this.command = new RelayCommand(this.DeleteNote, this.IsNoteSelected);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public RelayCommand AddNoteCommand
        {
            get
            {
                this.command = new RelayCommand(this.AddNote);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public RelayCommand EditNoteCommand
        {
            get
            {
                this.command = new RelayCommand(this.EditNote, this.IsNoteSelected);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public RelayCommand EditAccountCommand
        {
            get
            {
                this.command = new RelayCommand(this.EditAccount);
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public RelayCommand LogoutCommand
        {
            get
            {
                this.command = new RelayCommand(this.Logout);
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

        private void RetrieveNotesToDisplay()
        {
            int? selectedNoteId = null;
            //save the Id of the currently selected note
            if (this.SelectedNote != null)
            {
                selectedNoteId = this.SelectedNote.noteID;
            }
            try
            {
                if (string.IsNullOrEmpty(this.NotesMakerFilter))
                {
                    //retrieve all notes
                    this.NotesToDisplay = this.api.NotesListByUserID(this.currentUserID);
                }
                else
                {
                    //retrieve notes which comply to the filter
                    this.NotesToDisplay = this.api.NotesListByFilter(this.NotesMakerFilter, this.currentUserID);
                }
                //restore the selected note using its saved Id
                if (selectedNoteId.HasValue)
                {
                    this.SelectedNote = this.NotesToDisplay.Find(
                        delegate(NoteData dt)
                        {
                            return dt.noteID == selectedNoteId;
                        }
                    );
                }
                this.Status = string.Format(STATUS_MESSAGE, this.NotesToDisplay.Count);
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
            MessageBox.Show(exceptionMessage, ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DeleteNote()
        {
            NoteData noteToDelete = this.SelectedNote;
            MessageBoxResult deleteConfirmation = MessageBox.
                Show(string.Format(CONFIRM_DELETE_MESSAGE, noteToDelete.Title),
                                   CONFIRM_DELETE_TITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (deleteConfirmation != MessageBoxResult.Yes)
            {
                return;
            }
            bool saveChangesIsSuccessful = false;
            try
            {
                saveChangesIsSuccessful = this.api.RemoveNote(noteToDelete.noteID);
            }
            catch (FaultException<DatabaseFault> ex)
            {
                MessageBox.Show(ex.Detail.DbReason + "\r\n\n" + ex.Detail.DbMessage, ex.Detail.DbOperation, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FaultException<SystemFault> ex)
            {
                MessageBox.Show(ex.Detail.SystemReason + "\r\n\n" + ex.Detail.SystemMessage, ex.Detail.SystemOperation, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(DELETE_ERROR_MESSAGE + ex.Message, DELETE_ERROR_TITLE,
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (saveChangesIsSuccessful)
            {
                this.RetrieveNotesToDisplay();
            }
        }

        private void AddNote()
        {
            bool? operationIsSuccessful;
            using (AddEditNoteViewModel addModel = new AddEditNoteViewModel(null))
            {
                operationIsSuccessful = addModel.ShowDialog();
            }
            if (operationIsSuccessful == true)
            {
                this.RetrieveNotesToDisplay();
            }
        }

        private void EditNote()
        {
            NoteData noteToEdit = this.SelectedNote;
            bool? operationIsSuccessful;
            using (AddEditNoteViewModel editModel = new AddEditNoteViewModel(noteToEdit.noteID))
            {
                operationIsSuccessful = editModel.ShowDialog();
            }
            if (operationIsSuccessful == true)
            {
                this.RetrieveNotesToDisplay();
            }
        }

        private void EditAccount()
        {
            bool? operationIsSuccessful;
            using (AddEditUserViewModel addModel = new AddEditUserViewModel(currentUserID))
            {
                operationIsSuccessful = addModel.ShowDialog();
            }
            if (operationIsSuccessful == true)
            {
                this.RetrieveNotesToDisplay();
            }
        }

        private bool IsNoteSelected()
        {
            return this.SelectedNote != null;
        }

        private void FilterNotes()
        {
            this.RetrieveNotesToDisplay();
            this.isFilteringActive = (string.IsNullOrEmpty(this.NotesMakerFilter) == false);
        }

        private void ResetFilter()
        {
            this.NotesMakerFilter = string.Empty;
            this.isFilteringActive = false;
            this.RetrieveNotesToDisplay();
        }

        private bool CanResetFilter()
        {
            return this.isFilteringActive;
        }

        private void Refresh()
        {
            if (CanResetFilter() == false)
            {
                this.NotesMakerFilter = string.Empty;
            }
            this.RetrieveNotesToDisplay();
        }

        private void Logout()
        {
            MessageBoxResult deleteConfirmation = MessageBox.
                Show(string.Format(CONFIRM_LOGOUT_MESSAGE),
                                   CONFIRM_LOGOUT_TITLE, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (deleteConfirmation != MessageBoxResult.Yes)
            {
                return;
            }
            this.loginViewModel = new LoginViewModel();

            //Ferme la page principale
            this.window.Close();

            this.loginViewModel.Show();
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
