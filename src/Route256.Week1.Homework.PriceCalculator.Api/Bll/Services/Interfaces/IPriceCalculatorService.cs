using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;

public interface IPriceCalculatorService
{
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods, ParamsModel param = null); //чтобы не перегружать метод и не дублировать код, решил добавить необязательный параметр, который выше задаст значение по умолчанию для старых версий
    void DeleteLog();
    CalculationLogModel[] QueryLog(int take);
    public Report01Model GetReport01();
}