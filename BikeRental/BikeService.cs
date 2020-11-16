using System.Collections.Generic;
using System.Linq;

namespace BikeRental
{
    public class BikeService : IBikeService
    {
        private readonly Dictionary<string, Bike> _bikes;
        private const decimal BikeMinPrice = 0.01m;

        public BikeService()
        {
            _bikes = new Dictionary<string, Bike>();
        }

        public BikeService(Dictionary<string, Bike> bikeStorage)
        {
            _bikes = bikeStorage;
        }

        public void AddBike(string id, decimal pricePerMinute)
        {
            if (pricePerMinute < BikeMinPrice)
                throw new NegativePriceException();
            
            if (string.IsNullOrEmpty(id))
                throw  new BikeIdNullException();

            if(_bikes.ContainsKey(id))
                throw new BikeIdExistsException();
            
            _bikes.Add(id, new Bike(id, pricePerMinute));
        }

        public void RemoveBike(string id)
        {
            if(ValidateId(id))
                _bikes.Remove(id);
        }

        public IList<Bike> GetBikes()
        {
            return _bikes.Select(b => b.Value).ToList();
        }

        public Bike GetBikeById(string id)
        {
            return ValidateId(id) ? _bikes[id] : null;
        }

        private bool ValidateId(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BikeIdNullException();

            if (!_bikes.ContainsKey(id))
                throw new BikeNotExistsException();

            return true;
        }
    }
}
