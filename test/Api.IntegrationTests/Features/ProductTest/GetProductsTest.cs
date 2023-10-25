using System.Net.Http.Json;
using Api.Domain.Entities;
using Api.Features.Products.Queries.GetProducts;
using FluentAssertions;
using NUnit.Framework;

namespace Api.IntegrationTests.Features.ProductTest;

public class GetProductsTest : TestBase
{
    [Test]
    public async Task GetProducts_WithUserAdmin()
    {
        //Arrange
        await AddAsync(new Product(0, "Product 1", 10.0));
        await AddAsync(new Product(0, "Product 2", 20.0));

        var (Client, UserId) = await GetClientAsAdmin();

        //Act
        var products = await Client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/v1/{nameof(Product)}");
        //Assert
        products.Should().NotBeNullOrEmpty();
        products?.Count.Should().Be(2);
    }

    [Test]
    public async Task GetProducts_ProducesException_WithAnonymUser()
    {
        //Arrange
        await AddAsync(new Product(0, "Product 1", 10.0));
        await AddAsync(new Product(0, "Product 2", 20.0));

        var client = Application.CreateClient();

        //Act and Assert
        await FluentActions.Invoking(() =>
                client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/v1/{nameof(Product)}"))
                    .Should().ThrowAsync<HttpRequestException>();
    }

    [Test]
    public async Task GetProducts_ProducesException_WithUserWithOutRolAdmin()
    {
        //Arrange
        await AddAsync(new Product(0, "Product 1", 10.0));
        await AddAsync(new Product(0, "Product 2", 20.0));

        var (Client, UserId) = await GetClientAsDefaultUserAsync();

        //Act and Assert
        await FluentActions.Invoking(() =>
                Client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/v1/{nameof(Product)}"))
                    .Should().ThrowAsync<HttpRequestException>();
    }

}