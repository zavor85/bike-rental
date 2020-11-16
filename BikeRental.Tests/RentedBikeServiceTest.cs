using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BikeRental.Tests
{
    [TestClass]
    public class RentedBikeServiceTest
    {
        private IRentedBikeService _rentedBikeService;
        private readonly Mock<IIncomeCalculator> _calculator;

        public RentedBikeServiceTest()
        {
            _calculator = new Mock<IIncomeCalculator>();
            _calculator.Setup(ic => ic.BikeRentCalculatingIncome(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0.2M)).Returns(5);
            _rentedBikeService = new RentedBikeService(_calculator.Object);
        }

        [TestMethod]
        public void StartRentOfBikeTest()
        {
            Bike bike = new Bike("1", 0.2M);
            _rentedBikeService.StartRentOfBike(bike);
            Assert.IsTrue(bike.IsRented);
        }

        [TestMethod]
        public void StartRentOfBikeWhichAlreadyRentedTest()
        {
            Bike bike = new Bike("1", 0.2M);

            _rentedBikeService.StartRentOfBike(bike);

            Assert.ThrowsException<BikeAlreadyRentedException>(() =>
                _rentedBikeService.StartRentOfBike(bike));
        }

        [TestMethod]
        public void EndRentOfBikeTest()
        {
            var bike = new Bike("1", 0.2M);
            var rentedBike = new RentedBike(bike, DateTime.Now);
            _rentedBikeService.StartRentOfBike(bike);

            _rentedBikeService.EndRentOfBike(rentedBike.Bike.Id);

            Assert.IsFalse(bike.IsRented);
        }

        [TestMethod]
        public void EndRentOfBikeWhichDidNotStartRentTest()
        {
            var bike = new Bike("1", 0.2m);
            var rentedBike = new RentedBike(bike, DateTime.Now);

            Assert.ThrowsException<BikeNotRentedException>(() =>
                _rentedBikeService.EndRentOfBike(rentedBike.Bike.Id));
        }

        [TestMethod]
        public void AllRentedBikesHistoryTest()
        {
            _rentedBikeService.StartRentOfBike(new Bike("1", 0.2m));
            _rentedBikeService.EndRentOfBike("1");

            var listOfRentedBikes = _rentedBikeService.AllRentedBikesHistory(null);

            Assert.AreEqual(1, listOfRentedBikes.Count);
        }

        [DataTestMethod]
        [DataRow(null, 75)]
        [DataRow(2017, 10)]
        [DataRow(2018, 15)]
        [DataRow(2019, 20)]
        [DataRow(2020, 30)]
        public void AllActiveRentedBikesIncomesTest(int? year, int expected)
        {
            _rentedBikeService = new RentedBikeService(_calculator.Object, AllActiveBikes(), null);

            var incomes = _rentedBikeService.AllActiveRentedBikesIncomes(year);
            Assert.AreEqual(expected, incomes);
        }

        private Dictionary<string, RentedBike> AllActiveBikes()
        {
            return new Dictionary<string, RentedBike>
            {
                {"11", new RentedBike(new Bike("11", 0.2M), new DateTime(2020, 1, 1))},
                {"15", new RentedBike(new Bike("15", 0.2M), new DateTime(2020, 1, 1))},
                {"18", new RentedBike(new Bike("18", 0.2M), new DateTime(2020, 1, 1))},
                {"22", new RentedBike(new Bike("22", 0.2M), new DateTime(2020, 1, 1))},
                {"26", new RentedBike(new Bike("26", 0.2M), new DateTime(2020, 1, 1))},
                {"29", new RentedBike(new Bike("29", 0.2M), new DateTime(2020, 1, 1))},
                {"44", new RentedBike(new Bike("44", 0.2M), new DateTime(2019, 1, 1))},
                {"45", new RentedBike(new Bike("45", 0.2M), new DateTime(2019, 1, 1))},
                {"46", new RentedBike(new Bike("46", 0.2M), new DateTime(2019, 1, 1))},
                {"47", new RentedBike(new Bike("47", 0.2M), new DateTime(2019, 1, 1))},
                {"65", new RentedBike(new Bike("65", 0.2M), new DateTime(2018, 1, 1))},
                {"70", new RentedBike(new Bike("70", 0.2M), new DateTime(2018, 1, 1))},
                {"87", new RentedBike(new Bike("87", 0.2M), new DateTime(2018, 1, 1))},
                {"96", new RentedBike(new Bike("96", 0.2M), new DateTime(2017, 1, 1))},
                {"99", new RentedBike(new Bike("99", 0.2M), new DateTime(2017, 1, 1))}
                };
        }
    }
}
