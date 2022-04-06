using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RecUber.ViewModel;
using RecUber.Interface;
using RecUber.Repository;
using Microsoft.EntityFrameworkCore;
using RecUber.View;
using System.IO;

namespace RecUber
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider? Services { get; }

        private const string DbName = "recuber.db";

        public App()
        {
            Services = ConfigureServices();

            if (!File.Exists(DbName)) Services!.GetService<DatabaseConnection>()!.Database.Migrate();
        }

        private IServiceProvider? ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddScoped<IConfiguration>((cfg) =>
            {
                return new ConfigurationBuilder()
                    .AddJsonFile("config.json")
                    .Build();
            });

            services.AddDbContext<DatabaseConnection>(options => options.UseSqlite(connectionString: $"Data Source={DbName}"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<HeaderInformationViewModel>();

            services.AddScoped<ConfigurationViewModel>();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
            mainWindow.Show();
        }
    }
}
