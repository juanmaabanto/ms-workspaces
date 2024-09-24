using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Services
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _context; 

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string ClientAppId =>
            _context.HttpContext.User.FindFirst("cliente_id").Value;

        public string CompanyId =>
            _context.HttpContext.User.FindFirst("company_id").Value;

        public string Token =>
            _context.HttpContext.Request.Headers[HeaderNames.Authorization];

        public string UserId =>
            _context.HttpContext.User.FindFirst("user_id").Value;

        public string UserName =>
            _context.HttpContext.User.FindFirst("username").Value;
    }
}