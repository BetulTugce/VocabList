using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocabList.Core.Entities.Common;

namespace VocabList.Core.Entities
{
    public class Sentence : BaseEntity
    {
        public string Value { get; set; }

        [ForeignKey("Word")]
        public int? WordId { get; set; }
        public virtual Word Word { get; set; }
    }
}
