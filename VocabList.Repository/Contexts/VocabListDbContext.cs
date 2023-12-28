using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VocabList.Core.Entities;
using VocabList.Core.Entities.Common;
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
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker : Entity Framework tarafından sağlanan bir özelliktir. Entityler üzerinden yapılan değişiklerin ya da yeni eklenen verinin yakalanmasını sağlayan propertydir. Update operasyonlarında Track edilen verileri yakalayıp elde etmemizi sağlar.
            var datas = ChangeTracker
                 .Entries<BaseEntity>();

            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    // Eğer entity ekleniyorsa, CreatedDate'i şu anki zaman ile ayarlar..
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    // Eğer entity güncelleniyorsa, UpdatedDate'i şu anki zaman ile ayarla.
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configurationları tarayıp uygular..
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
