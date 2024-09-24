using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Sofisoft.Accounts.WorkingSpace.API.Application.Adapters;
using Sofisoft.Accounts.WorkingSpace.API.Application.WebClients;
using Sofisoft.Accounts.WorkingSpace.API.Infrastructure.Exceptions;
using Sofisoft.Accounts.WorkingSpace.API.Models;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Workspace> _workspaceRepository;
        private readonly IIdentityService _identityService;
        private readonly ILoggingWebClient _logger;

        public WorkspaceService(IRepository<Workspace> workspaceRepository,
            IRepository<Company> companyRepository,
            IIdentityService identityService,
            ILoggingWebClient logger)
        {
            _companyRepository = companyRepository
                ?? throw new ArgumentNullException(nameof(companyRepository));
            _workspaceRepository = workspaceRepository
                ?? throw new ArgumentNullException(nameof(workspaceRepository));
            _identityService = identityService
                ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ModuleWorkspaceDto>> GetModulesAsync(string workspaceId)
        {
            var clientAppId = _identityService.ClientAppId;
            var userName = _identityService.UserName;

            try
            {
                PipelineDefinition<Workspace, ModuleWorkspaceDto> pipeline = new BsonDocument[]
                {
                    new BsonDocument {{"$match", new BsonDocument {{"_id", new ObjectId(workspaceId)}}}},
                    new BsonDocument {{"$unwind", "$modules"}},
                    new BsonDocument {{"$match", new BsonDocument {{"modules.active", true}}}},
                    new BsonDocument {{"$replaceRoot", new BsonDocument {{"newRoot", "$modules"}} }},
                    new BsonDocument {{"$lookup", new BsonDocument
                        {
                            {"from", "module"},
                            {"localField", "moduleId"},
                            {"foreignField", "_id"},
                            {"as", "modules"}
                        }
                    }},
                    new BsonDocument {{"$unwind", "$modules"}},
                    new BsonDocument {{"$match", new BsonDocument
                        {
                            {"modules.clientAppId", new ObjectId(clientAppId)},
                            {"modules.active", true}
                        }
                    }},
                    new BsonDocument {{"$replaceRoot",
                        new BsonDocument {{"newRoot",
                            new BsonDocument
                            {
                                {"ModuleId", new BsonDocument {{ "$toString", "$modules._id" }}},
                                {"Name", "$modules.name"},
                                {"ShortName", "$modules.shortName"},
                                {"Icon", "$modules.icon"}
                            }
                        }}
                    }}
                };
                
                var details = await _workspaceRepository
                    .AggregateAsync(pipeline);

                return details;
            }
            catch (Exception ex)
            {
                var result = await _logger.ErrorAsync(ex.Message, ex.StackTrace, userName);

                throw new WorkingSpaceDomainException("Ocurrio un error al obtener.", result);
            }
        }
    
        public async Task<IEnumerable<RouteDto>> GetRoutesAsync(string workspaceId)
        {
            var clientAppId = _identityService.ClientAppId;
            var userName = _identityService.UserName;

            try
            {
                PipelineDefinition<Workspace, RouteDto> pipeline1 = new BsonDocument[]
                {
                    new BsonDocument {{"$match", new BsonDocument {{"_id", new ObjectId(workspaceId)}}}},
                    new BsonDocument {{"$unwind", "$modules"}},
                    new BsonDocument {{"$match", new BsonDocument {{"modules.active", true}}}},
                    new BsonDocument {{"$lookup", new BsonDocument
                        {
                            {"from", "module"},
                            {"localField", "modules.moduleId"},
                            {"foreignField", "_id"},
                            {"as", "modules"}
                        }
                    }},
                    new BsonDocument {{"$unwind", "$modules"}},
                    new BsonDocument {{"$match", new BsonDocument
                        {
                            {"modules.clientAppId", new ObjectId(clientAppId)},
                            {"modules.active", true}
                        }
                    }},
                    new BsonDocument {{"$unwind", "$modules.options"}},
                    new BsonDocument {{"$lookup", new BsonDocument
                        {
                            {"from", "option"},
                            {"localField", "modules.options.optionId"},
                            {"foreignField", "_id"},
                            {"as", "myOptions"}
                        }
                    }},
                    new BsonDocument {{"$unwind", "$myOptions"}},
                    new BsonDocument {{"$match", new BsonDocument
                        {
                            {"myOptions.hostName", new BsonDocument {{ "$ne", BsonNull.Value }} },
                            {"myOptions.hostUri", new BsonDocument {{ "$ne", BsonNull.Value }} }
                        }
                    }},
                    new BsonDocument {{ "$replaceRoot",
                        new BsonDocument {{"newRoot",
                            new BsonDocument
                            {
                                {"HostName", "$myOptions.hostName"},
                                {"HostUri", "$myOptions.hostUri"},
                                {"OptionId", new BsonDocument {{ "$toString", "$myOptions._id" }}},
                                {"Paths", "$myOptions.paths"}
                            }
                        }}
                    }}
                };

                PipelineDefinition<Workspace, RouteDto> pipeline2 = new BsonDocument[]
                {
                    new BsonDocument {{"$match", new BsonDocument {{"_id", new ObjectId(workspaceId)}}}},
                    new BsonDocument {{"$unwind", "$modules"}},
                    new BsonDocument {{"$match", new BsonDocument {{"modules.active", true}}}},
                    new BsonDocument {{"$lookup", new BsonDocument
                        {
                            {"from", "module"},
                            {"localField", "modules.moduleId"},
                            {"foreignField", "_id"},
                            {"as", "modules"}
                        }
                    }},
                    new BsonDocument {{"$unwind", "$modules"}},
                    new BsonDocument {{"$match", new BsonDocument
                        {
                            {"modules.clientAppId", new ObjectId(clientAppId)},
                            {"modules.active", true}
                        }
                    }},
                    new BsonDocument {{"$lookup", new BsonDocument
                        {
                            {"from", "menu"},
                            {"localField", "modules._id"},
                            {"foreignField", "moduleId"},
                            {"as", "menus"}
                        }
                    }},
                    new BsonDocument {{"$unwind", "$menus"}},
                    new BsonDocument {{"$unwind", "$menus.options"}},
                    new BsonDocument {{"$lookup", new BsonDocument
                        {
                            {"from", "option"},
                            {"localField", "menus.options.optionId"},
                            {"foreignField", "_id"},
                            {"as", "myOptions"}
                        }
                    }},
                    new BsonDocument {{"$unwind", "$myOptions"}},
                    new BsonDocument {{"$match", new BsonDocument
                        {
                            {"myOptions.hostName", new BsonDocument {{ "$ne", BsonNull.Value }} },
                            {"myOptions.hostUri", new BsonDocument {{ "$ne", BsonNull.Value }} }
                        }
                    }},
                    new BsonDocument {{ "$replaceRoot",
                        new BsonDocument {{"newRoot",
                            new BsonDocument
                            {
                                {"HostName", "$myOptions.hostName"},
                                {"HostUri", "$myOptions.hostUri"},
                                {"OptionId", new BsonDocument {{ "$toString", "$myOptions._id" }}},
                                {"Paths", "$myOptions.paths"}
                            }
                        }}
                    }}
                };

                var tRoutes1 = _workspaceRepository
                    .AggregateAsync(pipeline1);
                
                var tRoutes2 = _workspaceRepository
                    .AggregateAsync(pipeline2);

                await Task.WhenAll(tRoutes1, tRoutes2);

                var lstRoutes = new List<RouteDto>();

                lstRoutes.AddRange(tRoutes1.Result);
                lstRoutes.AddRange(tRoutes2.Result);

                return lstRoutes;
            }
            catch (Exception ex)
            {
                var result = await _logger.ErrorAsync(ex.Message, ex.StackTrace, userName);

                throw new WorkingSpaceDomainException("Ocurrio un error al obtener.", result);
            }
        }

        public async Task<string> GetWorkspaceIdAsync()
        {
            var companyId = _identityService.CompanyId;
            var userName = _identityService.UserName;

            try
            {
                return await _companyRepository.FindOneAsync(
                    f => f.Id == companyId && f.Cancelled == false,
                    p => p.WorkspaceId
                );
            }
            catch (Exception ex)
            {
                var result = await _logger.ErrorAsync(ex.Message, ex.StackTrace, userName);

                throw new WorkingSpaceDomainException("Ocurrio un error al obtener.", result);
            }
        }
        
    }
}