
namespace Nancy.Blog.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => Response.AsRedirect("/posts");
        }
    }
}