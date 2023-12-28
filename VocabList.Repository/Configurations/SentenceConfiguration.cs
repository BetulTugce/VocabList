using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VocabList.Core.Entities;

namespace VocabList.Repository.Configurations
{
    public class SentenceConfiguration : IEntityTypeConfiguration<Sentence>
    {
        public void Configure(EntityTypeBuilder<Sentence> builder)
        {
            builder.HasKey(x => x.Id); //Id primary key olarak belirlendi.
            builder.Property(x => x.Id).UseIdentityColumn(); //Id kolonu bir bir artacak şekilde ayarlandı.
            builder.Property(x => x.Value).IsRequired(); //Value kolonu zorunlu
            builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow); //CreatedDate otomatik olarak DateTime.Now değeri alacak.
            builder.Property(x => x.UpdatedDate).IsRequired(false); //UpdatedDate kolonu boş geçilebilir.

            builder.Property(x => x.WordId).IsRequired();
            builder.HasOne(x => x.Word)
                   .WithMany(w => w.Sentences)
                   .HasForeignKey(x => x.WordId);

            builder.ToTable("Sentences"); //Tablo ismi
        }
    }
}
