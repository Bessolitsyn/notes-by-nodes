using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using notes_by_nodes.Service;
using notes_by_nodes.Storage;
using notes_by_nodes_winUI.Service;
using notes_by_nodes_winUI.Settings;
using System.IO;
using System;

namespace notes_by_nodes_winUI
{
    public partial class App : Application
    {
        private IServiceProvider? ServiceProvider { get; set; }
        private IConfiguration? Configuration { get; set; }

        public static MainWindow? MainWindow { get; private set; }

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var services = new ServiceCollection();

            // Конфигурация
            ConfigureServices(services);

            // Сервисы
            services.AddSingleton<INodeStorageProvider, NodeFileStorageAdapter>();
            services.AddSingleton<ISingleUserNoteService, SingleUserNoteServiceFacade>();

            // ViewModel
            services.AddSingleton<MainViewModel>();

            // Окно
            services.AddSingleton<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();
            MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            MainWindow.Activate();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            string pathToIniFile = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                 .SetBasePath(pathToIniFile)
                 .AddIniFile("appsettings.ini", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

#if DEBUG
            var current = "c:\\Users\\tocha\\source\\notes-by-nodes\\TestProject\\FilesStorage\\";
            if (Directory.Exists(current))
            {
                Directory.SetCurrentDirectory(current);
            }
            else
            {
                Directory.CreateDirectory(current);
                Directory.SetCurrentDirectory(current);
            }
#else
            string userProfile = configuration.GetRequiredSection("Startup:userprofile").Value ?? throw new NullReferenceException();
            Directory.SetCurrentDirectory(userProfile);
#endif

            services.Configure<NotesByNodesSettings>(options =>
            {
                options.UserProfile = Directory.GetCurrentDirectory();
            });
        }

        internal MainViewModel GetMainViewModel()
        {
            return ServiceProvider?.GetRequiredService<MainViewModel>() ?? throw new NullReferenceException();
        }

        internal ISingleUserNoteService GetNoteService()
        {
            return ServiceProvider?.GetRequiredService<ISingleUserNoteService>() ?? throw new NullReferenceException();
        }
    }
}
