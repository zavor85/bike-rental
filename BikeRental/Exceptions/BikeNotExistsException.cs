using System;

namespace BikeRental
{
    public class BikeNotExistsException : Exception
    {
        public BikeNotExistsException() : base("Bike does not exist")
        {
        }
    }
}
