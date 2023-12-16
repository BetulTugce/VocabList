using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VocabList.Core.Entities.Identity;

namespace VocabList.Repository.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(x => x.WordLists)
                   .WithOne(s => s.AppUser)
                   .HasForeignKey(s => s.AppUserId);
        }
    }
}
