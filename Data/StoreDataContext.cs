using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data.Maps;
using ProductCatalog.Models;

namespace ProductCatalog.Data
{
    public class StoreDataContext: DbContext
    {
        //Define quais classes quero que seja mapeadas para o meu BD
        public DbSet<Product> Products {get;set;} //propriedades para mapear meus produtos
        public DbSet<Category> Categories {get;set;} //propriedades para mapear minhas categorias

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ProductCatalog;Data Source=DSK-CAIO");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //toda vez que gerar o banco ele vai aplicar as configurações definidas nos maps
            builder.ApplyConfiguration(new ProductMap());
            builder.ApplyConfiguration(new CategoryMap());
        }
    }
}