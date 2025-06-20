using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using notes_by_nodes_wpfApp.Settings;
using notes_by_nodes_wpfApp.Service;
using notes_by_nodes.Storage;
using notes_by_nodes.Service;

namespace notes_by_nodes_wpfApp
{
    public partial class NotesByNodesApp : Application
    {

        private IServiceProvider? ServiceProvider { get; set; }
        private IConfiguration? Configuration { get; set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            // Конфигурация
            services.Configure<NotesByNodesSettings>(ConfigureServices);
            // Сервисы
            services.AddSingleton<INodeStorageProvider, NodeFileStorageAdapter>();
            services.AddSingleton<INoteService, NoteServiceFacade>();

            // ViewModel
            //services.AddTransient<MainViewModel>();
            services.AddSingleton<MainViewModel>();

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
            var current = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\TestProject\\FilesStorage\\";
            if (Directory.Exists(current))
            {
                Directory.SetCurrentDirectory(current);
            }
            else { 
                Directory.CreateDirectory(current);
            }
            configure.UserProfile = Directory.GetCurrentDirectory();
            //configure.UserProfile = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\FilesStorage\\";
#else
            configure.UserProfile = Configuration.GetRequiredSection("Startup:userprofile").Value ?? throw new NullReferenceException();
#endif
        }

        internal MainViewModel GetMainViewModel()
        {
            return ServiceProvider?.GetRequiredService<MainViewModel>() ?? throw new NullReferenceException();
        }

        internal INoteService GetNoteService()
        {
            return ServiceProvider?.GetRequiredService<INoteService>() ?? throw new NullReferenceException();
        }
    }
}
