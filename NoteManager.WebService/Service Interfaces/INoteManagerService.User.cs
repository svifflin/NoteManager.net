using System.Collections.Generic;
using System.ServiceModel;
using NoteManagerEntityModel;
using NoteManagerObjectModel;

namespace NoteManagerWebService
{
    public partial interface INoteManagerService
    {
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        UserData GetUser(int userID);

        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        int AddUser(UserData userData);

        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        bool RemoveUser(int userID);

        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        UserData UpdateUser(UserData user);
    }
}