namespace ContactMerger.Models
{
    public class ContactAccount
    {
        public string AccountEmail;
        public EContactAccountType ContactAccountType;
    }

    public enum EContactAccountType
    {
        Google,
        Facebook
    }
}