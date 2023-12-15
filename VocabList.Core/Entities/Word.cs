using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocabList.Core.Entities.Common;

namespace VocabList.Core.Entities
{
    public class Word : BaseEntity
    {
        public string Value { get; set; }
        public string Description { get; set; }

        public virtual List<Sentence> Sentences { get; set; }
    }
}
