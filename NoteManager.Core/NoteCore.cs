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
    public static class NoteCore
    {
        public static List<Note> NotesListByUserID(int userID)
        {
            List<Note> notesList = new List<Note>();

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    var notes = from note in database.Notes
                                where note.n_user_id == userID
                                orderby note.n_date_modif descending
                                select note;

                    notesList = notes.ToList();
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
                        SystemOperation = "Iterate through notes",
                        SystemReason = "Exception reading notes",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return notesList;
        }

        public static List<Note> NotesListByFilter(string filter, int userID)
        {
            List<Note> notesList = new List<Note>();

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    var notes = from note in database.Notes
                                where ((note.n_title.Contains(filter) || note.n_content.Contains(filter))
                                    && note.n_user_id == userID)
                                orderby note.n_date_modif descending
                                select note;

                    notesList = notes.ToList();
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
                        SystemOperation = "Iterate through notes",
                        SystemReason = "Exception reading notes",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return notesList;
        }

        public static Note GetNote(int noteID)
        {
            Note noteToGet = new Note();

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    var notes = from note in database.Notes
                                where note.n_id == noteID
                                select note;
                    noteToGet = notes.FirstOrDefault();
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
                        SystemOperation = "Iterate through notes",
                        SystemReason = "Exception reading notes",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return noteToGet;
        }

        public static int AddNote(Note note)
        {
            int result = 0;

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    note.n_date_creation = DateTime.Now;
                    note.n_date_modif = DateTime.Now;

                    database.Notes.Add(note);
                    database.SaveChanges();

                    Note newNote = database.Notes.FirstOrDefault(n => n.n_id == note.n_id);
                    result = (newNote == null) ? 0 : newNote.n_id;
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
                        SystemOperation = "Adding note",
                        SystemReason = "Exception adding note",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return result;
        }

        public static bool RemoveNote(int noteID)
        {
            bool result = false;

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    Note noteToRemove = database.Notes.FirstOrDefault(n => n.n_id == noteID);

                    if (noteToRemove != null)
                    {
                        database.Notes.Remove(noteToRemove);
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
                        SystemOperation = "Removing note",
                        SystemReason = "Removing note",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return result;
        }

        public static Note UpdateNote(Note note)
        {
            Note result = null;

            try
            {
                using (NoteManagerEntities database = new NoteManagerEntities())
                {
                    database.Configuration.LazyLoadingEnabled = true;
                    database.Configuration.ProxyCreationEnabled = false;

                    Note noteToUpdate = database.Notes.FirstOrDefault(n => n.n_id == note.n_id);
                    if (noteToUpdate == null)
                    {
                        noteToUpdate = new Note()
                        {
                            n_id = 0,
                            n_title = note.n_title,
                            n_content = note.n_content,
                            n_date_creation = DateTime.Now,
                            n_date_modif = DateTime.Now,
                            n_user_id = note.n_user_id
                        };
                        database.Notes.Add(noteToUpdate);
                    }
                    else
                    {
                        noteToUpdate.n_title = note.n_title;
                        noteToUpdate.n_content = note.n_content;
                        noteToUpdate.n_date_modif = DateTime.Now;
                        noteToUpdate.n_user_id = note.n_user_id;
                    }

                    database.SaveChanges();
                    result = database.Notes.FirstOrDefault(n => n.n_id == noteToUpdate.n_id);
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
                        SystemOperation = "Updating note",
                        SystemReason = "Updating note",
                        SystemMessage = e.Message
                    };

                    throw new FaultException<SystemFault>(sf);
                }
            }

            return result;
        }
    }
}
