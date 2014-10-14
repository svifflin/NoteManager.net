using System.Text.RegularExpressions;
namespace NoteManagerUI.Common
{
    class Helpers
    {
        /// <summary>
        /// Vérifier que l'adresse email est valide
        /// </summary>
        public static bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
        }
    }
}
