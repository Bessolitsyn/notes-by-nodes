using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using notes_by_nodes_wpfApp.Settings;
using notes_by_nodes_wpfApp.Services;
using notes_by_nodes.Storage;
using notes_by_nodes.Services;
using notes_by_nodes_wpfApp.ViewModel;

namespace notes_by_nodes_wpfApp
{
    public partial class NotesByNodesApp : Application
    {

        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            // Конфигурация
            services.Configure<NotesByNodesSettings>(ConfigureServices);
            // Сервисы
            //services.AddSingleton<INodeBuilder, NodeBuilder>();
            services.AddSingleton<INodeStorageFactory, StorageFactoryServiceAdapter>();
            services.AddSingleton<INotePresenter, ModelsPresenter>();
            services.AddSingleton<INoteService, NoteServiceFacade>();

            // ViewModel
            services.AddTransient<MainViewModel>();

            // Окно
            services.AddSingleton<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        static void ConfigureServices(NotesByNodesSettings configure)
        {
            string pathToIniFile = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                 .SetBasePath(pathToIniFile)
                 .AddIniFile("appsettings.ini", optional: false, reloadOnChange: true);
            var Configuration = builder.Build();
#if DEBUG
            configure.UserProfile = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\FilesStorage\\";
#else
            configure.UserProfile = Configuration.GetRequiredSection("Startup:userprofile").Value ?? throw new NullReferenceException();
#endif
        }

    }
}
