using Entities.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net.Http.Json;
using TestWebApi;

namespace WebApi.Tests
{
    [TestFixture]
    class ProductTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {

                builder.ConfigureServices(services =>
                {



                });
            });
            _client = _factory.CreateClient();
        }


        [Test]
        public async Task GetProducts_ReturnOkResponce()
        {
            var response = await _client.GetAsync("/api/products?top=0");

            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();

            Assert.That(products is not null);
        }

        [Test]
        public async Task GetProductsWitdhTop_ReturnsCorrectProduct()
        {

            var count = 5;

            var response = await _client.GetAsync("/api/products?top=5");

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();

            response.EnsureSuccessStatusCode();

            Assert.That(products is not null);
            Assert.That(products?.Count <= count);
        }

        [Test]
        public async Task PostProduct_AddNewProduct()
        {
            var newProduct = new Product
            {
                Name = "Test Product",
                Price = 500,
            };
            var response = await _client.PostAsJsonAsync("/api/products", newProduct);
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
            Assert.That(createdProduct is not null);
            Assert.That(newProduct.Name, Is.EqualTo(createdProduct?.Name));
        }
    }
}
