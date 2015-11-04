using System;
using RentalService.Tests;

namespace RentalService.Console
{
    internal class Tests
    {
        private NinjectHelper _ninjectHelper;

        public Tests()
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

        public void RegisterSomeRentals(DateTime date)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Registering some rentals...");
            var rentalsTests = new RentalsTests();
            rentalsTests.MakeRentalRequest("Småbil", "ABC880", date, "19800101-7894", 5001, _ninjectHelper.RentalService);
            rentalsTests.MakeRentalRequest("Kombi", "APA900", date, "19700101-7880", 8001, _ninjectHelper.RentalService);
            rentalsTests.MakeRentalRequest("Lastbil", "JOS131", date, "19720101-9012", 9001, _ninjectHelper.RentalService);
        }

        public void ReturnSomeVehicles(DateTime date)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Returning some vehicles...");
            var rentalsTests = new RentalsTests();

            rentalsTests.MakeRentalReturn("1", 5100, date, _ninjectHelper.RentalService);

            rentalsTests.MakeRentalReturn("2", 8100, date, _ninjectHelper.RentalService);

            rentalsTests.MakeRentalReturn("3", 9100, date, _ninjectHelper.RentalService);
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

        public void GetCostForReturnedVehicles()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Cost for returned vehicles:");

            const double basDygnsHyra = 100;
            const double basKmPris = 10;

            var rentalToPaySmåbil = _ninjectHelper.RentalService.GetPriceForRental("1", basDygnsHyra, basKmPris);
            System.Console.WriteLine(String.Format("Cost for rental item 1:{0}", rentalToPaySmåbil));

            var rentalToPayKombi = _ninjectHelper.RentalService.GetPriceForRental("2", basDygnsHyra, basKmPris);
            System.Console.WriteLine(String.Format("Cost for rental item 2:{0}", rentalToPayKombi));

            var rentalToPayLastbil = _ninjectHelper.RentalService.GetPriceForRental("3", basDygnsHyra, basKmPris);
            System.Console.WriteLine(String.Format("Cost for rental item 3:{0}", rentalToPayLastbil));
        }
    }
}
