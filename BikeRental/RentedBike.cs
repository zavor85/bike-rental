using System;

namespace BikeRental
{
    public class RentedBike : IRentedBike
    {
        public RentedBike(Bike bike, DateTime startTime)
        {
            Bike = bike;
            StartRent = startTime;
        }
        public DateTime StartRent { get; set; }
        public decimal Price { get; set; }
        public DateTime EndRent { get; set; }
        public Bike Bike { get; set; }

        public void EndOfRentedBike(DateTime endTime, decimal ridePrice)
        {
            EndRent = endTime;
            Price = ridePrice;
        }
    }
}
