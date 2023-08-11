using Route256.Week1.Homework.PriceCalculator.Api.Bll.Enum;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Exceptions
{
    public sealed class AnaliticsOverflowException : Exception
    {
        public AnaliticsOverflowException(OverflowReason reason) : base(GetMessageByReason(reason))
        { 
        }

        private static string GetMessageByReason(OverflowReason reason)
            => reason switch
            {
                OverflowReason.ResultHeightToSmall => "Высота не должна быть отрицательной",
                OverflowReason.ResultLengthToSmall => "Глубина не должна быть отрицательной",
                OverflowReason.ResultWidthToSmall => "Ширина не должна быть отрицательной",
                OverflowReason.ResultWeightToSmall => "Вес не должен быть отрицательным",
                OverflowReason.ResultDistanceToSmall => "Расстояние не может быть меньше 1 м",
                _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, null)
            };
    }
}
