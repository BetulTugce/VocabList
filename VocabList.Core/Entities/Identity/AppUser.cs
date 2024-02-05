using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabList.Core.Entities.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string? ProfileImagePath { get; set; }

        // AccessToken süresi dolduktan sonra RefreshTokenEndDate baz alınarak bu token kontrol edilecek..
        public string? RefreshToken { get; set; } 
        public DateTime? RefreshTokenEndDate { get; set; }

        public virtual List<WordList> WordLists { get; set; }
    }
}
