using System.Collections.Generic;
using RentalsRepository.Contract;

namespace RentalService.Contract
{
    /// <summary>
    /// Interface for a service that registers rentals and rental returns
    /// </summary>
    public interface IRentalService
    {
        /// <summary>
        /// Does a rental request in the system
        /// </summary>
        /// <param name="rentalRequest">The input needed for a rental request</param>
        /// <returns>A receipt that states if the rental request was accepted and registred</returns>
        RentalReceipt RentVehicle(RentalRequest rentalRequest);
        /// <summary>
        /// Does a rental return in the system
        /// </summary>
        /// <param name="returnRequest">The input needed for a rental return</param>
        /// <returns>A receipt that states if the rental return was accepted and registred</returns>
        ReturnReceipt ReturnVehicle(ReturnRequest returnRequest);
        /// <summary>
        /// Gets the price for a registred rental
        /// Note: started days are calculated as whole days!
        /// </summary>
        /// <param name="rentalNumber">The rental number in the system</param>
        /// <param name="dailyBaseCost">The base cost per day</param>
        /// <param name="milageKmBaseCost">The base cost per km</param>
        /// <returns></returns>
        double GetPriceForRental(string rentalNumber, double dailyBaseCost, double milageKmBaseCost);
        /// <summary>
        /// Gets all registred rentals in the system
        /// </summary>
        /// <returns>A collection of rental registrations</returns>
        IEnumerable<RentalInfo> GetAllRentals();
        /// <summary>
        /// Gets a specific rental registration from the system
        /// </summary>
        /// <param name="rentalNumber">The rental number of the rental registration to get</param>
        /// <returns>A rental info object</returns>
        RentalInfo GetRental(string rentalNumber);
        /// <summary>
        /// Removes all rental registrations from the system
        /// </summary>
        void RemoveAllRentals();
    }
}
