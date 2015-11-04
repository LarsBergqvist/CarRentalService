using System;

namespace RentalService.Console
{
    class Program
    {
        static void Main()
        {
            var tests = new Tests();
            tests.ListAllAvailableItems();

            DateTime rentalDateTime = DateTime.Now;
            tests.RegisterSomeRentals(rentalDateTime);
            tests.ListAllRentals();
            tests.ReturnSomeVehicles(rentalDateTime.AddDays(2));
            tests.ListAllRentals();
            tests.GetCostForReturnedVehicles();

            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to exit");
            System.Console.ReadKey();

        }
    }
}
