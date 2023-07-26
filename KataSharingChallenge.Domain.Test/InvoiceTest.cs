using FluentAssertions;

namespace KataSharingChallenge.Domain.Test;

public class InvoiceTest
{
    [Theory]
    [InlineData(3, 1.21, "3.63 €")]
    [InlineData(1, 1.21, "1.21 €")]
    public void MultipleArticlesWithoutTax(int count, decimal unitPrice, string expected) =>
        Invoice.Create(count, unitPrice).ToString().Should().Be(expected);

    [Theory]
    [InlineData(3, 1.21, 5, "3.81 €")]
    [InlineData(3, 1.21, 20, "4.36 €")]
    public void MultipleArticlesWithTax(int count, decimal unitPrice, decimal tax, string expected) =>
        Invoice.Create(count, unitPrice).WithTax(tax).ToString().Should().Be(expected);

    [Theory]
    [InlineData(5, 345, 10, "1840.58 €")]
    [InlineData(5, 200, 10, "1067.00 €")]
    [InlineData(5, 1299, 10, "6787.28 €")]
    [InlineData(5, 1000, 10, "5225.00 €")]
    public void MultipleArticlesWithTaxAndDiscount(int count, decimal unitPrice, decimal tax, string expected) =>
        Invoice.Create(count, unitPrice).WithTax(tax).ToString().Should().Be(expected);
}