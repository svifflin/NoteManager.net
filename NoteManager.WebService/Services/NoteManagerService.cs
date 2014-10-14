using System.Collections.Generic;
using System.ServiceModel;
using NoteManagerCore;
using NoteManagerEntityModel;
using NoteManagerObjectModel;

namespace NoteManagerWebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public partial class NoteManagerService : INoteManagerService
    {
        public string Test() { return "Test successful."; }

        public void Login() { }
    }
}