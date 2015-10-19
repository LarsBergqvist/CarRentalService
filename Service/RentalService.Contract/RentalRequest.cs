using System;
using RentalsRepository.Contract;

namespace RentalService.Contract
{
    /// <summary>
    /// Represents the input for a rental request
    /// </summary>
    public class RentalRequest
    {
        /// <summary>
        /// The name of the vehicle type to request a rent for
        /// </summary>
        public string VehicleTypeName { get; set; }
        /// <summary>
        /// The registration number of the vehicle to rent
        /// </summary>
        public string RegNo { get; set; }
        /// <summary>
        /// The requested date and time for the rental
        /// </summary>
        public DateTime RentDateTime { get; set; }
        /// <summary>
        /// Contains information about the customer that wants to make a rental
        /// </summary>
        public CustomerInfo CustomerInfo { get; set; }
        /// <summary>
        /// The current milage in km of the vehicle to rent
        /// </summary>
        public double CurrentMilageKm { get; set; }
    }
}
