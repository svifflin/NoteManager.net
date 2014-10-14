using System.Collections.Generic;
using System.ServiceModel;
using NoteManagerEntityModel;
using NoteManagerObjectModel;

namespace NoteManagerWebService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "INoteManagerService" à la fois dans le code et le fichier de configuration.
    [ServiceContract(
        Name = "NoteManagerService")]
    public partial interface INoteManagerService
    {
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        string Test();

        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(DatabaseFault))]
        [OperationContract]
        void Login();
    }
}
