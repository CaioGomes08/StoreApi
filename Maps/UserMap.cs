using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Maps
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
            builder.Property(x => x.Sobrenome).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
            builder.Property(x => x.Senha).IsRequired().HasMaxLength(32).HasColumnType("varchar(32)");
            builder.Property(x => x.Perfil).IsRequired().HasMaxLength(15).HasColumnType("varchar(15)");
        }
    }
}
