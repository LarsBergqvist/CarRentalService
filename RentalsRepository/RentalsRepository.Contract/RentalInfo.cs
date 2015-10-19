using System;

namespace RentalsRepository.Contract
{
    /// <summary>
    /// Represents a rental registration in the repository
    /// </summary>
    public class RentalInfo
    {
        /// <summary>
        /// Defines the state of the rental
        /// </summary>
        public enum ERentStatus
        {
            /// <summary>
            /// The vehicle is out for rent
            /// </summary>
            Rented,
            /// <summary>
            /// The vehicle has been returned
            /// </summary>
            Returned
        }

        /// <summary>
        /// The unique rental number for the registration in the repository
        /// </summary>
        public string RentalNumber { get; set; }
        /// <summary>
        /// The vehicle type name of the rental
        /// </summary>
        public string VehicleTypeName { get; set; }
        /// <summary>
        /// The registration number of the vehicle
        /// </summary>
        public string RegNo { get; set; }
        /// <summary>
        /// Information about the customer that made the rental
        /// </summary>
        public CustomerInfo CustomerInfo { get; set; }
        /// <summary>
        /// The status of the rental
        /// </summary>
        public ERentStatus Status { get; set; }
        /// <summary>
        /// The date and time when the vehicle was delivered to the customer
        /// </summary>
        public DateTime RentalDate { get; set; }
        /// <summary>
        /// The date and time when the vehicle was returned
        /// </summary>
        public DateTime? ReturnDate { get; set; }
        /// <summary>
        /// The original milage of the vehicle when it was delivered to the customer
        /// </summary>
        public double OriginalMileageKm { get; set; }
        /// <summary>
        /// The new milage of the vehicle when it was returned
        /// </summary>
        public double? NewMileageKm { get; set; }
    }

    /// <summary>
    /// Represents information about the customer that rented the vehicle
    /// </summary>
    public class CustomerInfo
    {
        /// <summary>
        /// Personnummer of the customer that rented the vehicle
        /// </summary>
        public string PersonNummer { get; set; }
    }
}
