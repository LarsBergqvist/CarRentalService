using VehicleTypes.Contract;

namespace VehicleTypes
{
    public class Småbil : IVehicleType
    {
        public string Name
        {
            get { return "Småbil"; }
        }

        public double GetRentalCost(CostParameters costPar)
        {
            double price = costPar.DailyBaseCost * costPar.NumberOfRentalDays;
            return price;
        }
    }
}
