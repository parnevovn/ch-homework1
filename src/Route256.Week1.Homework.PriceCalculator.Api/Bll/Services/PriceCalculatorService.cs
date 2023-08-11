using Route256.Week1.Homework.PriceCalculator.Api.Bll.Const;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Enum;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Exceptions;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Extensions;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;
using System.ComponentModel;
using System.Linq;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private const decimal volumeToPriceRatio = 3.27m;
    private const decimal weightToPriceRatio = 1.34m;
    private const int defaultDistance = 1000;

    private readonly IStorageRepository _storageRepository;
    
    public PriceCalculatorService(
        IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }

    public decimal CalculatePrice(IReadOnlyList<GoodModel> goods, ParamsModel param)
    {
        InitParams(param, out var distance);

        CheckGoods(goods);

        CheckDistance(distance);

        var volumePrice = CalculatePriceByVolume(goods, out var volume);
        var weightPrice = CalculatePriceByWeight(goods, out var weight);

        var resultPrice = Math.Max(volumePrice, weightPrice) * (distance / Constants.KmToM);
        
        // ед. измерения хранения не меняем, так как предпологаем, что уже что-то хранится в см3 и т.
        _storageRepository.Save(new StorageEntity(
            DateTime.UtcNow,
            volume,
            weight,
            resultPrice,
            distance));
        
        return resultPrice;
    }

    private void InitParams(
        ParamsModel param,
        out int distance)
    {
        distance = defaultDistance;

        if (param != null)
        {
            distance = param.Distance;
        }
    }

    private void CheckGoods(IReadOnlyList<GoodModel> goods)
    {
        if (goods == null)
        {
            throw new ArgumentNullException(nameof(goods));
        }

        foreach (GoodModel good in goods)
        {
            good.GoodNotNull()
                .GoodAttributesNotOverflow();
        }
    }

    private void CheckDistance(int distance)
    {
        if (distance < 1)
        {
            throw new AnaliticsOverflowException(OverflowReason.ResultDistanceToSmall);
        }
    }

    private decimal CalculatePriceByVolume(
        IReadOnlyList<GoodModel> goods,
        out decimal volume)
    {
        volume = goods
            .Select(x => x.Height * x.Length * x.Width / Constants.Mm3ToSm3) // приходит в мм, пересчитываем в см3 
            .Sum();

        return volume * volumeToPriceRatio;
    }
    
    private decimal CalculatePriceByWeight(
        IReadOnlyList<GoodModel> goods,
        out decimal weight)
    {
        weight = goods
            .Select(x => x.Weight)
            .Sum();

        weight = weight / Constants.TonToKg; // приходит в кг, пересчитываем в тонны

        return weight * weightToPriceRatio;
    }

    public CalculationLogModel[] QueryLog(int take)
    {
        if (take == 0)
        {
            return Array.Empty<CalculationLogModel>();
        }
        
        var log = _storageRepository.Query()
            .OrderByDescending(x => x.At)
            .Take(take)
            .ToArray();

        return log
            .Select(x => new CalculationLogModel(
                x.Volume, 
                x.Weight,
                x.Price,
                x.Distance))
            .ToArray();
    }

    public void DeleteLog()
    {
        _storageRepository.Clear();
    }

    public Report01Model GetReport01()
    {
        return new ReportService(_storageRepository).GetReport01();
    }
}