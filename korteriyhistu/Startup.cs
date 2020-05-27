using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using korteriyhistu.Models;
using DinkToPdf.Contracts;
using DinkToPdf;
using korteriyhistu.Data;

namespace korteriyhistu
{
    public class Startup
    {
        readonly string BillsFEAppOrigins = "_billsFEAppOrigins";

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            isDev = env.IsDevelopment();
        }

        public IConfiguration Configuration { get; }

        private bool isDev;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: BillsFEAppOrigins,
                    builder => {
                        builder.WithOrigins("https://bills-management-app.herokuapp.com", "http://localhost:4200")
                        .AllowAnyHeader();
                    });
            });

            services.AddMvc().AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            
            services.AddMvc(o =>
            {
                o.AllowEmptyInputInBodyModelBinding = true;
            });

            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<ISupervisor, BillSupervisor>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();

            if (isDev)
            {
                services.AddDbContext<BillsContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "InMemBillsDB"));
                services.AddDbContext<BudgetContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "InMemBudgetDB"));
                services.AddDbContext<ApartmentsContext>(options =>
                 options.UseInMemoryDatabase(databaseName: "InMemApartmentsDB"));
            }
            else
            {
                services.AddDbContext<BillsContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("BillsContext")));
                services.AddDbContext<BudgetContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BudgetContext")));
                services.AddDbContext<ApartmentsContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("ApartmentsContext")));
            }
                 
            //registers DinkToPdf library
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddDbContext<LogEntriesContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("LogEntriesContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (isDev)
            {
                app.UseDeveloperExceptionPage();
                DataGenerator.Initialize(
                    new BillsContext(
                        app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DbContextOptions<BillsContext>>()),
                    new BudgetContext(
                        app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DbContextOptions<BudgetContext>>()),
                    new ApartmentsContext(app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DbContextOptions<ApartmentsContext>>()));

            }

            app.UseCors(BillsFEAppOrigins);

            app.UseMvc();
        }
    }
}
