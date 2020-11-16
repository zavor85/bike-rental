using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BikeRental.Tests
{
    [TestClass]
    public class RentedBikeTest
    {
        [TestMethod]
        public void EndOfRentTest()
        {
            var rentedBike = new RentedBike(new Bike("1", 0.2m), new DateTime(2020,1, 1, 0,0,0));
            rentedBike.EndOfRentedBike(new DateTime(2020,1,1,1,0,0), 12m);
            Assert.AreEqual(rentedBike.Price, 12m);
        }
    }
}
