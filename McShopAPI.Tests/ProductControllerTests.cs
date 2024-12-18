using McShopAPI.Controllers;
using McShopAPI.Data;
using McShopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicShopAPI.Tests
{
    public class ProductsControllerTests
    {
        // Метод для отримання тестових категорій
        private List<Category> GetTestCategories()
        {
            return new List<Category>
            {
                new Category { Id = 1, Name = "Instruments", Description = "Musical instruments" }
            };
        }

        // Метод для отримання тестових продуктів
        private List<Product> GetTestProducts(List<Category> categories)
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Guitar",
                    Price = 500.00m,
                    Category = categories.First(c => c.Id == 1), // Використовуємо існуючу категорію
                    Image = "guitar.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Piano",
                    Price = 1000.00m,
                    Category = categories.First(c => c.Id == 1), // Використовуємо існуючу категорію
                    Image = "piano.jpg"
                }
            };
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Підготовка
            var options = new DbContextOptionsBuilder<MusicShopDbContext>()
                .UseInMemoryDatabase(databaseName: "GetProductsTestDb")
                .Options;

            using var context = new MusicShopDbContext(options);

            var categories = GetTestCategories();
            context.Categories.AddRange(categories); // Додаємо тестові категорії
            context.Products.AddRange(GetTestProducts(categories)); // Додаємо тестові продукти
            context.SaveChanges();

            var controller = new ProductsController(context);

            // Виконання
            var result = await controller.GetProducts();

            // Перевірка
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Перетворюємо результат у список анонімних типів
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);

            // Перевіряємо кількість повернених продуктів
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public async Task GetProducts_ReturnsEmptyList_WhenNoProductsExist()
        {
            // Підготовка
            var options = new DbContextOptionsBuilder<MusicShopDbContext>()
                .UseInMemoryDatabase(databaseName: "EmptyProductsTestDb")
                .Options;

            using var context = new MusicShopDbContext(options);

            var controller = new ProductsController(context);

            // Виконання
            var result = await controller.GetProducts();

            // Перевірка
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Перетворюємо результат у список анонімних типів
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);

            // Перевіряємо, що список порожній
            Assert.Empty(returnedProducts);
        }

    }
}
