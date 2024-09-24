using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sofisoft.Accounts.WorkingSpace.API.Application.Adapters;
using Sofisoft.Accounts.WorkingSpace.API.Application.WebClients;
using Sofisoft.Accounts.WorkingSpace.API.Infrastructure.Exceptions;
using Sofisoft.Accounts.WorkingSpace.API.Models;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IIdentityService _identityService;
        private readonly ILoggingWebClient _logger;

        public CompanyService(IRepository<Company> companyRepository,
            IIdentityService identityService,
            ILoggingWebClient logger)
        {
            _companyRepository = companyRepository
                ?? throw new ArgumentNullException(nameof(companyRepository));
            _identityService = identityService
                ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CompanyDataDto> GetCompanyAsync()
        {
            var companyId = _identityService.CompanyId;
            var userName = _identityService.UserName;

            try
            {
                var company = await _companyRepository.FindOneAsync(
                    f => f.Id == companyId && f.Cancelled == false,
                    p => new CompanyDataDto {
                        Active = p.Active,
                        BusinessName = p.BusinessName,
                        Code = p.Code,
                        CompanyId = p.Id,
                        Tin = p.Tin,
                        TradeName = p.TradeName
                    }
                );

                return company;
            }
            catch (Exception ex)
            {
                var result = await _logger.ErrorAsync(ex.Message, ex.StackTrace, userName);

                throw new WorkingSpaceDomainException("Ocurrio un error al obtener.", result);
            }
        }
    }
}