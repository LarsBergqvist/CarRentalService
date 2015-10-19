using System;
using System.Linq;
using NUnit.Framework;
using RentalsRepository.Contract;
using RentalService.Contract;

namespace RentalService.Tests
{
    [TestFixture]
    public class RentalsTests
    {
        private const double _dailyBaseCost = 100;
        private const double _milageKmBaseCost = 10;

        private NinjectHelper _ninjectHelper;
        private IRentalService RentalService
        {
            get { return _ninjectHelper.RentalService; }
        }

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _ninjectHelper = new NinjectHelper();
            _ninjectHelper.BindImplementations();
        }

        [SetUp]
        public void SetUpTest()
        {
            _ninjectHelper.RentalService.RemoveAllRentals();
        }

        [Test]
        public void Test_rentals_are_persisted()
        {
            var rentals = RentalService.GetAllRentals();
            Assert.IsTrue(rentals.ToList().Count == 0);

            var requestReceipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "19700101-5212", 1000, RentalService);
            rentals = RentalService.GetAllRentals();
            Assert.IsTrue(rentals.ToList().Count == 1);

            requestReceipt = MakeRentalRequest("Kombi", "ABC124", DateTime.Now, "19700101-5212", 1000, RentalService);
            rentals = RentalService.GetAllRentals();
            Assert.IsTrue(rentals.ToList().Count == 2);
        }

        [Test]
        public void Test_make_return_with_invalid_rental_number()
        {
            var returnRequest = new ReturnRequest()
            {
                RentalNumber = "ABGSDFKN"
            };

            // Make a rental return
            var returnReceipt = RentalService.ReturnVehicle(returnRequest);
            Assert.IsTrue(returnReceipt.Status == ERentalReturnStatus.NotOk);
            Assert.IsTrue(returnReceipt.Message == string.Format("Rental with number {0} does not exist in the system.", returnRequest.RentalNumber));
        }

        [Test]
        public void Test_rent_with_invalid_vehicle_type()
        {
            // Rental request data
            const double currentMilage = 10001;
            var rentalDay = new DateTime(2015, 10, 14);
            const string vehicleTypeToRent = "non existing type";

            // Make a rental request
            var rentalRequest = new RentalRequest()
            {
                RentDateTime = rentalDay,
                CurrentMilageKm = currentMilage,
                CustomerInfo = new CustomerInfo() { PersonNummer = "1980-01-01" },
                VehicleTypeName = vehicleTypeToRent
            };
            var rentalReceipt = RentalService.RentVehicle(rentalRequest);
            Assert.IsTrue(rentalReceipt.Status == ERentalRequestStatus.NotOk);

            Assert.IsTrue(rentalReceipt.Message ==
                string.Format("The vehicle type {0} does not exist.", vehicleTypeToRent));
        }

        [Test]
        public void Test_rent_with_empty_regno()
        {
            var receipt = MakeRentalRequest("Småbil", "", DateTime.Now, "19700101-5212", 5000, RentalService);
            Assert.IsTrue(receipt.Status == ERentalRequestStatus.NotOk);
        }

        [Test]
        public void Test_rent_with_empty_personnummer()
        {
            var receipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "", 5000, RentalService);
            Assert.IsTrue(receipt.Status == ERentalRequestStatus.NotOk);
        }

        [Test]
        public void Test_rent_with_invalid_milage()
        {
            var receipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "19700101-5212", -10, RentalService);
            Assert.IsTrue(receipt.Status == ERentalRequestStatus.NotOk);
            Assert.IsTrue(receipt.Message == "Milage can not be < 0 km.");
        }

        [Test]
        public void Test_return_with_invalid_date()
        {
            var requestReceipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "19700101-5212", 1000, RentalService);
            Assert.IsTrue(requestReceipt.Status == ERentalRequestStatus.Ok);
            var returnReceipt = MakeRentalReturn(requestReceipt.RentalNumber, 2000, DateTime.Now.AddDays(-2), RentalService);
            Assert.IsTrue(returnReceipt.Status == ERentalReturnStatus.NotOk);
            Assert.IsTrue(returnReceipt.Message == "Return date can not be before rental date.");
        }

        [Test]
        public void Test_return_with_invalid_milage()
        {
            var requestReceipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "19700101-5212", 1000, RentalService);
            Assert.IsTrue(requestReceipt.Status == ERentalRequestStatus.Ok);
            var returnReceipt = MakeRentalReturn(requestReceipt.RentalNumber, 500, DateTime.Now.AddDays(2), RentalService);
            Assert.IsTrue(returnReceipt.Status == ERentalReturnStatus.NotOk);
            Assert.IsTrue(returnReceipt.Message == "Milage on returned vehicle can not be less than original milage.");
        }

        [Test]
        public void Test_rent_vehicle_that_is_already_rented()
        {
            var requestReceipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "19700101-5212", 1000, RentalService);
            Assert.IsTrue(requestReceipt.Status == ERentalRequestStatus.Ok);
            requestReceipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "19700101-5212", 1000, RentalService);
            Assert.IsTrue(requestReceipt.Status == ERentalRequestStatus.NotOk);
            Assert.IsTrue(requestReceipt.Message == "Vehicle with RegNo ABC123 can not be rented as it is already out for rent.");
        }

        [Test]
        public void Test_rent_vehicle_return_and_rent_again()
        {
            var requestReceipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "19700101-5212", 1000, RentalService);
            Assert.IsTrue(requestReceipt.Status == ERentalRequestStatus.Ok);

            var returnReceipt = MakeRentalReturn(requestReceipt.RentalNumber, 1500, DateTime.Now.AddDays(2), RentalService);
            Assert.IsTrue(returnReceipt.Status == ERentalReturnStatus.Ok);

            requestReceipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now.AddDays(3), "19700101-5212", 1500, RentalService);
            Assert.IsTrue(requestReceipt.Status == ERentalRequestStatus.Ok);
        }

        [Test]
        public void Test_rent_vehicle_return_twice()
        {
            var requestReceipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "19700101-5212", 1000, RentalService);
            Assert.IsTrue(requestReceipt.Status == ERentalRequestStatus.Ok);

            var returnReceipt = MakeRentalReturn(requestReceipt.RentalNumber, 1500, DateTime.Now.AddDays(2), RentalService);
            Assert.IsTrue(returnReceipt.Status == ERentalReturnStatus.Ok);

            returnReceipt = MakeRentalReturn(requestReceipt.RentalNumber, 1500, DateTime.Now.AddDays(2), RentalService);
            Assert.IsTrue(returnReceipt.Status == ERentalReturnStatus.NotOk);
            Assert.IsTrue(returnReceipt.Message == "The vehicle is already registred as returned.");
        }

        [Test]
        public void Test_get_price_before_rental_return()
        {
            var requestReceipt = MakeRentalRequest("Småbil", "ABC123", DateTime.Now, "19700101-5212", 1000, RentalService);
            Assert.IsTrue(requestReceipt.Status == ERentalRequestStatus.Ok);
            double cost = RentalService.GetPriceForRental(requestReceipt.RentalNumber, 10, 10);
            // There should be no cost as the vehicle has not yet been returned
            Assert.IsTrue(cost == 0.0);
        }

        [Test]
        public void Test_get_price_Småbil()
        {
            double rentalCost = GetRentalCost("Småbil", "ABC123", 2, 100, RentalService);
            Assert.IsTrue(rentalCost == 200.0);
            rentalCost = GetRentalCost("Småbil", "ABC123", 2, 150, RentalService);
            Assert.IsTrue(rentalCost == 200.0);
            rentalCost = GetRentalCost("Småbil", "ABC123", 3, 150, RentalService);
            Assert.IsTrue(rentalCost == 300.0);
        }

        [Test]
        public void Test_get_price_Kombi()
        {
            double rentalCost = GetRentalCost("Kombi", "EFG456", 2, 100, RentalService);
            Assert.IsTrue(rentalCost == 1260.0);
            rentalCost = GetRentalCost("Kombi", "EFG456", 2, 150, RentalService);
            Assert.IsTrue(rentalCost == 1760.0);
            rentalCost = GetRentalCost("Kombi", "EFG456", 3, 150, RentalService);
            Assert.IsTrue(rentalCost == 1890.0);
        }

        [Test]
        public void Test_get_price_Lastbil()
        {
            double rentalCost = GetRentalCost("Lastbil", "HIJ789", 2, 100, RentalService);
            Assert.IsTrue(rentalCost == 1800.0);
            rentalCost = GetRentalCost("Lastbil", "HIJ789", 2, 150, RentalService);
            Assert.IsTrue(rentalCost == 2550.0);
            rentalCost = GetRentalCost("Lastbil", "HIJ789", 3, 150, RentalService);
            Assert.IsTrue(rentalCost == 2700.0);
        }

        [Test]
        public void Test_get_price_rental_time_rounded_up()
        {
            // Started days should be calculated as whole days
            // For example for 1.5 days of rent, one has to pay for 2 days of rent
            double rentalCost = GetRentalCost("Småbil", "ABC123", 1.5, 100, RentalService);
            Assert.IsTrue(rentalCost == 200.0);
            rentalCost = GetRentalCost("Småbil", "ABC123", 1.1, 100, RentalService);
            Assert.IsTrue(rentalCost == 200.0);
            rentalCost = GetRentalCost("Småbil", "ABC123", 1.01, 100, RentalService);
            Assert.IsTrue(rentalCost == 200.0);
            rentalCost = GetRentalCost("Småbil", "ABC123", 1.99, 100, RentalService);
            Assert.IsTrue(rentalCost == 200.0);
        }

        public RentalReceipt MakeRentalRequest(string vehicleTypeName, string regNo, DateTime rentalDay, string personnummer, double currentMilage, IRentalService rentalService)
        {
            // Rental request data
            string vehicleTypeToRent = vehicleTypeName;

            var rentalRequest = new RentalRequest()
            {
                RentDateTime = rentalDay,
                CurrentMilageKm = currentMilage,
                CustomerInfo = new CustomerInfo() { PersonNummer = personnummer },
                VehicleTypeName = vehicleTypeToRent,
                RegNo = regNo
            };
            var rentalReceipt = rentalService.RentVehicle(rentalRequest);
            return rentalReceipt;
        }

        public ReturnReceipt MakeRentalReturn(string rentalNumber, double currentMilage, DateTime returnDateTime, IRentalService rentalService)
        {
            var returnRequest = new ReturnRequest()
            {
                CurrentMilageKm = currentMilage,
                RentalNumber = rentalNumber,
                ReturnDateTime = returnDateTime,
            };

            var returnReceipt = rentalService.ReturnVehicle(returnRequest);

            return returnReceipt;
        }

        private double GetRentalCost(string vehicleTypeName, string regNo, double numberOfDays, double numberOfKm, IRentalService rentalService)
        {
            //
            // Make a rental request and check that registred
            //
            const double currentMilage = 10001;
            var rentalDay = new DateTime(2015, 10, 14);
            var rentalReceipt = MakeRentalRequest(vehicleTypeName, regNo, rentalDay, "19800101-8211", currentMilage, rentalService);
            Assert.IsTrue(rentalReceipt.Status == ERentalRequestStatus.Ok);
            var rentalInfo = rentalService.GetRental(rentalReceipt.RentalNumber);
            Assert.IsTrue(rentalInfo.Status == RentalInfo.ERentStatus.Rented);

            //
            // Make a rental return and check that registred
            //
            var returnReceipt = MakeRentalReturn(rentalReceipt.RentalNumber, currentMilage + numberOfKm, rentalDay.AddDays(numberOfDays), rentalService);
            Assert.IsTrue(returnReceipt.Status == ERentalReturnStatus.Ok);
            rentalInfo = rentalService.GetRental(rentalReceipt.RentalNumber);
            Assert.IsTrue(rentalInfo.Status == RentalInfo.ERentStatus.Returned);

            var rentalToPay = rentalService.GetPriceForRental(rentalReceipt.RentalNumber, _dailyBaseCost, _milageKmBaseCost);

            return rentalToPay;
        }

    }

}
