using System;
using System.Collections.Generic;
using System.Linq;
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
    public class OptionService : IOptionService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Option> _optionRepository;
        private readonly IIdentityService _identityService;
        private readonly ILoggingWebClient _logger;

        public OptionService(IRepository<Menu> menuRepository,
            IRepository<Module> moduleRepository,
            IRepository<Option> optionRepository,
            IIdentityService identityService,
            ILoggingWebClient logger)
        {
            _menuRepository = menuRepository
                ?? throw new ArgumentNullException(nameof(menuRepository));
            _moduleRepository = moduleRepository
                ?? throw new ArgumentNullException(nameof(moduleRepository));
            _optionRepository = optionRepository
                ?? throw new ArgumentNullException(nameof(optionRepository));
            _identityService = identityService
                ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OptionDto> GetOptionAsync(string optionId)
        {
            var userName = _identityService.UserName;

            try
            {
                PipelineDefinition<Option, OptionDto> pipeline = new BsonDocument[]
                {
                    new BsonDocument {{"$match", new BsonDocument {{"_id", new ObjectId(optionId)}}}},
                    new BsonDocument {{"$lookup", new BsonDocument
                        {
                            {"from", "action"},
                            {"localField", "actions"},
                            {"foreignField", "_id"},
                            {"as", "actions"}
                        }
                    }},
                    new BsonDocument {{"$addFields",
                        new BsonDocument {{ "actions", new BsonDocument {{"$map",
                            new BsonDocument
                            {
                                {"input", "$actions"},
                                {"as", "item"},
                                {"in",
                                    new BsonDocument
                                    {
                                        {"ActionId", new BsonDocument {{ "$toString", "$$item._id" }}},
                                        {"Name", "$$item.name"}
                                    }
                                }
                            }
                        }}
                        }}
                    }},
                    new BsonDocument {{"$replaceRoot",
                        new BsonDocument {{"newRoot",
                            new BsonDocument
                            {
                                {"Actions", "$actions"},
                                {"Icon", "$icon"},
                                {"Name", "$name"},
                                {"OptionId", new BsonDocument {{ "$toString", "$_id" }}}
                            }
                        }}
                    }}
                };

                var option = (await _optionRepository
                    .AggregateAsync(pipeline)).FirstOrDefault();

                return option ?? throw new KeyNotFoundException(nameof(optionId));
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var result = await _logger.ErrorAsync(ex.Message, ex.StackTrace, userName);

                throw new WorkingSpaceDomainException("Ocurrio un error al obtener.", result);
            }
        }

        public async Task<IEnumerable<OptionListDto>> GetOptionsAsync(string moduleId)
        {
            var userName = _identityService.UserName;

            try
            {
                PipelineDefinition<Module, OptionListDto> pipeline1 = new BsonDocument[]
                {
                    new BsonDocument {{"$match", new BsonDocument {{"_id", new ObjectId(moduleId)}}}},
                    new BsonDocument {{"$unwind", "$options"}},
                    new BsonDocument {{"$lookup", new BsonDocument
                        {
                            {"from", "option"},
                            {"localField", "options.optionId"},
                            {"foreignField", "_id"},
                            {"as", "myOptions"}
                        }
                    }},
                    new BsonDocument {{"$unwind", "$myOptions"}},
                    new BsonDocument {{"$replaceRoot",
                        new BsonDocument {{"newRoot",
                            new BsonDocument
                            {
                                {"Collapsible", false},
                                {"Icon", "$myOptions.icon"},
                                {"Leaf", true},
                                {"Name", "$myOptions.name"},
                                {"OptionId", new BsonDocument {{ "$toString", "$myOptions._id" }}},
                                {"Order", "$options.order"},
                                {"Paths", "$myOptions.paths"}
                            }
                        }}
                    }}
                };

                PipelineDefinition<Menu, OptionListDto> pipeline2 = new BsonDocument[]
                {
                    new BsonDocument {{"$match", new BsonDocument {{"moduleId", new ObjectId(moduleId)}}}},
                    new BsonDocument {{"$unwind", "$options"}},
                    new BsonDocument {{"$lookup", new BsonDocument
                        {
                            {"from", "option"},
                            {"localField", "options.optionId"},
                            {"foreignField", "_id"},
                            {"as", "myOptions"}
                        }
                    }},
                    new BsonDocument {{"$unwind", "$myOptions"}},
                    new BsonDocument {{"$replaceRoot",
                        new BsonDocument {{"newRoot",
                            new BsonDocument
                            {
                                {"Collapsible", false},
                                {"Icon", "$myOptions.icon"},
                                {"Leaf", true},
                                {"Name", "$myOptions.name"},
                                {"OptionId", new BsonDocument {{ "$toString", "$myOptions._id" }}},
                                {"Order", "$options.order"},
                                {"ParentId", new BsonDocument {{ "$toString", "$_id" }}},
                                {"Paths", "$myOptions.paths"}
                            }
                        }}
                    }}
                };

                var tOptions1 = _moduleRepository
                    .AggregateAsync(pipeline1);
                
                var tOptions2 = _menuRepository
                    .AggregateAsync(pipeline2);

                var tOptions3 = _menuRepository.FilterByAsync(
                    f => f.ModuleId == moduleId,
                    p => new OptionListDto {
                        Collapsible = p.Collapsible,
                        Icon = p.Icon,
                        Leaf = false,
                        Name = p.Name,
                        OptionId = p.Id,
                        Order = p.Order,
                        ParentId = p.ParentId
                    }
                );

                await Task.WhenAll(tOptions1, tOptions2, tOptions3);

                var lstOptions = new List<OptionListDto>();

                lstOptions.AddRange(tOptions1.Result);
                lstOptions.AddRange(tOptions2.Result);
                lstOptions.AddRange(tOptions3.Result);

                return lstOptions;
            }
            catch (Exception ex)
            {
                var result = await _logger.ErrorAsync(ex.Message, ex.StackTrace, userName);

                throw new WorkingSpaceDomainException("Ocurrio un error al obtener.", result);
            }
        }

    }
}