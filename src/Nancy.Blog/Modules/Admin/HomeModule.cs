
namespace Nancy.Blog.Modules.Admin
{
    public class HomeModule : AdminModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["index"];
        }
    }
}