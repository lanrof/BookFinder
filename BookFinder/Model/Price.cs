
using BookFinder.CurrencyConverter;

namespace BookFinder.Model
{
    public class Price
    {
        public double Cost { get; set; }
        public Currency Currency { get; set; }

        public Price Convert(Currency toCurrency)
        {
            var currencyClient = new CurrencyClient();
            var resutl = currencyClient.GetCurrencyCourses(Currency).Rates;
            double course = (double)GetPropValue((object)resutl, toCurrency.ToString());

            Cost *= (course == 0 ? 1 : course);
            Currency = toCurrency;

            return this;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
