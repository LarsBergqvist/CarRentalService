using VehicleTypes.Contract;

namespace VehicleTypes
{
    public class Lastbil : IVehicleType
    {
        public string Name
        {
            get { return "Lastbil"; }
        }

        public double GetRentalCost(CostParameters costPar)
        {
            double price = costPar.DailyBaseCost * costPar.NumberOfRentalDays * 1.5 + costPar.MilageKmBaseCost * costPar.NumberOfMilageKm * 1.5;
            return price;
        }
    }
}
