using System;
using System.Collections.Generic;
using System.Linq;


namespace BikeRental
{
    public class IncomeCalculator : IIncomeCalculator
    {
        private readonly decimal _bikeMaxPrice;

        public IncomeCalculator(decimal maxPrice)
        {
            _bikeMaxPrice = maxPrice;
        }

        public decimal AllIncomes(IList<RentedBike> rentedBikesHistory)
        {
            return rentedBikesHistory.Select(rb => rb.Price).Sum();
        }

        public decimal BikeRentCalculatingIncome(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute)
        {
            if (rentStart > rentEnd)
                throw new RentedBikeIncorrectTimeException();
            var daysBikeInRent = (rentEnd.Date - rentStart.Date).TotalDays;
            return daysBikeInRent > 1
                ? MoreThenOneDayCalculatingIncome(rentStart, rentEnd, pricePerMinute)
                : OneDayCalculatingIncome(rentStart, rentEnd, pricePerMinute);
        }

        public decimal OneDayCalculatingIncome(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute)
        {
            var incomeForRent = (decimal)(rentEnd - rentStart).TotalMinutes * pricePerMinute;
            return incomeForRent > _bikeMaxPrice ? _bikeMaxPrice : incomeForRent;
        }

        public decimal MoreThenOneDayCalculatingIncome(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute)
        {
            int minutesInDay = 1440;
            var firstDayRentIncome = (decimal) (minutesInDay - rentStart.TimeOfDay.TotalMinutes) * pricePerMinute;
            if (firstDayRentIncome >= _bikeMaxPrice)
            {
                firstDayRentIncome = _bikeMaxPrice;
            }

            var wholeDaysRentIncome = (decimal) ((rentEnd.Date - rentStart.Date).TotalDays - 1) * _bikeMaxPrice;

            var lastDayRentIncome = (decimal) rentEnd.TimeOfDay.TotalMinutes * pricePerMinute;
            lastDayRentIncome = lastDayRentIncome >= _bikeMaxPrice
                ? _bikeMaxPrice
                : lastDayRentIncome;

            var result = firstDayRentIncome + wholeDaysRentIncome + lastDayRentIncome;
            return result;
        }
    }
}
