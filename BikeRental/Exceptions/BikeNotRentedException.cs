using System;

namespace BikeRental
{
    public class BikeNotRentedException :Exception
    {
        public BikeNotRentedException() : base("Bike not rented")
        {
        }
    }
}
