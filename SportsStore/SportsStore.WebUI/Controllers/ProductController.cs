using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        public int PageSize = 4;

        public ProductController(IProductRepository repository)
        {
            _productRepository = repository;
        }

        public ViewResult List(int page = 1)
        {
            return View(_productRepository.Products
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize));
        }

    }
}