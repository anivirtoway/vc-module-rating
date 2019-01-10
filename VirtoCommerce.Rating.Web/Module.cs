using Microsoft.Practices.Unity;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Rating.Core.Services;
using VirtoCommerce.Rating.Data.Repositories;
using VirtoCommerce.Rating.Data.Services;

namespace VirtoCommerce.Rating.Web
{
    public class Module : ModuleBase
    {
        private readonly string _connectionString = ConfigurationHelper.GetConnectionStringValue("VirtoCommerce");
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void SetupDatabase()
        {
            using (var db = new RatingRepository(_connectionString))
            {
                var initializer = new SetupDatabaseInitializer<RatingRepository, Data.Migrations.Configuration>();
                initializer.InitializeDatabase(db);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            _container.RegisterType<IRatingRepository>(new InjectionFactory(c => new RatingRepository(_connectionString, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>())));
            _container.RegisterType<IRatingService, RatingService>();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            //Registering settings to store module allows to use individual values in each store
            var settingManager = _container.Resolve<ISettingsManager>();
            var storeSettingsNames = new[] { "VirtoCommerce.Rating.RatingEnabled" };
            var storeSettings = settingManager.GetModuleSettings("VirtoCommerce.Rating.Web").Where(x => storeSettingsNames.Contains(x.Name)).ToArray();
            settingManager.RegisterModuleSettings("VirtoCommerce.Store", storeSettings);
        }
    }
}
