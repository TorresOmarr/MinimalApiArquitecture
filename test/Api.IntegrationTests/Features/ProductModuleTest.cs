using System.Net;
using System.Net.Http.Json;
using Api.Domain.Entities;
using Api.Features.Products.Commands.CreateProduct;
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
        products?.Count.Should().Be(2);

    }
    [Test]
    public async Task CreateProductWithValidFieldsAndUserAdmin()
    {

        var (Client, UserId) = await GetClientAsAdmin();

        var product = new CreateProductCommand
        {
            Description = "Product 1",
            Price = 10.0
        };

        var response = await Client.PostAsJsonAsync($"/api/{nameof(Product)}", product);

        response.EnsureSuccessStatusCode();


    }
    [Test]
    public async Task CreateProduct_ProducesException_WithAnonymUser()
    {

        var client = Application.CreateClient();

        var product = new CreateProductCommand
        {
            Description = "Product 1",
            Price = 10.0
        };
        var response = await client.PostAsJsonAsync($"/api/{nameof(Product)}", product);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Test]
    public async Task Product_IsNotCreated_WhenInvalidFieldsAreProvided_AndUserIsAdmin()
    {
        var (Client, UserId) = await GetClientAsAdmin();

        var command = new CreateProductCommand()
        {
            Description = "",
            Price = 0.0
        };

        var response = await Client.PostAsJsonAsync($"/api/{nameof(Product)}", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


}