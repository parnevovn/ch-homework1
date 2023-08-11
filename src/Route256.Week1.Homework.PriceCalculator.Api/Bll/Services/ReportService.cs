using Route256.Week1.Homework.PriceCalculator.Api.Bll.Const;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services
{
    public class ReportService : IReportService
    {
        private readonly IStorageRepository _storageRepository;

        public ReportService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public Report01Model GetReport01()
        {
            var CountGoods = _storageRepository.Query().Count();

            if (CountGoods == 0)
            {
                return new Report01Model(0, 0, 0, 0, 0);
            }

            var MaxWeight = GetMaxWeight();

            var MaxVolume = GetMaxVolume();

            var MaxDistHeaviestGood = GetMaxDistHeaviestGood();

            var MaxDistLargestGood = GetMaxDistLargestGood();

            var SumPrices = GetSumPrices();

            var WavgPrice = CountGoods > 0 ? (SumPrices / CountGoods) : 0;

            return new Report01Model(
                    (int)MaxWeight, // переводим в int так как в контракте целочисленное
                    (int)MaxVolume, // переводим в int так как в контракте целочисленное
                    MaxDistHeaviestGood,
                    MaxDistLargestGood,
                    WavgPrice);
        }

        private decimal GetMaxWeight()
        {
            return _storageRepository
                .Query()
                .OrderByDescending(x => x.Weight)
                .First().Weight
                * Constants.TonToKg;
        }

        private decimal GetMaxVolume()
        {
            return _storageRepository
                .Query()
                .OrderByDescending(x => x.Volume)
                .First().Volume
                / Constants.Sm3ToM3;
        }

        private int GetMaxDistHeaviestGood()
        {
            return _storageRepository
                .Query()
                .OrderByDescending(x => x.Weight)
                .First().Distance;
        }

        private int GetMaxDistLargestGood()
        {
            return _storageRepository
                .Query()
                .OrderByDescending(x => x.Volume)
                .First().Distance;
        }

        private decimal GetSumPrices()
        {
            return _storageRepository
                .Query()
                .Sum(x => x.Price);
        }
    }
}
