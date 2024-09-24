namespace Sofisoft.Accounts.WorkingSpace.API.ViewModels
{
    /// <summary>
    /// Error model returned to user.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets the id of the event record.
        /// </summary>
        public string EventLogId { get; }

        /// <summary>
        /// Get the error message.
        /// </summary>
        /// <value></value>
        public string Message { get; }

        /// <summary>
        /// Create a new error model to display to the user.
        /// </summary>
        /// <param name="eventLogId">Record id.</param>
        /// <param name="message">Message to display.</param>
        public ErrorViewModel(string eventLogId, string message)
        {
            EventLogId = eventLogId;
            Message = message;
        }
    }
}