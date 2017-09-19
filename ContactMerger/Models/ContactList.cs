using System.Collections.Generic;

namespace ContactMerger.Models
{
    public class ContactList
    {
        public List<Contact> Contacts;
        public string AccountEmail;
        public EContactAccountType ContactAccountType;

        public ContactList()
        {
            Contacts = new List<Contact>();
        }
    }

    public enum EContactAccountType
    {
        Google,
        Facebook
    }
}