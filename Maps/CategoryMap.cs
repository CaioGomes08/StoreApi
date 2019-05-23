using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Models;

namespace ProductCatalog.Data.Maps
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category"); //define o nome da tabela 
            builder.HasKey(x => x.Id); //define a chave primaria da tabela
            builder.Property(x => x.Title).IsRequired().HasMaxLength(120).HasColumnType("varchar(120)"); //define as propriedades da coluna
            builder.Property(x => x.Description).IsRequired().HasMaxLength(300).HasColumnType("varchar(300)");
        }
    }
}