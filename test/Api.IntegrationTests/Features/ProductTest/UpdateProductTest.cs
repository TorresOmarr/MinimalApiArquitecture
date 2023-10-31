using System.Net;
using System.Net.Http.Json;
using Api.Domain.Entities;
using Api.Features.Products.Commands.UpdateProduct;
using FluentAssertions;
using NUnit.Framework;

namespace Api.IntegrationTests.Features.ProductTest;

public class UpdateProductTest : TestBase
{
    [Test]
    public async Task Product_IsUpdated_WithValidFields_AndAuthUser()
    {
        // Arrenge
        var productAdded = Product.Create(productId: 0, description: "Product 01", price: 10.0);
        await AddAsync(productAdded);
        var (Client, UserId) = await GetClientAsAdmin();
        var command = new UpdateProductCommand
        {
            ProductId = productAdded.ProductId,
            Description = "Updated Product",
            Price = 123456
        };

        // Act
        var result = await Client.PutAsJsonAsync($"/api/v1/{nameof(Product)}/{productAdded.ProductId}", command);

        // Assert
        FluentActions.Invoking(() => result.EnsureSuccessStatusCode())
            .Should().NotThrow();

        var product = await FindAsync<Product>(command.ProductId);

        product.Should().NotBeNull();
        product!.Description.Should().Be(command.Description);
        product.Price.Should().Be(command.Price);
        product.LastModifiedBy.Should().Be(UserId);

    }
    [Test]
    public async Task Product_IsNotUpdated_WithInvalidFields_AndAuthUser()
    {
        var productAdded = Product.Create(productId: 0, description: "Product 01", price: 10.0);
        await AddAsync(Product.Create(productId: 0, description: "Product 02", price: 20.0));
        await AddAsync(productAdded);
        var (Client, UserId) = await GetClientAsAdmin();
        var command = new UpdateProductCommand
        {
            ProductId = productAdded.ProductId,
            Description = String.Empty,
            Price = 0
        };

        // Act
        var result = await Client.PutAsJsonAsync($"/api/v1/{nameof(Product)}/{productAdded.ProductId}", command);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

}