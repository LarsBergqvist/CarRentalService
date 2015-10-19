using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace VehicleTypes.Contract
{
    /// <summary>
    /// A factory for loading implementations of IVechileType dynamically
    /// All dll:s in the executing assembly's folder are examined for IVechileType implementations
    /// </summary>
    public class Factory
    {
        private bool _typesLoaded = false;
        private ICollection<IVehicleType> _vehicleTypes = new List<IVehicleType>();
 
        /// <summary>
        /// Gets a VehicleType object with a specific name
        /// </summary>
        /// <param name="name">The name of the vehicle type object to get</param>
        /// <returns>An IVehicle implementation. Returns null if the vehicle type does not exist.</returns>
        public IVehicleType GetVehicleType(string name)
        {
            if (!_typesLoaded)
            {
                LoadTypes();
            }

            return _vehicleTypes.ToList().Find(x => x.Name == name);
        }

        /// <summary>
        /// Gets a collection of all available vehicle types
        /// </summary>
        /// <returns>A collection of IVehicle implementations</returns>
        public IEnumerable<IVehicleType> GetAllVehicleTypes()
        {
            if (!_typesLoaded)
            {
                LoadTypes();
            }

            return _vehicleTypes;
        }

        private void LoadTypes()
        {
            var allAssemblies = new List<Assembly>();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                allAssemblies.Add(Assembly.LoadFile(dll));                
            }

            var type = typeof(IVehicleType);
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(a => a.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface);

            foreach (var t in types)
            {
                var vehicleType = (IVehicleType)Activator.CreateInstance(t);
                if (_vehicleTypes.ToList().Find(x => x.Name == vehicleType.Name) == null)
                {
                    _vehicleTypes.Add(vehicleType);
                }
            }

            _typesLoaded = true;
        }
    }
}
