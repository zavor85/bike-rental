using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BikeRental.Tests
{
    [TestClass]
    public class IncomeCalculatorTests
    {
        private readonly IIncomeCalculator _incomeCalculator;
        private readonly decimal _bikeMaxPrice = 20;
        public IncomeCalculatorTests()
        {
            _incomeCalculator = new IncomeCalculator(_bikeMaxPrice);
        }

        [TestMethod]
        public void RentedBikeIncorrectTimeTest()
        {
            var rentStart = new DateTime(2020,1,2,0,0,0);
            var rentEnd = new DateTime(2020,1,1,0,0,0);
            Assert.ThrowsException<RentedBikeIncorrectTimeException>(() =>
                _incomeCalculator.BikeRentCalculatingIncome(rentStart, rentEnd, 0.2m));
        }

        [DataTestMethod]
        [DataRow("2020/01/01 12:00:00", "2020/01/01 12:10:00", 2)] 
        [DataRow("2020/01/01 12:00:00", "2020/01/01 14:00:00", 20)]
        [DataRow("2020/01/01 23:50:00", "2020/01/02 00:10:00", 4)] 
        [DataRow("2020/01/01 23:50:00", "2020/01/02 23:50:00", 20)] 
        [DataRow("2020/01/01 23:50:00", "2020/01/03 00:10:00", 24)] 
        [DataRow("2020/01/01 12:00:00", "2020/01/03 00:10:00", 42)]
        [DataRow("2020/01/01 23:50:00", "2020/01/03 03:00:00", 42)] 
        [DataRow("2020/01/01 12:00:00", "2020/01/03 12:00:00", 60)]
        [DataRow("2020/01/01 23:00:00", "2020/01/04 01:00:00", 64)]
        [DataRow("2020/01/01 22:00:00", "2020/01/04 02:00:00", 80)]
        [DataRow("2020/01/31 23:50:00", "2020/02/01 00:10:00", 4)]
        [DataRow("2019/12/31 23:50:00", "2020/01/01 00:10:00", 4)]
        [DataRow("2019/11/30 23:50:00", "2019/12/02 00:10:00", 24)]
        [DataRow("2019/12/31 23:50:00", "2020/01/02 00:10:00", 24)]
        public void BikesRentCalculatingIncomeTest(string startRent, string endRent, int resultPrice)
        {
            var income = _incomeCalculator.BikeRentCalculatingIncome(DateTime.Parse(startRent), DateTime.Parse(endRent), 0.2m);
            Assert.AreEqual(resultPrice, income);
    }

        [DataTestMethod]
        public void AllIncomesTest()
        {
            var firstRentedBike = new RentedBike(new Bike("1", 0.2m), new DateTime(2020,1,1));
            firstRentedBike.EndOfRentedBike(new DateTime(2020,1,1), 8m);
            var secondRentedBike = new RentedBike(new Bike("1", 0.2m), new DateTime(2020, 1, 1));
            secondRentedBike.EndOfRentedBike(new DateTime(2020, 1, 1), 13m);
            var thirdRentedBike = new RentedBike(new Bike("1", 0.2m), new DateTime(2020, 1, 1));
            thirdRentedBike.EndOfRentedBike(new DateTime(2020, 1, 1), 5m);
            var rentedBikesList = new List<RentedBike> { firstRentedBike, secondRentedBike, thirdRentedBike };

            var allIncomes = _incomeCalculator.AllIncomes(rentedBikesList);
            Assert.AreEqual(26, allIncomes);
        }
    }
}
