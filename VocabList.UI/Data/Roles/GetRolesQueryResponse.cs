namespace VocabList.UI.Data.Roles
{
    public class GetRolesQueryResponse
    {
        //public object Datas { get; set; }
        //public int TotalCount { get; set; }

        public List<Data> datas { get; set; }
        public int totalCount { get; set; }

        public class Data
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}
