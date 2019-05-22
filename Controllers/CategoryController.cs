using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers
{
    [Route("api/v1/[controller]")]
    public class CategoryController : Controller
    {
        //criando uma propriedade de acesso para nosso StoreDataContext - para acesso aos dados
        private readonly StoreDataContext _context;

        public CategoryController(StoreDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        } 

       
        [HttpGet]
        public IEnumerable<Category> GetCategories() 
        {
            return _context.Categories.AsNoTracking().ToList();
        }

        [HttpGet("{id}")]
        public Category GetCategoryById(int id)
        {
            return _context.Categories.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
        }


        [HttpGet("{id}/products")]
        public IEnumerable<Product> GetProductsByCategoryId(int id)
        {
            return _context.Products.AsNoTracking().Where(x => x.CategoryId == id).ToList();
        }

        [HttpPost]
        public Category CreateCategory([FromBody]Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges(); //é necessário o SaveChanges pois o _context é conhecido só em tempo de execução, se não executar o SaveChanges os dados não são persistidos no banco

            return category;
        }

        [HttpPut]
        public Category UpdateCategory([FromBody]Category category)
        {
            _context.Entry<Category>(category).State = EntityState.Modified;
            _context.SaveChanges();

            return category;
        }

        [HttpDelete]
        public Category DeleteCategory([FromBody]Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return category;
        }
    }
}
