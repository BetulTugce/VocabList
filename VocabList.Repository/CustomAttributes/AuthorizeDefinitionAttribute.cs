using VocabList.Repository.Enums;

namespace VocabList.Repository.CustomAttributes
{
    public class AuthorizeDefinitionAttribute : Attribute
    {
        // Yetkilendirmeye tabi tutulacak actionlarla ilgili bilgileri toplayacak attribute.

        public string Menu { get; set; }
        public string Definition { get; set; }

        // Endpointin reading, writing vs. hangi operasyonu yaptığının bilgisini tutar.
        public ActionType ActionType { get; set; }
    }
}
