using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmployeePayrollMVC.Startup))]
namespace EmployeePayrollMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
