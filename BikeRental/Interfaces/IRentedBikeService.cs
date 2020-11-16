using System.Collections.Generic;

namespace BikeRental
{
    public interface IRentedBikeService
    {
        IList<RentedBike> AllRentedBikesHistory(int? year);
        void StartRentOfBike(Bike bike);
        decimal EndRentOfBike(string id);
        decimal AllActiveRentedBikesIncomes(int? year);
    }
}