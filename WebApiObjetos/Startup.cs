using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApiObjetos.Data;
using WebApiObjetos.Models.Repositories;
using WebApiObjetos.Models.Repositories.Interfaces;
using WebApiObjetos.Services;
using WebApiObjetos.Services.Interfaces;

namespace WebApiObjetos
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
       
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyConnStr")));

            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<DbContext, ApplicationDBContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILocationRepository,LocationRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);//.AddXmlDataContractSerializerFormatters(); con esto si seteas en el header del request el tipo en que queres que te devuelva la info esto lo formatea solo
                                                                                        //Accept tipo;  // tambien podes setear que formato queres que te devuelta cada método poniendo [Produces("formato")]

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILogger<Startup>logger)
        {
            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                developerExceptionPageOptions.SourceCodeLineCount = 10; // muestra 10 lineas + y 10 lineas menos de donde se genero la excepcion.
                app.UseDeveloperExceptionPage(); // es mejor ponerlo lo antes posible por si se rompe laguno de los otros pipes
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //throw new Exception("excepcion"); para demostrar como se genera la pagina de excepciones.

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            logger.LogInformation("comienzo del Programa"); // loggeo el inicio, imprime por consola.

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
