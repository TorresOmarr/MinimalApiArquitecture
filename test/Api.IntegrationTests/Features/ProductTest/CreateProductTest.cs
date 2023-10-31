using System.Net;
using System.Net.Http.Json;
using Api.Domain.Entities;
using Api.Features.Products.Commands.CreateProduct;
using FluentAssertions;
using NUnit.Framework;

namespace Api.IntegrationTests.Features.ProductTest;


public class CreateProductTest : TestBase
{


    [Test]
    public async Task CreateProductWithValidFieldsAndUserAdmin()
    {

        var (Client, UserId) = await GetClientAsAdmin();

        var product = new CreateProductCommand
        {
            Description = "Product 1",
            Price = 10.0
        };

        var response = await Client.PostAsJsonAsync($"/api/v1/{nameof(Product)}", product);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task CreateProduct_ProducesException_WithAnonymUser()
    {

        var (client, userId) = await GetClientAsDefaultUserAsync();

        var product = new CreateProductCommand
        {
            Description = "Product 1",
            Price = 10.0
        };
        var response = await client.PostAsJsonAsync($"/api/v1/{nameof(Product)}", product);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
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

        var response = await Client.PostAsJsonAsync($"/api/v1/{nameof(Product)}", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}