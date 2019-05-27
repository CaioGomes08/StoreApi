using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.CategoryViewModels;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{
    [Route("api/v1/[controller]")]
    public class CategoryController : Controller
    {
        //criando uma propriedade de acesso para nosso StoreDataContext - para acesso aos dados
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        } 

       
        [HttpGet]
        public IEnumerable<ListCategoryViewModel> GetCategories() 
        {
            return _categoryRepository.Get();
        }

        [HttpGet("{id}")]
        public Category GetCategoryById(int id)
        {
            return _categoryRepository.Get(id); 
        }


        [HttpGet("{id}/products")]
        public IEnumerable<ListProductViewModel> GetProductsByCategoryId(int id)
        {
            return _categoryRepository.GetProductsByCategoryId(id);
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
            category.Description = model.Description;

            //Salvar
            _categoryRepository.Save(category);

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

            var category = _categoryRepository.Get(model.Id);
            category.Title = model.Title;
            category.Description = model.Description;

            //Update
            _categoryRepository.Update(category);

            return new ResultViewModel
            {
                Success = true,
                Message = "Categoria editada com sucesso!",
                Data = category
            }; ;
        }

        [HttpDelete("{id}")]
        public ResultViewModel DeleteCategory(int id)
        {
           

            var model = _categoryRepository.Get(id);
            model.Title = model.Title;
            model.Description = model.Description;
            
            if (model == null)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Erro ao excluir categoria!"                    
                };

            //Delete
            _categoryRepository.Delete(model);

            return new ResultViewModel
            {
                Success = true,
                Message = "Categoria excluida com sucesso!",
                Data = model
            };
        }
    }
}
