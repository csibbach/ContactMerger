using System.Collections.Generic;

namespace ContactMerger.Models
{
    public class ContactRelationship
    {
        public Contact Contact;
        public IList<string> ContactAccountMap;
    }
}