using System.Collections.Generic;
using NoteManagerCore;
using NoteManagerEntityModel;
using NoteManagerObjectModel;

namespace NoteManagerWebService
{
    public partial class NoteManagerService : INoteManagerService
    {
        public UserData GetUser(int userID)
        {
            UserData userData = new UserData();

            userData = UserToUserData(UserCore.GetUser(userID));
            return userData;
        }

        public int AddUser(UserData userData)
        {
            User user = new User();

            user = UserDataToUser(userData);
            return UserCore.AddUser(user);
        }

        public bool RemoveUser(int userID)
        {
            return UserCore.RemoveUser(userID);
        }

        public UserData UpdateUser(UserData userData)
        {
            User user = new User();

            user = UserDataToUser(userData);
            return UserToUserData(UserCore.UpdateUser(user));
        }
    }
}