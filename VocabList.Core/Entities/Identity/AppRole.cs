using Microsoft.AspNetCore.Identity;

namespace VocabList.Core.Entities.Identity
{
    public class AppRole : IdentityRole<string>
    {
        // Bir endpoint birden fazla role sahip olabilir aynı şekilde bir rol birden fazla endpoint ile ilişkilendirilebilir. (Çoka çok ilişki)
        public ICollection<Endpoint> Endpoints { get; set; }
    }
}
