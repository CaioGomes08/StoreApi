using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.CategoryViewModels;
using ProductCatalog.ViewModels.ProductViewModels;

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
        public IEnumerable<ListCategoryViewModel> GetCategories() 
        {
            return _context.Categories
                     .Select(c => new ListCategoryViewModel
                     {
                         Title = c.Title                         
                     })
                     .AsNoTracking()
                     .ToList();
        }

        [HttpGet("{id}")]
        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);  
        }


        [HttpGet("{id}/products")]
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

        [HttpPost]
        public ResultViewModel CreateCategory([FromBody]EditorCategoryViewModel model)
        {

            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possivel criar a categoria!",
                    Data = model.Notifications
                };

            var category = new Category();
            category.Title = model.Title;

            _context.Categories.Add(category);
            _context.SaveChanges(); //é necessário o SaveChanges pois o _context é conhecido só em tempo de execução, se não executar o SaveChanges os dados não são persistidos no banco

            return new ResultViewModel
            {
                Success = true,
                Message = "Categoria criada com sucesso!",
                Data = category
            };
        }

        [HttpPut]
        public ResultViewModel UpdateCategory([FromBody]EditorCategoryViewModel model)
        {

            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possivel editar a categoria!",
                    Data = model.Notifications
                };

            var category = _context.Categories.Find(model.Id);
            category.Title = model.Title;

            _context.Entry<Category>(category).State = EntityState.Modified;
            _context.SaveChanges();

            return new ResultViewModel
            {
                Success = true,
                Message = "Categoria editada com sucesso!",
                Data = category
            }; ;
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
