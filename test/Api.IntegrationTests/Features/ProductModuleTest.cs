using System.Net.Http.Json;
using Api.Domain.Entities;
using Api.Features.Products.Queries.GetProducts;
using FluentAssertions;
using NUnit.Framework;

namespace Api.IntegrationTests.Features;

public class ProductsModuleTests : TestBase
{
    [Test]
    public async Task GetProducts()
    {
        //Arrange
        await AddAsync(new Product(0, "Product 1", 10.0));
        await AddAsync(new Product(0, "Product 2", 20.0));

        var client = Application.CreateClient();

        //Act
        var products = await client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/{nameof(Product)}");
        //Assert
        products.Should().NotBeNullOrEmpty();
        products.Count.Should().Be(2);

    }
}