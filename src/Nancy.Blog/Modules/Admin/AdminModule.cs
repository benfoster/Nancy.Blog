
namespace Nancy.Blog.Modules.Admin
{
    public abstract class AdminModule : NancyModule
    {
        public AdminModule() : base("admin")
        {
        }
        
        public AdminModule(string modulePath) : base("admin/" + modulePath)
        {
        }
    }
}