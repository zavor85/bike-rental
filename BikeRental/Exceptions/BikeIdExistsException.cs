using System;

namespace BikeRental
{
    public class BikeIdExistsException : Exception
    {
        public BikeIdExistsException() : base("Bike Id already exists")
        {
        }
    }
}
