using VocabList.Core.Entities.Common;

namespace VocabList.Core.Entities
{
    public class Menu : BaseEntity
    {
        public string Name { get; set; }

        // Bir endpoint bir menuye aittir ancak menuler birden fazla endpoint barındırabilir.(Bire Çok İlişki)
        public ICollection<Endpoint> Endpoints { get; set; }
    }
}
