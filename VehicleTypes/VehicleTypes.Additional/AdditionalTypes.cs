using VehicleTypes.Contract;

namespace VehicleTypes.Additional
{
    public class Hummer : IVehicleType
    {
        public string Name
        {
            get { return "Hummer"; }
        }

        public double GetRentalCost(CostParameters costPar)
        {
            double price = costPar.DailyBaseCost * costPar.NumberOfRentalDays * 10 + costPar.MilageKmBaseCost * costPar.NumberOfMilageKm * 12;
            return price;
        }
    }

    public class MonsterTruck : IVehicleType
    {
        public string Name
        {
            get { return "MonsterTruck"; }
        }

        public double GetRentalCost(CostParameters costPar)
        {
            double price = costPar.DailyBaseCost * costPar.NumberOfRentalDays * 15 + costPar.MilageKmBaseCost * costPar.NumberOfMilageKm * 18;
            return price;
        }
    }
}
