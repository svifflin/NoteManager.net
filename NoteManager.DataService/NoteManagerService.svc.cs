using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace NoteManagerDataService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public partial class NoteManagerService : INoteManagerService
    {
        public string Test() { return "Test successful."; }
    }
}
