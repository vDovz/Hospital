using Hospital.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hospital.Startup))]
namespace Hospital
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
 
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);                

                var user = new ApplicationUser();
                user.UserName = "root";
                user.Email = "root@my.com";

                string userPWD = "AAA111/";

                var chkUser = UserManager.Create(user, userPWD);

                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }  
            if (!roleManager.RoleExists("Patients"))
            {
                var role = new IdentityRole();
                role.Name = "Patients";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Doctors"))
            {
                var role = new IdentityRole();
                role.Name = "Doctors";
                roleManager.Create(role);
            }
        }
    }


}