//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoteManagerEntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Note
    {
        public int n_id { get; set; }
        public string n_title { get; set; }
        public string n_content { get; set; }
        public System.DateTime n_date_creation { get; set; }
        public System.DateTime n_date_modif { get; set; }
        public int n_user_id { get; set; }
    
        public virtual User User { get; set; }
    }
}