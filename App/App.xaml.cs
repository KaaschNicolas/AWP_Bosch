﻿using System.Linq.Expressions;
using App.Activation;
using App.Contracts.Services;
using App.Core.Contracts.Services;
using App.Core.Models;
using App.Core.DataAccess;
using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Helpers;
using App.Models;
using App.Services;
using App.ViewModels;
using App.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Serilog;

namespace App;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public App()
    {
        InitializeComponent();

        var builder = new ConfigurationBuilder();
        ConfigSetup(builder);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .CreateLogger();


        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        UseSerilog().
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IInfoBarService, InfoBarService>();


            // Core Services
            services.AddSingleton<IFileService, FileService>();
            services.AddTransient<ILoggingService, LoggingService>();
            services.AddTransient<ICrudService<PcbType>, CrudService<PcbType>>();
            services.AddTransient<ICrudService<StorageLocation>, CrudService<StorageLocation>>();
            //services.AddTransient<ICrudService<BaseEntity>, CrudService<BaseEntity>>();


            // Views and ViewModels
            services.AddTransient<StorageLocationPaginationViewModel>();
            services.AddTransient<MD_CreatePartNumberPage>();
            services.AddTransient<MDPartNumberViewModel>();
            services.AddTransient<MDPartNumberPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<BlankViewModel>();
            services.AddTransient<BlankPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            services.AddDbContext<BoschContext>(
                    options => options.UseSqlServer(@"Data Source=localhost;Initial Catalog=TestDB;User ID=sa;Password=Nicolas!1234;TrustServerCertificate=True"));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private static void ConfigSetup(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())

            .AddJsonFile("C:\\Users\\Admin\\source\\repos\\AWP_Bosch\\App\\appsettings.json", optional: false, reloadOnChange: true)

            .AddEnvironmentVariables();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
