using System.Collections.Generic;
using System.ServiceModel;
using NoteManagerEntityModel;
using NoteManagerObjectModel;

namespace NoteManagerDataService
{
    public partial interface INoteManagerService
    {
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        List<NoteData> NotesListByUserID(int userID);

        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        List<NoteData> NotesListByFilter(string filter, int userID);

        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        NoteData GetNote(int noteID);

        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        int AddNote(NoteData noteData);

        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        bool RemoveNote(int noteID);

        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        NoteData UpdateNote(NoteData noteData);
    }
}