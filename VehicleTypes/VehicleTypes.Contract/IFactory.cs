using System.Collections.Generic;

namespace VehicleTypes.Contract
{
    /// <summary>
    /// A factory for providing implementations of IVehicle objects
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Gets a VehicleType object with a specific name
        /// </summary>
        /// <param name="name">The name of the vehicle type object to get</param>
        /// <returns>An IVehicle implementation. Returns null if the vehicle type does not exist.</returns>
        IVehicleType GetVehicleType(string name);

        /// <summary>
        /// Gets a collection of all available vehicle types
        /// </summary>
        /// <returns>A collection of IVehicle implementations</returns>
        IEnumerable<IVehicleType> GetAllVehicleTypes();
    }
}