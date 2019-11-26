using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{
    [Route("api/v1/[controller]")]
    public class ProductController : Controller
    {

        private readonly ProductRepository _productRepository;

        public ProductController(ProductRepository productRepository)
        {

            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpGet, Authorize]
        //[ResponseCache(Duration = 10)] //adiciona no header da requisição a duração do cache da nossa requisição
        public IEnumerable<ListProductViewModel> GetProducts()
        {
            return _productRepository.Get();
        }

        [HttpGet("{id}")]
        public Product GetProductById(int id)
        {
            return _productRepository.Get(id);
        }

        [HttpPost]
        public ResultViewModel CreateProduct([FromBody]EditorProductViewModel model)
        {
            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível cadastrar o produto",
                    Data = model.Notifications
                };


            var product = new Product();
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.CreateDate = DateTime.Now; //nunca recebe essa informação via tela
            product.Description = model.Description;
            product.Image = Convert.FromBase64String(model.Image);
            product.LastUpdateDate = DateTime.Now; //nunca recebe essa informação via tela
            product.Price = model.Price;
            product.Quantity = model.Quantity;

            //Salvar
            _productRepository.Save(product);

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso!",
                Data = product
            };
        }

        [HttpPut]
        public ResultViewModel UpdateProduct([FromBody]EditorProductViewModel model)
        {
            //Utilizando o pacote Flunt para definir as validações
            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível cadastrar o produto",
                    Data = model.Notifications
                };

            var product = _productRepository.Get(model.Id);
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            //product.CreateDate = DateTime.Now; //nunca recebe essa informação via tela
            product.Description = model.Description;
            product.Image = Convert.FromBase64String(model.Image);
            product.LastUpdateDate = DateTime.Now; //nunca recebe essa informação via tela
            product.Price = model.Price;
            product.Quantity = model.Quantity;

            //Editar e salvar
            _productRepository.Update(product);

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto alterado com sucesso!",
                Data = product
            };
        }

        [HttpDelete("{id}")]
        public ResultViewModel DeleteProduct(int id)
        {
                        
            var product = _productRepository.Get(id);
      

            if (product == null)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível excluir esse produto!"                    
                };


            //Excluir
            _productRepository.Delete(product);

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto excluido com sucesso!",
                Data = product
            };
        }

    }
}
