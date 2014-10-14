using System.Runtime.Serialization;

namespace NoteManagerObjectModel
{
    [DataContract]
    public class UserData
    {
        [DataMember]
        public int userId { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
    }
}
