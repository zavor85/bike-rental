using System;
using System.Collections.Generic;
using System.Linq;

namespace BikeRental
{
    public class RentedBikeService : IRentedBikeService
    {
        private readonly IIncomeCalculator _incomeCalculator;
        private readonly Dictionary<string, RentedBike> _rentedBikes = new Dictionary<string, RentedBike>();
        private readonly IList<RentedBike> _rentedBikesHistory = new List<RentedBike>();

        public RentedBikeService(IIncomeCalculator calculator)
        {
            _incomeCalculator = calculator;
        }

        public RentedBikeService(IIncomeCalculator calculator, Dictionary<string, RentedBike> rentedBikes, IList<RentedBike> rentedBikesHistory)
        {
            _incomeCalculator = calculator;
            _rentedBikes = rentedBikes;
            _rentedBikesHistory = rentedBikesHistory;
        }

        public IList<RentedBike> AllRentedBikesHistory(int? year)
        {
            return year.HasValue
                ? _rentedBikesHistory.Where(rb => rb.EndRent.Year == year).ToList()
                : _rentedBikesHistory;
        }

        public void StartRentOfBike(Bike bike)
        {
            if (bike.IsRented || _rentedBikes.ContainsKey(bike.Id))
                throw new BikeAlreadyRentedException();

            _rentedBikes.Add(bike.Id, new RentedBike(bike, DateTime.Now));
            bike.IsRented = true;
        }

        public decimal EndRentOfBike(string id)
        {
            if (!_rentedBikes.ContainsKey(id))
                throw new BikeNotRentedException();

            var rentedBike = _rentedBikes[id];
            var rentEnd = DateTime.Now;
            var rentedBikeIncome = _incomeCalculator.BikeRentCalculatingIncome(rentedBike.StartRent, rentEnd, rentedBike.Price);
            rentedBike.EndOfRentedBike(rentEnd, rentedBikeIncome);
            rentedBike.Bike.IsRented = false;
            _rentedBikes.Remove(rentedBike.Bike.Id);
            _rentedBikesHistory.Add(rentedBike);
            return rentedBikeIncome;
        }

        public decimal AllActiveRentedBikesIncomes(int? year)
        {
            var endRent = DateTime.Now;
            var rentedBikes = year.HasValue
                ? _rentedBikes.Values.Where(rb => rb.StartRent.Year == year)
                : _rentedBikes.Values;

            return rentedBikes.Sum(rb =>
                _incomeCalculator.BikeRentCalculatingIncome(rb.StartRent, endRent, rb.Bike.PricePerMinute));
        }
    }
}
