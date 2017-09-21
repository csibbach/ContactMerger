using System.Collections.Generic;

namespace ContactMerger.Models
{
    public class ContactSet
    {
        public IDictionary<string, IList<ContactRelationship>> ContactGrid;
    }
}