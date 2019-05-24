using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels.CategoryViewModels;
using ProductCatalog.ViewModels.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Repositories
{
    public class CategoryRepository
    {
        //criando uma propriedade de acesso para nosso StoreDataContext - para acesso aos dados
        private readonly StoreDataContext _context;

        public CategoryRepository(StoreDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<ListCategoryViewModel> Get()
        {
           return  _context.Categories
                     .Select(c => new ListCategoryViewModel
                     {
                         Id = c.Id,
                         Title = c.Title,
                         Description = c.Description
                     })
                     .AsNoTracking()
                     .ToList();
        }

        public Category Get(int id)
        {
            return _context.Categories.Find(id);
        }

        public IEnumerable<ListProductViewModel> GetProductsByCategoryId(int id)
        {
            return _context.Products
                        .Where(c => c.CategoryId == id)
                        .Select(c => new ListProductViewModel
                        {
                            Id = c.Id,
                            CategoryId = c.CategoryId,
                            Category = c.Category.Title,
                            Price = c.Price,
                            Title = c.Title
                        })
                        .AsNoTracking()
                        .ToList();
        }

        public void Save(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges(); //é necessário o SaveChanges pois o _context é conhecido só em tempo de execução, se não executar o SaveChanges os dados não são persistidos no banco
        }

        public void Update(Category category)
        {
            _context.Entry<Category>(category).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
