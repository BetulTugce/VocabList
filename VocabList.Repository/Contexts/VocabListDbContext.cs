using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VocabList.Core.Entities;
using VocabList.Core.Entities.Identity;

namespace VocabList.Repository.Contexts
{
    public class VocabListDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public VocabListDbContext(DbContextOptions<VocabListDbContext> options) : base(options)
        {
            
        }

        public DbSet<WordList> WordLists { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Sentence> Sentences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configurationları tarayıp uygular..
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
