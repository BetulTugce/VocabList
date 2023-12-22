namespace VocabList.Core.DTOs.Configuration
{
    // AuthorizeDefinition attributeu ile işaretlenmiş tüm endpointleri getirecek methodun döneceği dto.
    public class Menu
    {
        public string Name { get; set; }
        public List<Action> Actions { get; set; } = new();
    }
}
