using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PharmaceuticalChain.API.Auth0;
using PharmaceuticalChain.API.Auth0.Services.Implementations;
using PharmaceuticalChain.API.Auth0.Services.Interfaces;
using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Implementations;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.BackgroundJobs.Implementations;
using PharmaceuticalChain.API.Services.BackgroundJobs.Interfaces;
using PharmaceuticalChain.API.Services.Implementations;
using PharmaceuticalChain.API.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace PharmaceuticalChain.API
{
    public class Startup
    {
        private readonly string CorsPolicy = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var ethereumSettings = Configuration.GetSection("EthereumSettings");
            services.Configure<EthereumSettings>(ethereumSettings);

            services.AddTransient<IEthereumService, EthereumService>();

            services.AddTransient<IAuth0Service, Auth0Service>();

            services.AddTransient<IUploadService, UploadService>();

            services.AddTransient<ISupplyChainService, SupplyChainService>();
            services.AddTransient<IVerificationService, VerificationService>();

            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IDrugTransactionService, DrugTransactionService>();
            services.AddTransient<IMedicineService, MedicineService>();
            services.AddTransient<IMedicineBatchService, MedicineBatchService>();
            services.AddTransient<IMedicineBatchTransferService, MedicineBatchTransferService>();

            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<IMedicineRepository, MedicineRepository>();
            services.AddTransient<IMedicineBatchRepository, MedicineBatchRepository>();
            services.AddTransient<IMedicineBatchTransferRepository, MedicineBatchTransferRepository>();
            services.AddTransient<IResourceRepository, ResourceRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "PharmaChain Server API",
                    Version = "v1"
                });

                c.OperationFilter<SwaggerFileOperationFilter>();

                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization",
                        Type = "apiKey"
                    }
                );
                c.AddSecurityRequirement(
                    new Dictionary<string, IEnumerable<string>>
                    {
                        {
                            "Bearer", Enumerable.Empty<string>()
                        },
                    }
                );

                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);
            });


            services.AddCors(o =>
            {
                o.AddPolicy(CorsPolicy,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("Location", "location");
                    });
            });

            // Hangfire automatically creates necessary tables in the database. No need to run extra migrations for the service.
            services.AddHangfire(
                x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddHangfireServer();

            services.AddTransient<ITenantBackgroundJob, TenantBackgroundJob>();
            services.AddTransient<IMedicineBackgroundJob, MedicineBackgroundJob>();
            services.AddTransient<IMedicineBatchBackgroundJob, MedicineBatchBackgroundJob>();
            services.AddTransient<IMedicineBatchTransferBackgroundJob, MedicineBatchTransferBackgroundJob>();

            // Auth0 configure
            var domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("create:users", policy => policy.Requirements.Add(new HasScopeRequirement("scope", "create:users", domain)));
                options.AddPolicy("roles:admin", policy => policy.Requirements.Add(new HasScopeRequirement("https://www.pharmachain.net/roles", "admin", domain)));
                options.AddPolicy("roles:manufacturer", policy => policy.Requirements.Add(new HasScopeRequirement("https://www.pharmachain.net/roles", "manufacturer", domain)));
                options.AddPolicy("roles:retailer", policy => policy.Requirements.Add(new HasScopeRequirement("https://www.pharmachain.net/roles", "retailer", domain)));
            });

            // Register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(CorsPolicy);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PharmaChain API v1");
            });

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            //RecurringJob.AddOrUpdate<ITenantBackgroundJob>(
            //    tenantBackgroundJob => tenantBackgroundJob.SyncDatabaseWithBlockchain(),
            //    Cron.Minutely);

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
