using System;
using System.Collections.Generic;

namespace RentalsRepository.Contract
{
    /// <summary>
    /// An interface for a repository that can store, update and retrieve rental information
    /// </summary>
    public interface IRentalsRepository
    {
        /// <summary>
        /// Adds a new rental registration in the repository
        /// </summary>
        /// <param name="vehicleTypeName">The type name of the vehicle that is rented</param>
        /// <param name="regNo">The registration number of the vehicle that is rented</param>
        /// <param name="rentalDate">The rental date and time of the vehicle</param>
        /// <param name="currentMileageKm">The milage of the vehicle when it was rented</param>
        /// <param name="customerInfo">The information about the customer that makes the rental</param>
        /// <returns></returns>
        RentalInfo AddNewRental(string vehicleTypeName, string regNo, DateTime rentalDate, double currentMileageKm, CustomerInfo customerInfo);
        /// <summary>
        /// Updates a rental registration as returned
        /// </summary>
        /// <param name="rentalNumber">The rental number of the registration</param>
        /// <param name="returnDate">The return date</param>
        /// <param name="newMileageKm">The new milage of the vehicle</param>
        /// <returns></returns>
        RentalInfo UpdateRentalAsReturned(string rentalNumber, DateTime returnDate, double newMileageKm);
        /// <summary>
        /// Returns a rental registration with a specific rental number
        /// </summary>
        /// <param name="rentalNumber"></param>
        /// <returns>A valid rental object or null if no match was found</returns>
        RentalInfo GetRental(string rentalNumber);
        /// <summary>
        /// Checks if a vehicle with a specified registration number is out for rent
        /// </summary>
        /// <param name="regNo">The registration number of the vehicle</param>
        /// <returns>true if the vehicle is out for rent, false if not</returns>
        bool VehicleIsOutForRent(string regNo);
        /// <summary>
        /// Gets a collection of all rental registrations in the repository
        /// </summary>
        /// <returns>A collection of rental registrations</returns>
        IEnumerable<RentalInfo> GetAllRentals();
        /// <summary>
        /// Removes all rental registration from the repository
        /// </summary>
        void RemoveAllRentals();
    }
}
