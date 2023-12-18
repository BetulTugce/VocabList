using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VocabList.Core.Entities;

namespace VocabList.Repository.Configurations
{
    public class WordListConfiguration : IEntityTypeConfiguration<WordList>
    {
        public void Configure(EntityTypeBuilder<WordList> builder)
        {
            builder.HasKey(x => x.Id); //Id primary key olarak belirlendi.
            builder.Property(x => x.Id).UseIdentityColumn(); //Id kolonu bir bir artacak şekilde ayarlandı.
            builder.Property(x => x.Name).IsRequired(); //Name kolonu zorunlu
            builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValue(DateTime.Now); //CreatedDate otomatik olarak DateTime.Now değeri alacak.
            builder.Property(x => x.UpdatedDate).IsRequired(false); //UpdatedDate kolonu boş geçilebilir.

            builder.Property(x => x.AppUserId).IsRequired();
            builder.HasOne(x => x.AppUser)
                   .WithMany(w => w.WordLists)
                   .HasForeignKey(x => x.AppUserId);

            builder.HasMany(x => x.Words)
                   .WithOne(s => s.WordList)
                   .HasForeignKey(s => s.WordListId);

            builder.ToTable("WordLists"); //Tablo ismi
        }
    }
}
