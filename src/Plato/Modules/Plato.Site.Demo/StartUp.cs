﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Plato.Internal.Abstractions.SetUp;
using Plato.Internal.Features.Abstractions;
using Plato.Internal.Models.Shell;
using Plato.Internal.Hosting.Abstractions;
using Plato.Internal.Navigation.Abstractions;
using Plato.Internal.Layout.ViewProviders;
using Plato.Site.Demo.Handlers;
using Plato.Site.Demo.Navigation;
using Plato.Site.Demo.Configuration;
using Plato.Site.Demo.Models;
using Plato.Site.Demo.Stores;
using Plato.Site.Demo.ViewProviders;
using Plato.Internal.Models.Users;
using Plato.Site.Demo.Services;

namespace Plato.Site.Demo
{

    public class Startup : StartupBase
    {

        private readonly IShellSettings _shellSettings;

        public Startup(IShellSettings shellSettings)
        {
            _shellSettings = shellSettings;
        }

        public override void ConfigureServices(IServiceCollection services)
        {

            // Set-up event handler
            services.AddScoped<ISetUpEventHandler, SetUpEventHandler>();

            // Feature installation event handler
            services.AddScoped<IFeatureEventHandler, FeatureEventHandler>();
            
            // Navigation provider
            services.AddScoped<INavigationProvider, AdminMenu>();

            // Configuration
            services.AddTransient<IConfigureOptions<DemoOptions>, DemoOptionsConfiguration>();

            // Stores
            services.AddScoped<IDemoSettingsStore<DemoSettings>, DemoSettingsStore>();

            // View providers
            services.AddScoped<IViewProviderManager<DemoSettings>, ViewProviderManager<DemoSettings>>();
            services.AddScoped<IViewProvider<DemoSettings>, AdminViewProvider>();
            
            // Login view provider
            services.AddScoped<IViewProviderManager<UserLogin>, ViewProviderManager<UserLogin>>();
            services.AddScoped<IViewProvider<UserLogin>, LoginViewProvider>();

            // Services
            services.AddScoped<ISampleEntitiesService, SampleEntitiesService>();
            services.AddScoped<ISampleUsersService, SampleUsersService>();
            services.AddScoped<ISampleCategoriesService, SampleCategoriesService>();
            services.AddScoped<ISampleLabelsService, SampleLabelsService>();
            services.AddScoped<ISampleTagsService, SampleTagsService>();
            services.AddScoped<ISampleEntityTagsService, SampleEntityTagsService>();
            services.AddScoped<ISampleEntityLabelsService, SampleEntityLabelsService>();
            services.AddScoped<ISampleEntityCategoriesService, SampleEntityCategoriesService>();

            // Permissions provider
            //services.AddScoped<IPermissionsProvider<Permission>, Permissions>();            

        }

        public override void Configure(
            IApplicationBuilder app,
            IRouteBuilder routes,
            IServiceProvider serviceProvider)
        {

            routes.MapAreaRoute(
                name: "PlatoSiteDemoAdmin",
                areaName: "Plato.Site.Demo",
                template: "admin/settings/demo",
                defaults: new { controller = "Admin", action = "Index" }
            );

        }

    }

}