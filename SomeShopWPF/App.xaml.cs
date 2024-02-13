using Microsoft.Extensions.DependencyInjection;
using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.Services.Implementations;
using SomeShopWPF.ViewModels;
using SomeShopWPF.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SomeShopWPF
{
    public partial class App : Application
    {
        private static IServiceProvider? _services;

        public static IServiceProvider? Services => _services ??= InitializeServices().BuildServiceProvider();

        private static IServiceCollection InitializeServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainWindowViewModel>();
            services.AddScoped<AuthViewModel>();
            services.AddScoped<AddClientViewModel>();
            services.AddScoped<EditClientViewModel>();

            services.AddSingleton<IUserDialog, UserDialogService>();
            services.AddSingleton<IRepository, Repository>();

            services.AddTransient(
                s =>
                {
                    var scope = s.CreateScope();
                    var model = s.GetRequiredService<AuthViewModel>();
                    var window = new AuthWindow { DataContext = model };
                    model.DialogComplete += (_, _) => window.Close();
                    window.Closed += (_, _) => scope.Dispose();
                    return window;
                });

            services.AddTransient(
                s =>
                {                    
                    var model = s.GetRequiredService<MainWindowViewModel>();
                    var window = new MainWindow { DataContext = model };
                    model.DialogComplete += (_, _) => window.Close();                 
                    return window;
                });

            services.AddTransient(
                s =>
                {
                    var scope = s.CreateScope();
                    var model = s.GetRequiredService<AddClientViewModel>();
                    var window = new AddClientWindow { DataContext = model };
                    model.DialogComplete += (_, _) => window.Close();
                    window.Closed += (_, _) =>
                    {
                        var m = s.GetRequiredService<MainWindowViewModel>();
                        var r = s.GetRequiredService<IRepository>();
                        m.ClientsList = new ObservableCollection<Client>(r.GetClients());
                        scope.Dispose();
                    };
                    return window;
                });

            services.AddTransient(
                s =>
                {
                    var scope = s.CreateScope();
                    var model = s.GetRequiredService<EditClientViewModel>();
                    var window = new EditeClientWindow { DataContext = model };
                    model.DialogComplete += (_, _) => window.Close();
                    window.Closed += (_, _) =>
                    {
                        var m = s.GetRequiredService<MainWindowViewModel>();
                        var r = s.GetRequiredService<IRepository>();
                        m.ClientsList = new ObservableCollection<Client>(r.GetClients());
                        scope.Dispose();
                    };
                    return window;
                });

            return services;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Services.GetRequiredService<IUserDialog>().OpenAuthWindow();
        }
    }
}
