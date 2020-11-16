using System;

namespace BikeRental
{
    public class RentalCompanyNotNullException : Exception
    {
        public RentalCompanyNotNullException() : base("Company Name can't be null or empty")
        {
        }
    }
}
