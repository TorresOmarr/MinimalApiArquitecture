using System.Net;
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
        var productToAdd = Product.Create(productId: 0, description: "Product 01", price: 25000);
        await AddAsync(productToAdd);

        var (Client, UserId) = await GetClientAsAdmin();

        //Act
        var product = await Client.GetFromJsonAsync<GetProductsQueryResponse>($"/api/v1/{nameof(Product)}/{productToAdd.ProductId}");

        //Assert
        product.Should().NotBeNull();
        product?.ProductId.Should().Be(productToAdd.ProductId);
        product?.Description.Should().Be(productToAdd.Description);
        product?.Price.Should().Be(productToAdd.Price);

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
        var product = Product.Create(productId: 0, description: "Product 01", price: 25000);
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
        var product = Product.Create(productId: 0, description: "Product 01", price: 25000);
        await AddAsync(product);

        var client = Application.CreateClient();

        //Act and Assert
        await FluentActions.Invoking(() =>
                client.GetFromJsonAsync<List<GetProductsQueryResponse>>($"/api/v1/{nameof(Product)}/{product.ProductId}"))
                    .Should().ThrowAsync<HttpRequestException>();
    }
}