using System;

namespace RentalService.Contract
{
    /// <summary>
    /// Represents the input for a rental return
    /// </summary>
    public class ReturnRequest
    {
        /// <summary>
        /// The rental number of the vehicle to return
        /// </summary>
        public string RentalNumber { get; set; }
        /// <summary>
        /// The date and time of vehicle return
        /// </summary>
        public DateTime ReturnDateTime { get; set; }
        /// <summary>
        /// The current milage of the vehicle
        /// </summary>
        public double CurrentMilageKm { get; set; }
    }
}
