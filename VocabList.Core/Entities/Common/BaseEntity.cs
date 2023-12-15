using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabList.Core.Entities.Common
{
    //Abstract class olarak tasarlama sebebim doğrudan örneklendirilmesini engellemek için. 
    public abstract class BaseEntity
    {
        //Bütün classlar için ortak propertyleri barındırır..
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
