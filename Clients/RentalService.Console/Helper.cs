using System;
using RentalService.Tests;

namespace RentalService.Console
{
    internal class Helper
    {
        private NinjectHelper _ninjectHelper;

        public Helper()
        {
            _ninjectHelper = new NinjectHelper();
            _ninjectHelper.BindImplementations();
        }

        public void ListAllAvailableItems()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Current available vehicle types:");
            var vehicleTypes = _ninjectHelper.VehicleTypesRepo.GetAvailableVehicleTypes();
            foreach (var vehicleType in vehicleTypes)
            {
                System.Console.WriteLine("\"{0}\" from {1}", vehicleType.Name, vehicleType.GetType().ToString());
            }
        }

        public void RegisterSomeRentals()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Registering some rentals...");
            var rentalsTests = new RentalsTests();
            rentalsTests.MakeRentalRequest("Småbil", "ABC880", DateTime.Now, "19800101-7894", 5001, _ninjectHelper.RentalService);
            rentalsTests.MakeRentalRequest("Kombi", "APA900", DateTime.Now, "19700101-7880", 8001, _ninjectHelper.RentalService);
        }

        public void ReturnSomeVehicles()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Returning some vehicles...");
            var rentalsTests = new RentalsTests();
            rentalsTests.MakeRentalReturn("1", 5100, DateTime.Now.AddDays(1), _ninjectHelper.RentalService);
            rentalsTests.MakeRentalReturn("2", 8100, DateTime.Now.AddDays(2), _ninjectHelper.RentalService);
        }

        public void ListAllRentals()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Current rental entries in the system:");
            var rentals = _ninjectHelper.RentalService.GetAllRentals();
            foreach (var rental in rentals)
            {
                System.Console.WriteLine("RentalNumber: {0}, RegNo: {1}, Status: {2}, Type: {3}, RentalDate: {4}, ReturnDate: {5}, OrgKm: {6}, NewKm: {7}, PersNr: {8}",
                    rental.RentalNumber, rental.RegNo,
                    rental.Status, rental.VehicleTypeName, 
                    rental.RentalDate, rental.ReturnDate,
                    rental.OriginalMileageKm, rental.NewMileageKm,
                    rental.CustomerInfo.PersonNummer);
            }
        }
    }
}
