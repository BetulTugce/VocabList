namespace VocabList.Core.DTOs.Common
{
    //Abstract class olarak tasarlama sebebim doğrudan örneklendirilmesini engellemek için.
    public abstract class BaseDto
    {
        //Bütün dtolar için ortak propertyleri barındırır..
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
