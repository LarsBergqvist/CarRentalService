using System.Linq;
using NUnit.Framework;

namespace RentalService.Tests
{
    [TestFixture]
    public class VehicleTypesTests
    {
        private NinjectHelper _ninjectHelper;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _ninjectHelper = new NinjectHelper();
            _ninjectHelper.BindImplementations();
        }

        [Test]
        public void Test_GetAvailableVehicleTypes()
        {
            var vehicleTypes = _ninjectHelper.VehicleTypesRepo.GetAvailableVehicleTypes();
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "Hummer"));
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "MonsterTruck"));
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "Småbil"));
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "Kombi"));
            Assert.IsNotNull(vehicleTypes.ToList().Find(x => x.Name == "Lastbil"));
        }
    }
}
