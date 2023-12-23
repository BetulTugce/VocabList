using VocabList.Core.Entities.Common;
using VocabList.Core.Entities.Identity;

namespace VocabList.Core.Entities
{
    public class Endpoint : BaseEntity
    {
        public Endpoint()
        {
            // Her Endpoint nesnesi oluşturulduğunda bir rol kümesi oluşturur ve her bir AppRoleün yalnızca bir kez eklenmesine izin verir..
            Roles = new HashSet<AppRole>();
        }
        public string ActionType { get; set; }
        public string HttpType { get; set; }
        public string Definition { get; set; }
        public string Code { get; set; }

        // Bir endpoint bir menuye aittir ancak menuler birden fazla endpoint barındırabilir.(Bire Çok İlişki)
        public Menu Menu { get; set; }

        // Bir endpoint birden fazla role sahip olabilir aynı şekilde bir rol birden fazla endpoint ile ilişkilendirilebilir. (Çoka çok ilişki)
        public ICollection<AppRole> Roles { get; set; }
    }
}
