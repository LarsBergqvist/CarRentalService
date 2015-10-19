using System.Collections.Generic;
using VehicleTypes.Contract;

namespace RentalService.Contract
{
    /// <summary>
    /// Interface for a repository that provides a collection of available vehicle types
    /// </summary>
    public interface IVehicleTypesRepository
    {
        /// <summary>
        /// Returns a collection of available vehicle types
        /// </summary>
        /// <returns>A collection of IVehicle instances</returns>
        IEnumerable<IVehicleType> GetAvailableVehicleTypes();
    }
}
