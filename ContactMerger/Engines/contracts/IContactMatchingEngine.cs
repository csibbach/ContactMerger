using ContactMerger.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactMerger.Engines.contracts
{
    public interface IContactMatchingEngine
    {
        /// <summary>
        /// This function will take contact lists from multiple places and collate them
        /// into a ContactSet that represents the merged data.
        /// Note, this is defined as an async method- I have found it is SO MUCH EASIER
        /// to make things async up front, even if your planned implementation isn't
        /// doing any async things.
        /// </summary>
        /// <param name="contactLists"></param>
        /// <returns></returns>
        Task<ContactSet> MergeContactLists(IList<ContactList> contactLists);

        /// <summary>
        /// Returns true if the two contacts are the same person. What details it uses to do this
        /// are implementation specific.
        /// </summary>
        /// <param name="contact1"></param>
        /// <param name="contact2"></param>
        /// <returns></returns>
        Task<bool> ContactsMatch(Contact contact1, Contact contact2);
    }
}
