using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentElection1.Interface;
using StudentElection1.Model;
using StudentElection1.Service;

namespace StudentElection1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<StudentsDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<StudentsUser, StudentsRole>(options =>
            {

            }).AddEntityFrameworkStores<StudentsDbContext>()
                .AddDefaultTokenProviders().AddUserStore<UserStore<StudentsUser, StudentsRole, StudentsDbContext, Guid>>().
                AddRoleStore<RoleStore<StudentsRole, StudentsDbContext, Guid>>();
            builder.Services.AddScoped<IStudentsService, StudentsService>();
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            });
            builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = "/Account/Login"; });



            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
