namespace VehicleTypes.Contract
{
    /// <summary>
    /// The input parameters for the rental cost calulation
    /// </summary>
    public struct CostParameters
    {
        /// <summary>
        /// The base rental cost for one day
        /// </summary>
        public double DailyBaseCost { get; set; }
        /// <summary>
        /// The base rental cost per km
        /// </summary>
        public double MilageKmBaseCost { get; set; }
        /// <summary>
        /// Number of rental days
        /// </summary>
        public int NumberOfRentalDays { get; set; }
        /// <summary>
        /// Number of km used during the rental
        /// </summary>
        public double NumberOfMilageKm { get; set; }
    }
}
