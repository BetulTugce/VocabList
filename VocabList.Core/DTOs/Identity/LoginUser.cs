using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabList.Core.DTOs.Identity
{
    public class LoginUser
    {
        public string UsernameOrEmail { get; set;}
        public string Password { get; set; }
    }
}
