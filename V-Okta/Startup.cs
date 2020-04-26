using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

using Microsoft.AspNetCore.Authentication.Cookies;
using Okta.AspNetCore;


namespace V_Okta
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
            services.AddControllersWithViews();

            var oktaSettings = Configuration.GetSection("OktaSettings").Get<Settings.OktaSettings>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OktaDefaults.MvcAuthenticationScheme;
            })
            .AddCookie()
            .AddOktaMvc(new OktaMvcOptions
            {
                OktaDomain = oktaSettings.OktaDomain,
                ClientId = oktaSettings.ClientId,
                ClientSecret = oktaSettings.ClientSecret,
                OnUserInformationReceived = context =>
               {
                   var values = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(context.User.RootElement.ToString());
                   string username = "";
                   bool hasUsername = values.TryGetValue("preferred_username", out username);

                   if (hasUsername)
                   {
                       var dbContext = context.HttpContext.RequestServices.GetService<Data.VoktaContext>();
                       var user = dbContext.Users.Where(r => r.Username.Equals(username)).FirstOrDefault();

                       if (user == null)
                       {
                           dbContext.Users.Add(new Data.Entities.User()
                           {
                               Username = username
                           });

                           dbContext.SaveChanges();
                       }
                   }

                   return Task.CompletedTask;
               }
            });

            services.AddDbContext<Data.VoktaContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("VoktaDatabase")));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
