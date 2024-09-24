using System.Threading.Tasks;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.WebClients
{
    /// <summary>
    /// Interface for event registration.
    /// </summary>
    public interface ILoggingWebClient
    {
        /// <summary>
        /// Registers an event of type error.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="trace">Traceability of the error.</param>
        /// <param name="username">User name.</param>
        /// <returns>Returns the Id of the registered event.</returns>
        Task<string> ErrorAsync(string message, string trace, string username);
    }
}