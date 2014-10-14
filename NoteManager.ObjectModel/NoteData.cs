using System.Runtime.Serialization;

namespace NoteManagerObjectModel
{
    [DataContract]
    public class NoteData
    {
        [DataMember]
        public int noteID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public System.DateTime DateCreation { get; set; }
        [DataMember]
        public System.DateTime DateModification { get; set; }
        [DataMember]
        public int UserID { get; set; }
    }
}
