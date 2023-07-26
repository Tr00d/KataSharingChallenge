using System.Globalization;
using LanguageExt;

namespace KataSharingChallenge.Domain;

public class Invoice
{
    private const decimal FivePercentDiscount = 0.95m;
    private const decimal ThreePercentDiscount = 0.97m;
    private const int DisplayedDecimals = 2;
    private const int FivePercentDiscountThreshold = 5000;
    private const int ThreePercentDiscountThreshold = 1000;
    private readonly decimal grossAmount;
    private readonly Option<decimal> tax;

    private Invoice(decimal grossAmount) => this.grossAmount = grossAmount;

    private Invoice(decimal grossAmount, decimal tax)
        : this(grossAmount) => this.tax = tax;

    public static Invoice Create(int count, decimal unitPrice) => new(count * unitPrice);

    public override string ToString()
    {
        var totalAmount = this.grossAmount * this.EvaluateDiscountRate() * this.EvaluateTaxRate();
        return FormatAmount(RoundAmount(totalAmount));
    }

    public Invoice WithTax(decimal tax) => new(this.grossAmount, tax);

    private decimal EvaluateDiscountRate() =>
        this.grossAmount switch
        {
            >= FivePercentDiscountThreshold => FivePercentDiscount,
            >= ThreePercentDiscountThreshold => ThreePercentDiscount,
            _ => 1,
        };

    private decimal EvaluateTaxRate() => this.tax.Match(some => 1 + some / 100, () => 1);

    private static string FormatAmount(decimal roundedAmount) =>
        $"{roundedAmount.ToString(CultureInfo.InvariantCulture)} €";

    private static decimal RoundAmount(decimal totalAmount) => Math.Round(totalAmount, DisplayedDecimals);
}