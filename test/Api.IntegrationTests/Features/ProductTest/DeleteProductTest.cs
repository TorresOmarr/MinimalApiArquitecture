using System.Net;
using Api.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Api.IntegrationTests.Features.ProductTest;
public class DeleteProductTest : TestBase
{

    [Test]
    public async Task Product_Delete_WithUserAdmin_Should_Return_Ok_Or_NotFound()
    {
        var (Client, UserId) = await GetClientAsAdmin();
        // Arrange
        var product = new Product(0, "Description 1", 10.0);

        await AddAsync(product);

        // Act
        var responseMustSuccess = await Client.DeleteAsync($"api/v1/product/{product.ProductId}");
        var responseMustFail = await Client.DeleteAsync($"api/v1/product/0");


        responseMustSuccess.StatusCode.Should().Be(HttpStatusCode.OK);
        responseMustFail.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}