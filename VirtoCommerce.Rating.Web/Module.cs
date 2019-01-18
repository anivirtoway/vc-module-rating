using Microsoft.Practices.Unity;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Rating.Core.Services;
using VirtoCommerce.Rating.Data.Calculators;
using VirtoCommerce.Rating.Data.Repositories;
using VirtoCommerce.Rating.Data.Services;

namespace VirtoCommerce.Rating.Web
{
    public class Module : ModuleBase
    {
        private readonly string _connectionString = ConfigurationHelper.GetConnectionStringValue("VirtoCommerce");
        private readonly IUnityContainer _container;

        private const string ConfigStoreGroupName = "Rating";
        private const string ConfigModuleId = "VirtoCommerce.Rating";
        private const string ConfigStoreModuleId = "VirtoCommerce.Store";
        private const string ConfigCalculatorSettingsName = "Rating.Calculation.Method";
        private const string ConfigCalculatorSettingsTitle = "Calculation method";

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

            _container.RegisterType<IRatingRepository>(
                new InjectionFactory(c => new RatingRepository(
                        _connectionString,
                        new EntityPrimaryKeyGeneratorInterceptor(),
                        _container.Resolve<AuditableInterceptor>()))
                    );
            _container.RegisterType<IRatingService, RatingService>();
            _container.RegisterType<IRatingCalculator, AverageRatingCalculator>(new AverageRatingCalculator().Name);
            _container.RegisterType<IRatingCalculator, WilsonRatingCalculator>(new WilsonRatingCalculator().Name);
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            RegisterStoreSettings();
        }

        private void RegisterStoreSettings()
        {
            var settingManager = _container.Resolve<ISettingsManager>();
            var storeSettings = settingManager.GetModuleSettings(ConfigModuleId)
                                              .Where(x => x.GroupName == ConfigStoreGroupName)
                                              .ToList();

            storeSettings.Add(GetCalculatorStoreSettings());
            settingManager.RegisterModuleSettings(ConfigStoreModuleId, storeSettings.ToArray());
        }

        private SettingEntry GetCalculatorStoreSettings()
        {
            var defaultCalculator = new AverageRatingCalculator();
            var calculatorsNames = _container.ResolveAll<IRatingCalculator>()
                                             .Select(x => x.Name)
                                             .ToArray();

            return new SettingEntry
            {
                GroupName = ConfigStoreGroupName,
                Name = ConfigCalculatorSettingsName,
                Title = ConfigCalculatorSettingsTitle,
                ValueType = SettingValueType.ShortText,
                Value = defaultCalculator.Name,
                DefaultValue = defaultCalculator.Name,
                AllowedValues = calculatorsNames
            };
        }
    }
}
