using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocabList.Core.Entities.Common;
using VocabList.Core.Entities.Identity;

namespace VocabList.Core.Entities
{
    public class WordList : BaseEntity
    {
        public string Name { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public virtual List<Word> Words { get; set; }
    }
}
