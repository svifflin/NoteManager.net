using System.Collections.Generic;
using NoteManagerCore;
using NoteManagerEntityModel;
using NoteManagerObjectModel;

namespace NoteManagerWebService
{
    public partial class NoteManagerService : INoteManagerService
    {
        public List<NoteData> NotesList(int userID)
        {
            List<NoteData> noteDatas = new List<NoteData>();

            noteDatas = NoteToNoteDataList(NoteCore.NotesList(userID));
            return noteDatas;
        }

        public NoteData GetNote(int noteID)
        {
            NoteData noteData = new NoteData();

            noteData = NoteToNoteData(NoteCore.GetNote(noteID));
            return noteData;
        }

        public int AddNote(NoteData noteData)
        {
            Note note = new Note();

            note = NoteDataToNote(noteData);
            return NoteCore.AddNote(note);
        }

        public bool RemoveNote(int noteID)
        {
            return NoteCore.RemoveNote(noteID);
        }

        public NoteData UpdateNote(NoteData noteData)
        {
            Note note = new Note();

            note = NoteDataToNote(noteData);
            return NoteToNoteData(NoteCore.UpdateNote(note));
        }
    }
}