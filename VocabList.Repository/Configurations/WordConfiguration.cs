using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VocabList.Core.Entities;

namespace VocabList.Repository.Configurations
{
    public class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.HasKey(x => x.Id); //Id primary key olarak belirlendi.
            builder.Property(x => x.Id).UseIdentityColumn(); //Id kolonu bir bir artacak şekilde ayarlandı.
            builder.Property(x => x.Value).IsRequired(); //Value kolonu zorunlu
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UpdatedDate).IsRequired(false); //UpdatedDate kolonu boş geçilebilir.

            builder.Property(x => x.WordListId).IsRequired();
            builder.HasOne(x => x.WordList)
                   .WithMany(u => u.Words)
                   .HasForeignKey(x => x.WordListId);

            builder.HasMany(x => x.Sentences)
                   .WithOne(s => s.Word)
                   .HasForeignKey(s => s.WordId);

            builder.ToTable("Words"); //Tablo ismi
        }
    }
}
