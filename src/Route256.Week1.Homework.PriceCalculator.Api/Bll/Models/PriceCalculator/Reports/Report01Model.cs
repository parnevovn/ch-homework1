namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;

public record Report01Model(
    int MaxWeight,
    int MaxVolume,
    int MaxDistHeaviestGood,
    int MaxDistLargestGood,
    decimal WavgPrice);