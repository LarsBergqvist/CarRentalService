namespace RentalService.Console
{
    class Program
    {
        static void Main()
        {
            var helper = new Helper();
            helper.ListAllAvailableItems();

            helper.RegisterSomeRentals();
            helper.ListAllRentals();
            helper.ReturnSomeVehicles();
            helper.ListAllRentals();

            System.Console.ReadKey();

        }
    }
}
