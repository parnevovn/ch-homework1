namespace Route256.Week1.Homework.PriceCalculator.Api.Requests.V3;

/// <summary>
/// Харектеристики товара
/// </summary>
/// Сразу смутило что входные параметры должны быть в СИ, то есть в метрах, а тип int по модели
/// Считаю, что подразумевается принебрежение типами относительно реальной бизнес задачи
public record GoodProperties(
    int Height,
    int Length,
    int Width,
    int Weight);