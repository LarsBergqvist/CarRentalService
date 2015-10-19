using System.Linq;
using NUnit.Framework;
using VehicleTypes.Contract;

namespace VehicleTypes.Tests
{
    [TestFixture]
    public class VehicleTypesTests
    {
        [Test]
        public void Test_GetAllVehicleTypes()
        {
            var factory = new Factory();
            var vehicleTypes = factory.GetAllVehicleTypes();
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "Hummer"));
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "MonsterTruck"));
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "Småbil"));
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "Kombi"));
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "Lastbil"));
        }

        [Test]
        public void Test_rental_cost_per_type()
        {
            var factory = new Factory();
            var costPar = new CostParameters()
            {
                DailyBaseCost = 500,
                MilageKmBaseCost = 5,
                NumberOfMilageKm = 100,
                NumberOfRentalDays = 3
            };

            Assert.IsTrue(factory.GetVehicleType("Hummer").GetRentalCost(costPar) == 21000.0);
            Assert.IsTrue(factory.GetVehicleType("MonsterTruck").GetRentalCost(costPar) == 31500.0);
            Assert.IsTrue(factory.GetVehicleType("Småbil").GetRentalCost(costPar) == 1500.0);
            Assert.IsTrue(factory.GetVehicleType("Kombi").GetRentalCost(costPar) == 2450.0);
            Assert.IsTrue(factory.GetVehicleType("Lastbil").GetRentalCost(costPar) == 3000.0);
        }
    }
}
