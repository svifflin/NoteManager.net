using System.Collections.Generic;
using System.Runtime.Serialization;
using NoteManagerObjectModel;
using NoteManagerEntityModel;

namespace NoteManagerDataService
{
    public partial class NoteManagerService
    {
        // Classe NoteData - Entity Note
        #region Note-NoteData

        private NoteData NoteToNoteData(Note note)
        {
            NoteData noteData = new NoteData();
            noteData.noteID = note.n_id;
            noteData.Title = note.n_title;
            noteData.Content = note.n_content;
            noteData.DateCreation = note.n_date_creation;
            noteData.DateModification = note.n_date_modif;
            noteData.UserID = note.n_user_id;
            return noteData;
        }

        private Note NoteDataToNote(NoteData noteData)
        {
            Note note = new Note();
            note.n_id = noteData.noteID;
            note.n_title = noteData.Title;
            note.n_content = noteData.Content;
            note.n_date_creation = noteData.DateCreation;
            note.n_date_modif = noteData.DateModification;
            note.n_user_id = noteData.UserID;
            return note;
        }

        private List<NoteData> NoteToNoteDataList(List<Note> notes)
        {
            List<NoteData> noteDatas = new List<NoteData>();

            foreach (Note note in notes)
            {
                NoteData noteData = new NoteData();
                noteData = NoteToNoteData(note);
                noteDatas.Add(noteData);
            }

            return noteDatas;
        }

        #endregion

        // Classe UserData - Entity User
        #region User-UserData

        private UserData UserToUserData(User user)
        {
            UserData userData = new UserData();
            userData.userId = user.u_id;
            userData.Email = user.u_email;
            userData.Password = user.u_pwd;
            userData.FirstName = user.u_prenom;
            userData.LastName = user.u_nom;
            return userData;
        }

        private User UserDataToUser(UserData userData)
        {
            User user = new User();
            user.u_id = userData.userId;
            user.u_email = userData.Email;
            user.u_pwd = userData.Password;
            user.u_nom = userData.LastName;
            user.u_prenom = userData.FirstName;
            return user;
        }

        private List<UserData> UserToUserDataList(List<User> users)
        {
            List<UserData> userDatas = new List<UserData>();

            foreach (User user in users)
            {
                UserData userData = new UserData();
                userData = UserToUserData(user);
                userDatas.Add(userData);
            }

            return userDatas;
        }

        #endregion
    }
}
