using System.Net.Http.Json;
using Api.Domain.Entities;
using Api.Features.Products.Queries.GetProducts;
using FluentAssertions;

namespace Api.IntegrationTestsTCXUnit.Features;

public class GetProductsTest : BaseIntegrationTest
{
    public GetProductsTest(IntergrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetProducts_WithUserAdmin()
    {
        var productsInDB = _context.Products.ToList();
        //Arrange
        await AddAsync(Product.Create(productId: 0, description: "Product 01", price: 10.0));
        await AddAsync(Product.Create(productId: 0, description: "Product 02", price: 20.0));

        var (Client, UserId) = await GetClientAsAdmin();

        //Act
        var products = await Client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/v1/{nameof(Product)}");
        //Assert
        products.Should().NotBeNullOrEmpty();
        products?.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetProducts_ProducesException_WithAnonymUser()
    {
        //Arrange
        await AddAsync(Product.Create(productId: 0, description: "Product 01", price: 10.0));
        await AddAsync(Product.Create(productId: 0, description: "Product 02", price: 20.0));

        var (client, _) = await GetClientAsDefaultUserAsync();

        //Act and Assert
        await FluentActions.Invoking(() =>
                client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/v1/{nameof(Product)}"))
                    .Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetProducts_ProducesException_WithUserWithOutRolAdmin()
    {
        //Arrange
        await AddAsync(Product.Create(productId: 0, description: "Product 01", price: 10.0));
        await AddAsync(Product.Create(productId: 0, description: "Product 02", price: 20.0));

        var (Client, UserId) = await GetClientAsDefaultUserAsync();

        //Act and Assert
        await FluentActions.Invoking(() =>
                Client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/v1/{nameof(Product)}"))
                    .Should().ThrowAsync<HttpRequestException>();
    }


}