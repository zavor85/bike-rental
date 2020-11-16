using System;

namespace BikeRental
{
    public class BikeIdNullException : Exception
    {
        public BikeIdNullException() : base("Bike id can't be null")
        {
        }
    }
}
