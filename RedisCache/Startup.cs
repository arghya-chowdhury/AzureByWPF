using Owin;

//[assembly: OwinStartupAttribute(typeof(RedisCache.Startup))]
namespace RedisCache
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
