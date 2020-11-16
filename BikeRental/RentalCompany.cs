namespace BikeRental
{
    public class RentalCompany : IRentalCompany
    {
        private readonly IBikeService _service;
        private readonly IIncomeCalculator _calculator;
        private readonly IRentedBikeService _rentedBikeService;
        public RentalCompany(string name, IBikeService service, IIncomeCalculator calculator, IRentedBikeService rentedService)
        {
            if(string.IsNullOrEmpty(name))
                throw new RentalCompanyNotNullException();
            Name = name;
            _service = service;
            _calculator = calculator;
            _rentedBikeService = rentedService;
        }
        public string Name { get; }
        public void StartRent(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BikeIdNullException();

            var bike = _service.GetBikeById(id);
            _rentedBikeService.StartRentOfBike(bike);
        }

        public decimal EndRent(string id)
        {
            var income = _rentedBikeService.EndRentOfBike(id);
            return income;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            var rentedBikesHistory = _rentedBikeService.AllRentedBikesHistory(year);
            var completedRentedBikesIncomes = _calculator.AllIncomes(rentedBikesHistory);
            return completedRentedBikesIncomes + (includeNotCompletedRentals ? _rentedBikeService.AllActiveRentedBikesIncomes(year) : 0);
        }
    }
}
