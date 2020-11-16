using System;

namespace BikeRental
{
    public class BikeAlreadyRentedException : Exception
    {
        public BikeAlreadyRentedException() : base("Bike is already rented")
        {
        }
    }
}
