using System;
using System.ComponentModel;
using System.Windows.Input;
using NoteManagerUI.View.ViewInterfaces;

namespace NoteManagerUI.ViewModel
{
    /// <summary>
    /// Class to use ase mother of view models
    /// </summary>
    public class BaseViewModel<ViewType> : INotifyPropertyChanged
    where ViewType : IView
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ViewType view;
        public ViewType View
        {
            get
            {
                return this.view;
            }
        }

        public BaseViewModel(ViewType view)
        {
            this.view = view;
            this.View.DataContext = this;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
