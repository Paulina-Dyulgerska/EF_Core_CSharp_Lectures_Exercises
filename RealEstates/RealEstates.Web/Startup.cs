using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealEstates.Data;
using RealEstates.Services;

namespace RealEstates.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //tuk registriram vsichki services, koito shte izpolzwam. Towa e methoda, kojto izae az kato poiskam
        //daden interface, toj koj konkreten class da mi dade!!!
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RealEstateContext>(options =>
            {
                options.UseSqlServer(this.Configuration["ConnectionString"]);
            });
            services.AddTransient<IDistrictService, DistrictService>();
            services.AddTransient<IRealEstatePropertiesService, RealEstatePropertiesService>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Middlewares 

            //ako sym v development sreda, shte mi izkarwa pylnata information za errorite, ako ne sym - samo
            //error page-a shte mi se pokazwa:
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //gelda se dali addresa pochwa s https, i ako ne - prenasochva se kym https, za da moje vryzkata da e
            //sigurna:
            app.UseHttpsRedirection();

            //proverqwa dali towa, koeto iska user-a ne e nqkoe ot tezi statichni neshta, koito imam v wwwroot
            //directory-to:
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization(); //proverqwa imame li authorization za da dostypi usera tazi stranica.

            //stiga do defaultniqt page - t.e. ako controllera ne e posochen kak se kazwa, togawa
            //defaultnoto ime na controllera e Home, a ako actiona ne e posochen kak se kazwa, to
            //togawa defaultnoto mu e Index() method - toj se vika. Ako ne e posocheno nishto, se wika:
            //stranicata /Home/Index!
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
