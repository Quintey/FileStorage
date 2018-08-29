using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FileStorage.Startup))]
namespace FileStorage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
