using System.Collections.Generic;

namespace ContactMerger.Models
{
    public class ContactSet
    {
        public IList<ContactAccount> Accounts;
        public IList<ContactRelationship> Relationships;
    }
}