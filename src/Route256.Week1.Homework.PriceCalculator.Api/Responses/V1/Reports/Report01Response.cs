namespace Route256.Week1.Homework.PriceCalculator.Api.Responses.V1;

public record Report01Response(
    int MaxWeight,
    int MaxVolume,
    int MaxDistHeaviestGood,
    int MaxDistLargestGood,
    decimal WavgPrice);