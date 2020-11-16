using System;

namespace BikeRental
{
    public class NegativePriceException : Exception
    {
        public NegativePriceException() : base("Price per minute can't be negative")
        {
        }
    }
}
