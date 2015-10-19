using VehicleTypes.Contract;

namespace VehicleTypes
{
    public class Kombi : IVehicleType
    {
        public string Name
        {
            get { return "Kombi"; }
        }

        public double GetRentalCost(CostParameters costPar)
        {
            double price = costPar.DailyBaseCost * costPar.NumberOfRentalDays * 1.3 + costPar.MilageKmBaseCost * costPar.NumberOfMilageKm;
            return price;
        }
    }
}
