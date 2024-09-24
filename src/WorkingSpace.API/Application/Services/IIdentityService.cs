namespace Sofisoft.Accounts.WorkingSpace.API.Application.Services
{
    /// <summary>
    /// Interface to obtain access token information.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Get the id of the client application.
        /// </summary>
        string ClientAppId { get; }

        /// <summary>
        /// Get the company id of the authenticated user.
        /// </summary>
        string CompanyId { get; }

        /// <summary>
        /// Get the bearer Token.
        /// </summary>
        string Token { get; }

        /// <summary>
        /// Get the id of the authenticated user.
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// Get the authenticated username.
        /// </summary>
        string UserName { get; }

    }

    
}