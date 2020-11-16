using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BikeRental.Tests
{
    [TestClass]
    public class RentalCompanyTests
    {
        private readonly IRentalCompany _rentalCompany;
        private readonly Mock<IBikeService> _bikeService;
        private readonly Mock<IIncomeCalculator> _incomeCalculator;
        private readonly Mock<IRentedBikeService> _rentedBikeService;
        private string _companyName = "Test Company";

        public RentalCompanyTests()
        {
            _bikeService = new Mock<IBikeService>();
            var bike = new Bike("1", 0.2M);
            _bikeService.Setup(bs => bs.GetBikeById("1")).Returns(bike);

            _incomeCalculator = new Mock<IIncomeCalculator>();
            _rentedBikeService = new Mock<IRentedBikeService>();
            _rentalCompany = new RentalCompany(_companyName, _bikeService.Object, _incomeCalculator.Object, _rentedBikeService.Object);

        }

        [TestMethod]
        public void RentalCompanyNameTest()
        {
            Assert.AreEqual(_companyName, _rentalCompany.Name);
        }

        [TestMethod]
        public void RentalCompanyNameNullTest()
        {
            Assert.ThrowsException<RentalCompanyNotNullException>(() =>
                 new RentalCompany(null, _bikeService.Object, _incomeCalculator.Object, _rentedBikeService.Object));
        }

        [TestMethod]
        public void RentalCompanyStartRentTest()
        {
            var bike = _bikeService.Object.GetBikeById("1");
            _rentalCompany.StartRent("1");
            _rentedBikeService.Verify(rbs => rbs.StartRentOfBike(bike), Times.Once);

        }

        [TestMethod]
        public void RentBikeWithoutIdTest()
        {
            Assert.ThrowsException<BikeIdNullException>(() => _rentalCompany.StartRent(null));
        }

        [TestMethod]
        public void EndBikeRentTest()
        {
            _rentalCompany.EndRent("1");
            _rentedBikeService.Verify(rbs => rbs.EndRentOfBike("1"), Times.Once);
        }

        [TestMethod]
        public void EndRentGetPriceTest()
        {
            _rentedBikeService.Setup(rbs => rbs.EndRentOfBike("1")).Returns(5m);

            var income =_rentalCompany.EndRent("1");
            Assert.AreEqual(5m, income);
        }

        [DataTestMethod]
        [DataRow(2020, "13.5", "28.5", true)]
        [DataRow(2020, "13.5", "13.5", false)]
        [DataRow(2019, "4", "4", true)]
        [DataRow(2019, "4", "4", false)]
        [DataRow(2018, "35", "40", true)]
        [DataRow(2018, "35", "35", false)]
        [DataRow(null, "52.5", "72.5", true)]
        [DataRow(null, "52.5", "52.5", false)]
        public void CalculateIncomeTest(int? year, string notIncludedNotCompletedRentals, string total,
            bool included)
        {
            var rentedBikeHistory = year.HasValue
                ? GetRentedBikesHistory().Where(rb => rb.EndRent.Year == year).ToList()
                : GetRentedBikesHistory();

            RentedBikeServiceHelpMethod(rentedBikeHistory, year);

            CalculatorHelpMethod(rentedBikeHistory, Convert.ToDecimal(notIncludedNotCompletedRentals));

            var finalResult = _rentalCompany.CalculateIncome(year, included);

            Assert.AreEqual(Convert.ToDecimal(total), finalResult);
        }

        private void CalculatorHelpMethod(List<RentedBike> rentedBikesHistory, decimal result)
        {
            _incomeCalculator.Setup(ic => ic.AllIncomes(rentedBikesHistory)).Returns(result);
            _incomeCalculator.Setup(ic =>
                ic.BikeRentCalculatingIncome(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0.2M)).Returns(5);
        }

        private void RentedBikeServiceHelpMethod(List<RentedBike> rentedBikesHistory, int? year)
        {
            _rentedBikeService.Setup(rb => rb.AllRentedBikesHistory(year)).Returns(rentedBikesHistory);

            var count = year.HasValue
                ? GetActiveRentedBikes().Values.Count(rb => rb.StartRent.Year == year)
                : GetActiveRentedBikes().Count;
            var result = count * 5m;

            _rentedBikeService.Setup(rb => rb.AllActiveRentedBikesIncomes(It.IsAny<int?>())).Returns(result);
        }

        private Dictionary<string, RentedBike> GetActiveRentedBikes()
        {
            return new Dictionary<string, RentedBike>
            {
                {"7", new RentedBike(new Bike("7", 0.2M), new DateTime(2020, 8, 1))},
                {"8", new RentedBike(new Bike("8", 0.2M), new DateTime(2020, 8, 1))},
                {"9", new RentedBike(new Bike("9", 0.2M), new DateTime(2020, 8, 1))},
                {"10", new RentedBike(new Bike("10", 0.2M), new DateTime(2018, 8, 1))}
            };
        }

        private List<RentedBike> GetRentedBikesHistory()
        {
            var rentedBike1 = new RentedBike(new Bike("1", 0.2M), new DateTime(2019, 1, 1));
            rentedBike1.EndOfRentedBike(new DateTime(2019, 1, 1), 1.5M);
            var rentedBike2 = new RentedBike(new Bike("2", 0.2M), new DateTime(2019, 1, 1));
            rentedBike2.EndOfRentedBike(new DateTime(2019, 1, 1), 2.5M);
            var rentedBike3 = new RentedBike(new Bike("3", 0.2M), new DateTime(2020, 1, 1)); 
            rentedBike3.EndOfRentedBike(new DateTime(2020, 1, 1), 3.5M);
            var rentedBike4 = new RentedBike(new Bike("4", 0.2M), new DateTime(2020, 1, 1));
            rentedBike4.EndOfRentedBike(new DateTime(2020, 1, 1), 10M);
            var rentedBike5 = new RentedBike(new Bike("5", 0.2M), new DateTime(2018, 1, 1));
            rentedBike5.EndOfRentedBike(new DateTime(2018, 1, 1), 20M);
            var rentedBike6 = new RentedBike(new Bike("6", 0.2M), new DateTime(2018, 1, 1));
            rentedBike6.EndOfRentedBike(new DateTime(2018, 1, 1), 15M);
            return new List<RentedBike> { rentedBike1, rentedBike2, rentedBike3, rentedBike4, rentedBike5, rentedBike6 };
        }
    }
}
