﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreMentoring.App.Areas.Identity;
using NetCoreMentoring.App.Infrastructure;

namespace NetCoreMentoring.App.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApp(
            this IServiceCollection services,
            IConfiguration configuration,
            string dbConnectionStringName,
            string rootPath)
        {
            services.AddScoped<ActionInvocationLoggingFilter>();
            services.AddScoped<ImageCacheFilter>();

            var connectionString = configuration.GetConnectionString(dbConnectionStringName);
            if (connectionString.Contains("%CONTENTROOTPATH%"))
            {
                connectionString = connectionString.Replace("%CONTENTROOTPATH%", rootPath);
            }
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(connectionString));

            // This adds AspNetCore Identity Authentication
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();

            // This adds Azure AD Authentication
            //services.AddMicrosoftIdentityWebAppAuthentication(configuration);
            //services.AddRazorPages()
            //    .AddMicrosoftIdentityUI();

            return services;
        }
    }
}