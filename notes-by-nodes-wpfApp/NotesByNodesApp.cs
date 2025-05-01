using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using notes_by_nodes_wpfApp.Settings;
using notes_by_nodes_wpfApp.Services;
using notes_by_nodes.Storage;
using notes_by_nodes.StorageAdapters;
using notes_by_nodes.Service;
using notes_by_nodes.Services;
using System.Formats.Asn1;
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
            services.AddSingleton<INotePresenter, MainViewModelPresenter>();
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
            //var builder = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //Configuration = builder.Build();
            //var sdsd = Configuration.GetRequiredSection("NotesByNodesSettings").Value;
            //configure.UserProfile = "Configured";
        }
        static NodeFileStorageFactory GetNodeFileStorageFactory(IServiceProvider serviceProvider)
        {
            INodeBuilder builder = new NodeBuilder();
            return new NodeFileStorageFactory(builder, "");
        }
    }
}
