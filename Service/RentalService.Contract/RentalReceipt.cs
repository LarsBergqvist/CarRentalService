namespace RentalService.Contract
{
    /// <summary>
    /// The status of the rental request action
    /// </summary>
    public enum ERentalRequestStatus
    {
        /// <summary>
        /// The rental request was successful
        /// </summary>
        Ok,
        /// <summary>
        /// The rental request was not successful
        /// </summary>
        NotOk        
    }

    /// <summary>
    /// Represents a receipt that is returned after a rental request
    /// </summary>
    public class RentalReceipt
    {
        /// <summary>
        /// The rental number of the item registred in the system
        /// </summary>
        public string RentalNumber { get; set; }
        /// <summary>
        /// States if the rental request was successful or not
        /// </summary>
        public ERentalRequestStatus Status { get; set; }
        /// <summary>
        /// A complementary message to the status of the request
        /// </summary>
        public string Message { get; set; }
    }
}
