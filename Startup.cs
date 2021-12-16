using System;

using System.Text;
using System.Threading.Tasks;
using Bookmanagementapi.Configuration;
using Bookmanagementapi.data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Bookmanagementapi
{
    public class Startup
    {
        public string ConnectionString {get; set;}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)

        {

            //Update JWT Config from the Settings

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookmanagementapi", Version = "v1" });
            });
            
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

            
           
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer( jwt => 
            {
                //Getting the secret from the config
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);
                
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
                   
                };
                   

            });
            

            services.AddDefaultIdentity<IdentityUser> (options
                             => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AppDbContext>();
            
            
        }
        // Creating a deafult application user

        /* private async Task CreateUserRoles( UserManager<ApplicationUser> userManager)
            {

                var UserManager = userManager;

                //Assign Admin role to the main User here we have given our newly registered 
                //login id for Admin management
                ApplicationUser user = await UserManager.FindByEmailAsync("test@test.com");
                UserManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
            }

             public class IdentityHostingStartup : IHostingStartup
            {
            public void Configure(IWebHostBuilder builder)
            {
             builder.ConfigureServices((context, services) => {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    context.Configuration.GetConnectionString("ApplicationDBContextConnection")));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<AppDbContext>();
            });
             } */


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bookmanagementapi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           //When creating a default role uncomment this line of code below.
           //CreateDefaultRolesAsync(svp).GetAwaiter().GetResult(); 
           MigrateDatabaseContexts(svp);
        }

        public void MigrateDatabaseContexts(IServiceProvider svp)
         {
             var applicationDbContext = svp.GetRequiredService<AppDbContext>();
             applicationDbContext.Database.Migrate();
         }

         //If you want to create a default role you can uncomment the code below
        /*public async Task CreateDefaultRolesAsync(IServiceProvider svp) 
        {
            string[] roles = new string[] {"SystemAdministrator", "User"};

            var roleManager = svp.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole{Name = role});
                }
            }



            
        } */
    }
}
