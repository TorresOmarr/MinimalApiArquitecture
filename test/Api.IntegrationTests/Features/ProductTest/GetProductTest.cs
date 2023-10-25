using System.Net.Http.Json;
using Api.Domain.Entities;
using Api.Features.Products.Queries.GetProducts;
using FluentAssertions;
using NUnit.Framework;

namespace Api.IntegrationTests.Features.ProductTest;

public class GetProductTest : TestBase
{

    [Test]
    public async Task GetProduct_WithUserAdmin()
    {
        //Arrange
        var product = new Product(0, "Product 1", 10.0);
        await AddAsync(product);

        var (Client, UserId) = await GetClientAsAdmin();

        //Act
        var products = await Client.GetFromJsonAsync<GetProductsQueryResponse>($"/api/v1/{nameof(Product)}/{product.ProductId}");
        //Assert
        products.Should().NotBeNull();
        products?.ProductId.Should().Be(product.ProductId);
    }

    //consult a product dosent exist

    [Test]
    public async Task ConsultProductDoesNotExist()
    {
        //Arrange
        var (Client, UserId) = await GetClientAsAdmin();

        //Act adn Assert
        await FluentActions.Invoking(() =>
                Client.GetFromJsonAsync<GetProductsQueryResponse>($"/api/v1/{nameof(Product)}/150000000000"))
                    .Should().ThrowAsync<HttpRequestException>();
    }


    [Test]
    public async Task GetProducts_ProducesException_WithAnonymUser()
    {
        //Arrange
        var product = new Product(0, "Product 1", 10.0);
        await AddAsync(product);

        var client = Application.CreateClient();

        //Act and Assert
        await FluentActions.Invoking(() =>
                client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/v1/{nameof(Product)}/{product.ProductId}"))
                    .Should().ThrowAsync<HttpRequestException>();
    }

    [Test]
    public async Task GetProducts_ProducesException_WithUserWithOutRolAdmin()
    {
        //Arrange
        var product = new Product(0, "Product 1", 10.0);
        await AddAsync(product);

        var client = Application.CreateClient();

        //Act and Assert
        await FluentActions.Invoking(() =>
                client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/v1/{nameof(Product)}/{product.ProductId}"))
                    .Should().ThrowAsync<HttpRequestException>();
    }
}