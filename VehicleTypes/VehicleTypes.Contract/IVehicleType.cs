namespace VehicleTypes.Contract
{
    /// <summary>
    /// Represents a vehicle type with custom rental cost calculation
    /// </summary>
    public interface IVehicleType
    {
        /// <summary>
        /// The name of the vehicle type
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Calculates the cost of a rental for this vehicle type
        /// </summary>
        /// <param name="costParameters">The input parameters for the calculation</param>
        /// <returns>The rental cost for this vehicle type</returns>
        double GetRentalCost(CostParameters costParameters);
    }
}
