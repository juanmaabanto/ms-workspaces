using Autofac;
using Microsoft.Extensions.Configuration;
using Sofisoft.Enterprise.SeedWork.MongoDB;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        private readonly IConfiguration Configuration;

        public ApplicationModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>));

            builder.Register(c => 
                new MongoContext(Configuration["ConnectionString"], Configuration["Database"]))
            .As<MongoContext>()
            .InstancePerLifetimeScope();

            //Repositorios y Servicios
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("WebClient"))
                .AsImplementedInterfaces();

            // Validadores
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Validator"))
                .AsImplementedInterfaces();
        }
    }
}