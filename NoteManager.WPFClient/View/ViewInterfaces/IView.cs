namespace NoteManagerUI.View.ViewInterfaces
{
    public interface IView
    {
        object DataContext { get; set; }
        void Close();
    }
}
