using System.Collections.Generic;

namespace BikeRental
{
    public interface IBikeService
    {
        void AddBike(string id, decimal pricePerMinute);
        void RemoveBike(string id);
        IList<Bike> GetBikes();
        Bike GetBikeById(string id);
    }
}
