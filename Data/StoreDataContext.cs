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

        public StoreDataContext(DbContextOptions options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //toda vez que gerar o banco ele vai aplicar as configurações definidas nos maps
            builder.ApplyConfiguration(new ProductMap());
            builder.ApplyConfiguration(new CategoryMap());
        }
    }
}