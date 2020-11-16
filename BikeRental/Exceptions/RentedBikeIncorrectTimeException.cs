using System;

namespace BikeRental
{
    public class RentedBikeIncorrectTimeException : Exception
    {
        public RentedBikeIncorrectTimeException() : base("Rented bike incorrect time")
        {
        }
    }
}
