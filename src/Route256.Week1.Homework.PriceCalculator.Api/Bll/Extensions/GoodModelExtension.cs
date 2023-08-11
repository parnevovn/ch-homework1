using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Enum;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Exceptions;
using System.Runtime.CompilerServices;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Extensions
{
    public static class GoodModelExtension
    {
        public static GoodModel GoodNotNull(this GoodModel? src) 
        {
            ArgumentNullException.ThrowIfNull(src);

            return src;
        }

        public static GoodModel GoodAttributesNotOverflow(this GoodModel? src)
        {
            if (src.Height < 0)
            {
                throw new AnaliticsOverflowException(OverflowReason.ResultHeightToSmall);
            }

            if (src.Length < 0)
            {
                throw new AnaliticsOverflowException(OverflowReason.ResultLengthToSmall);
            }

            if (src.Width < 0)
            {
                throw new AnaliticsOverflowException(OverflowReason.ResultWidthToSmall);
            }

            if (src.Weight < 0)
            {
                throw new AnaliticsOverflowException(OverflowReason.ResultWeightToSmall);
            }

            return src;
        }
    }
}
