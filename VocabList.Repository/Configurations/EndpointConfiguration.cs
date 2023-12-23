using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VocabList.Core.Entities;

namespace VocabList.Repository.Configurations
{
    public class EndpointConfiguration : IEntityTypeConfiguration<Endpoint>
    {
        public void Configure(EntityTypeBuilder<Endpoint> builder)
        {
            builder.HasKey(x => x.Id); //Id primary key olarak belirlendi.
            builder.Property(x => x.Id).UseIdentityColumn(); //Id kolonu bir bir artacak şekilde ayarlandı.
            builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValue(DateTime.Now); //CreatedDate otomatik olarak DateTime.Now değeri alacak.
            builder.Property(x => x.UpdatedDate).IsRequired(false); //UpdatedDate kolonu boş geçilebilir.

            builder.ToTable("Endpoints"); //Tablo ismi
        }
    }
}
