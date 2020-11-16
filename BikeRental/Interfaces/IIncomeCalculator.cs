using System;
using System.Collections.Generic;

namespace BikeRental
{
    public interface IIncomeCalculator
    {
        decimal AllIncomes(IList<RentedBike> rentedBikeHistory);
        decimal BikeRentCalculatingIncome(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute);
        decimal OneDayCalculatingIncome(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute);
        decimal MoreThenOneDayCalculatingIncome(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute);
    }
}