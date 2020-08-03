using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(blog.Startup))]
namespace blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
