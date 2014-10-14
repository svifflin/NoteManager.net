using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NoteManagerEntityModel;
using NoteManagerObjectModel;

namespace NoteManagerCore
{
    public static class UserCore
    {
        public static bool Login(string email, string password)
        {
            bool result = false;

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    User userToAuthenticate = database.Users.FirstOrDefault(u => (u.u_email == email && u.u_pwd == password));
                    if (userToAuthenticate != null)
                        result = true;
                }
            }
            catch (Exception e)
            {
                if (e.InnerException is System.Data.SqlClient.SqlException)
                {
                    DatabaseFault dbf = new DatabaseFault
                    {
                        DbOperation = "Connect to database",
                        DbReason = "Exception accessing database",
                        DbMessage = e.InnerException.Message
                    };

                    throw new FaultException<DatabaseFault>(dbf);
                }
                else
                {
                    SystemFault sf = new SystemFault
                    {
                        SystemOperation = "Authentificating user",
                        SystemReason = "Authentificating user",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return result;
        }

        public static User GetUserByEmail(string email)
        {
            User userToGet = new User();

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    var users = from user in database.Users
                                where user.u_email == email
                                select user;
                    userToGet = users.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                if (e.InnerException is System.Data.SqlClient.SqlException)
                {
                    DatabaseFault dbf = new DatabaseFault
                    {
                        DbOperation = "Connect to database",
                        DbReason = "Exception accessing database",
                        DbMessage = e.InnerException.Message
                    };

                    throw new FaultException<DatabaseFault>(dbf);
                }
                else
                {
                    SystemFault sf = new SystemFault
                    {
                        SystemOperation = "Iterate through users",
                        SystemReason = "Exception reading users",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return userToGet;
        }

        public static User GetUser(int userID)
        {
            User userToGet = new User();

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    var users = from user in database.Users
                                where user.u_id == userID
                                select user;
                    userToGet = users.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                if (e.InnerException is System.Data.SqlClient.SqlException)
                {
                    DatabaseFault dbf = new DatabaseFault
                    {
                        DbOperation = "Connect to database",
                        DbReason = "Exception accessing database",
                        DbMessage = e.InnerException.Message
                    };

                    throw new FaultException<DatabaseFault>(dbf);
                }
                else
                {
                    SystemFault sf = new SystemFault
                    {
                        SystemOperation = "Iterate through users",
                        SystemReason = "Exception reading users",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return userToGet;
        }

        public static int AddUser(User user)
        {
            int result = 0;

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    database.Users.Add(user);
                    database.SaveChanges();

                    User newUser = database.Users.FirstOrDefault(u => u.u_id == user.u_id);
                    result = (newUser == null) ? 0 : newUser.u_id;
                }
            }
            catch (Exception e)
            {
                if (e.InnerException is System.Data.SqlClient.SqlException)
                {
                    DatabaseFault dbf = new DatabaseFault
                    {
                        DbOperation = "Connect to database",
                        DbReason = "Exception accessing database",
                        DbMessage = e.InnerException.Message
                    };

                    throw new FaultException<DatabaseFault>(dbf);
                }
                else
                {
                    SystemFault sf = new SystemFault
                    {
                        SystemOperation = "Adding user",
                        SystemReason = "Exception adding user",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return result;
        }

        public static bool RemoveUser(int userID)
        {
            bool result = false;

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    User userToRemove = database.Users.FirstOrDefault(u => u.u_id == userID);

                    if (userToRemove != null)
                    {
                        database.Users.Remove(userToRemove);
                        database.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                if (e.InnerException is System.Data.SqlClient.SqlException)
                {
                    DatabaseFault dbf = new DatabaseFault
                    {
                        DbOperation = "Connect to database",
                        DbReason = "Exception accessing database",
                        DbMessage = e.InnerException.Message
                    };

                    throw new FaultException<DatabaseFault>(dbf);
                }
                else
                {
                    SystemFault sf = new SystemFault
                    {
                        SystemOperation = "Removing user",
                        SystemReason = "Removing user",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return result;
        }

        public static User UpdateUser(User user)
        {
            User result = null;

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    User userToUpdate = database.Users.FirstOrDefault(u => u.u_id == user.u_id);
                    if (userToUpdate == null)
                    {
                        userToUpdate = new User()
                        {
                            u_id = 0,
                            u_email = user.u_email,
                            u_pwd = user.u_pwd,
                            u_nom = user.u_nom,
                            u_prenom = user.u_prenom
                        };
                        database.Users.Add(userToUpdate);
                    }
                    else
                    {
                        userToUpdate.u_email = user.u_email;
                        userToUpdate.u_pwd = user.u_pwd;
                        userToUpdate.u_nom = user.u_nom;
                        userToUpdate.u_prenom = user.u_prenom;
                    }
                    database.SaveChanges();
                    result = database.Users.FirstOrDefault(u => u.u_id == userToUpdate.u_id);
                }
            }
            catch (Exception e)
            {
                if (e.InnerException is System.Data.SqlClient.SqlException)
                {
                    DatabaseFault dbf = new DatabaseFault
                    {
                        DbOperation = "Connect to database",
                        DbReason = "Exception accessing database",
                        DbMessage = e.InnerException.Message
                    };

                    throw new FaultException<DatabaseFault>(dbf);
                }
                else
                {
                    SystemFault sf = new SystemFault
                    {
                        SystemOperation = "Updating user",
                        SystemReason = "Updating user",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return result;
        }
    }
}
