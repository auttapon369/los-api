using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using los_api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace los_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<ApplicationDBContext>(opt => opt.UseInMemoryDatabase("Stock"));
            services.AddDbContext<ApplicationDBContext>(opt => opt.UseInMemoryDatabase("Product"));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ApplicationDBContext context)
        {
           
     
            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
            }

            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            AddData(context);

        }


        private static void AddData(ApplicationDBContext context)
        {


            var product1 = new Models.Product
            {

                Id = "1",
                name = "PC",
                imageurl = "https://img.advice.co.th/images_nas/pic_product4/A0134099/A0134099OK_BIG_1.jpg",
                price = 10.00
            };

            context.Add(product1);

            var stock1 = new Models.Stock
            {
                Id = "abc123",
                productId = product1.Id,
                amount = 50
            };

            context.stocks.Add(stock1);

            context.SaveChanges();
        }
    }
}
