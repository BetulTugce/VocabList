using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabList.Core.DTOs.Identity
{
    public class VerifyPasswordResetToken
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
    }
}
