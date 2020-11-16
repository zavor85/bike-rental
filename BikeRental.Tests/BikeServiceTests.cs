using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BikeRental.Tests
{
    [TestClass]
    public class BikeServiceTests
    {
        private readonly IBikeService _bikeService;

        public BikeServiceTests()
        {
            _bikeService = new BikeService();
        }

        [TestMethod]
        public void AddBikeTest()
        {
            _bikeService.AddBike("1", 0.2m);
        }

        [TestMethod]
        public void GetBikeByIdTest()
        {
            _bikeService.AddBike("1", 0.2m);
            var bike = _bikeService.GetBikeById("1");
            Assert.AreEqual("1", bike.Id);
            Assert.AreEqual(0.2m, bike.PricePerMinute);
        }

        [TestMethod]
        public void AddBikeNegativePriceTest()
        {
            Assert.ThrowsException<NegativePriceException>(() => 
                _bikeService.AddBike("1", -0.2m));
        }

        [TestMethod]
        public void AddBikeNullTest()
        {
            Assert.ThrowsException<BikeIdNullException>(() => 
                _bikeService.AddBike(null, 0.2m));
        }

        [TestMethod]
        public void AddBikeUniqueIdTest()
        {
            _bikeService.AddBike("1", 0.2m);
            Assert.ThrowsException<BikeIdExistsException>(() =>
                _bikeService.AddBike("1", 0.2m));
        }

        [TestMethod]
        public void GetBikeByIdNullTest()
        {
            Assert.ThrowsException<BikeIdNullException>(() =>
                _bikeService.GetBikeById(null));
        }

        [TestMethod]
        public void GetBikeByIdNonExistingTest()
        {
            Assert.ThrowsException<BikeNotExistsException>(() =>
                _bikeService.GetBikeById("incorrectId"));
        }

        [TestMethod]
        public void RemoveBikeTest()
        {
            _bikeService.AddBike("1", 0.2m);
            _bikeService.RemoveBike("1");
            Assert.ThrowsException<BikeNotExistsException>(() =>
                _bikeService.GetBikeById("1"));
        }

        [TestMethod]
        public void RemoveNonExistingBikeTest()
        {
            Assert.ThrowsException<BikeNotExistsException>(() =>
                _bikeService.RemoveBike("incorrectBike"));
        }

        [TestMethod]
        public void RemoveBikeNullIdTest()
        {
            Assert.ThrowsException<BikeIdNullException>(() =>
                _bikeService.RemoveBike(null));
        }

        [TestMethod]
        public void GetBikesTest()
        {
            _bikeService.AddBike("1", 0.2m);
            Assert.AreEqual(1, _bikeService.GetBikes().Count);
        }
    }
}
