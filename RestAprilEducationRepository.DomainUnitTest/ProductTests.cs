using RestAprilEducationRepository.Domain;

namespace RestAprilEducationRepository.DomainUnitTest;

public class ProductTests
{
    [Fact]
    public void SetPrice_ValidPrice_SetsPrice()
    {
        // Arrange
        var product = new Product();
        var expectedPrice = 100.50m;

        // Act
        product.SetPrice(expectedPrice);

        // Assert
        Assert.Equal(expectedPrice, product.Price);
    }

    [Fact]
    public void SetPrice_Zero_SetsPrice()
    {
        // Arrange
        var product = new Product();
        var expectedPrice = 0m;

        // Act
        product.SetPrice(expectedPrice);

        // Assert
        Assert.Equal(expectedPrice, product.Price);
    }

    [Fact]
    public void SetPrice_NegativePrice_ThrowsException()
    {
        // Arrange
        var product = new Product();
        var negativePrice = -1m;

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => product.SetPrice(negativePrice));
        Assert.Equal("fiyat alanı 0'dan küçük olamaz", exception.Message);
    }

    [Fact]
    public void SetPrice_LargeNegativePrice_ThrowsException()
    {
        // Arrange
        var product = new Product();
        var largeNegativePrice = -1000.99m;

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => product.SetPrice(largeNegativePrice));
        Assert.Equal("fiyat alanı 0'dan küçük olamaz", exception.Message);
    }

    [Fact]
    public void SetPrice_LargePositivePrice_SetsPrice()
    {
        // Arrange
        var product = new Product();
        var largePrice = 999999.99m;

        // Act
        product.SetPrice(largePrice);

        // Assert
        Assert.Equal(largePrice, product.Price);
    }

    [Fact]
    public void SetPrice_UpdateExistingPrice_SetsNewPrice()
    {
        // Arrange
        var product = new Product();
        product.SetPrice(50m);
        var newPrice = 75.25m;

        // Act
        product.SetPrice(newPrice);

        // Assert
        Assert.Equal(newPrice, product.Price);
    }

    [Fact]
    public void SetPrice_MaxDecimalValue_SetsPrice()
    {
        // Arrange
        var product = new Product();
        var maxPrice = decimal.MaxValue;

        // Act
        product.SetPrice(maxPrice);

        // Assert
        Assert.Equal(maxPrice, product.Price);
    }

    [Fact]
    public void SetPrice_SmallPositivePrice_SetsPrice()
    {
        // Arrange
        var product = new Product();
        var smallPrice = 0.01m;

        // Act
        product.SetPrice(smallPrice);

        // Assert
        Assert.Equal(smallPrice, product.Price);
    }
}
