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

namespace RecUber
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider? Services { get; }

        public App()
        {
            Services = ConfigureServices();
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

            services.AddDbContext<DatabaseConnection>(options => options.UseSqlite(connectionString: "Data Source=RecUber.db"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<HeaderInformationViewModel>();

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
