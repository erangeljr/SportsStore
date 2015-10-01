using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductId = 1, Name = "P1"},
                new Product { ProductId = 2, Name = "P2"},
                new Product { ProductId = 3, Name = "P3"},
                new Product { ProductId = 4, Name = "P4"},
                new Product { ProductId = 5, Name = "P5"},
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            Product[] productArray = result.Products.ToArray();
            Assert.IsTrue(productArray.Length == 2);
            Assert.AreEqual(productArray[0].Name, "P4");
            Assert.AreEqual(productArray[1].Name, "P5");

        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange
            HtmlHelper helper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            MvcHtmlString result = helper.PageLinks(pagingInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());

        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductId = 1, Name = "P1"},
                new Product { ProductId = 2, Name = "P2"},
                new Product { ProductId = 3, Name = "P3"},
                new Product { ProductId = 4, Name = "P4"},
                new Product { ProductId = 5, Name = "P5"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel) controller.List(null,2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(2,pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(5, pageInfo.TotalItems);
            Assert.AreEqual(2, pageInfo.TotalPages);
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductId = 1, Name = "P1", Category = "Apples"},
                new Product { ProductId = 2, Name = "P2", Category = "Apples"},
                new Product { ProductId = 3, Name = "P3", Category = "Plums"},
                new Product { ProductId = 4, Name = "P4", Category = "Oranges"},
            });

            NavController controller = new NavController(mock.Object);
            string[] results = ((IEnumerable<string>) controller.Menu().Model).ToArray();

            Assert.AreEqual(3, results.Length);
            Assert.AreEqual("Apples",results[0]);
            Assert.AreEqual("Oranges", results[1]);
            Assert.AreEqual("Plums", results[2]);

        }
    }
}
