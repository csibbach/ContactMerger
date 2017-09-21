using System.Collections.Generic;

namespace ContactMerger.Models
{
    public class ContactRelationship: Contact
    {
        public bool FirstNameMatches;
        public bool LastNameMatches;
        public bool EmailMatches;
        public bool ContactExists;
        public string AccountEmail;
    }
}