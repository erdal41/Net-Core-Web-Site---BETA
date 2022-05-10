using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySite.Entities.Concrete;
using System;

namespace MySite.Data.Concrete.EntityFramework.Mappings
{
    public class UploadMap : IEntityTypeConfiguration<Upload>
    {
        public void Configure(EntityTypeBuilder<Upload> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.FileName).IsRequired();
            builder.Property(a => a.FileName).HasMaxLength(250);
            builder.Property(m => m.CreatedByName).IsRequired();
            builder.Property(m => m.CreatedByName).HasMaxLength(50);
            builder.Property(m => m.CreatedDate).IsRequired();
            builder.Property(m => m.AltText).HasMaxLength(100);
            builder.Property(m => m.Title).HasMaxLength(100);
            builder.Property(m => m.SubTitle).HasMaxLength(100);
            builder.Property(m => m.FileUrl).HasMaxLength(250);
            builder.Property(m => m.ContentType).HasMaxLength(20);
            builder.Property(a => a.CreatedByName).IsRequired();
            builder.Property(a => a.CreatedByName).HasMaxLength(50);
            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(50);
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.ModifiedDate).IsRequired();
            builder.Property(a => a.Note).HasMaxLength(500);
            builder.ToTable("Uploads");

            builder.HasData(
                new Upload
                {
                    Id = 1,
                    FileName = "default-user-image.png",
                    AltText = "Varsayılan Kullanıcı Resmi",
                    Title = "Varsayılan Kullanıcı Resmi",
                    SubTitle = "Varsayılan Kullanıcı Resmi",
                    FileUrl = "~/admin/assets/img/default-user-image.png",
                    ContentType = "image/png",
                    Size = 1,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "Bu resim varsayılan kullanıcı resmidir."
                },
                new Upload
                {
                    Id = 2,
                    FileName = "default-post-image.jpg",
                    AltText = "Varsayılan Sayfa Resmi",
                    Title = "Varsayılan Sayfa Resmi",
                    SubTitle = "Varsayılan Sayfa Resmi",
                    FileUrl = "~/admin/assets/img/default-post-image.jpg",
                    ContentType = "image/jpeg",
                    Size = 1,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "Bu resim varsayılan sayfa resmidir."
                }
            );
        }
    }
}