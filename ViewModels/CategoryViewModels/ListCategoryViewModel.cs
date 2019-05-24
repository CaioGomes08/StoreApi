using ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.ViewModels.CategoryViewModels
{
    public class ListCategoryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }   
        public string Description { get; set; }
    }
}
