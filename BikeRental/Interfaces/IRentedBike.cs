using System;

namespace BikeRental
{
    public interface IRentedBike
    {
        DateTime EndRent { get; set; }
        decimal Price { get; set; }
        Bike Bike { get; set; }
        DateTime StartRent { get; set; }
    }
}