namespace RentalService.Contract
{
    /// <summary>
    /// The state of the rental return action
    /// </summary>
    public enum ERentalReturnStatus
    {
        /// <summary>
        /// The rental return was successful
        /// </summary>
        Ok,
        /// <summary>
        /// The rental return was not successful
        /// </summary>
        NotOk
    }
    /// <summary>
    /// Represents a receipt after a rental return request
    /// </summary>
    public class ReturnReceipt
    {
        /// <summary>
        /// States if the rental return was successful or not
        /// </summary>
        public ERentalReturnStatus Status { get; set; }
        /// <summary>
        /// A complementary message to the status of the rental return
        /// </summary>
        public string Message { get; set; }
    }
}
