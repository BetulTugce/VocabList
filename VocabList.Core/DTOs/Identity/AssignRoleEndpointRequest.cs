namespace VocabList.Core.DTOs.Identity
{
    public class AssignRoleEndpointRequest
    {
        public string[] Roles { get; set; }
        public string Code { get; set; }
        public string Menu { get; set; }
        //public Type? Type { get; set; }
    }
}
