using Nancy.Blog.Commands;
using Nancy.Blog.Domain;
using Nancy.ModelBinding;
using Raven.Client;
using System.Linq;

namespace Nancy.Blog.Modules.Admin
{
    public class PostsModule : AdminModule
    {
        private readonly IDocumentSession session;

        public PostsModule(IDocumentSession session) : base("posts")
        {
            this.session = session;

            Get["/"] = p =>
            {
                return session.Query<Post>().OrderByDescending(x => x.PublishDate).ToList();
            };

            Post["/"] = p =>
            {
                var command = this.Bind<AddPostCommand>();

                // TODO validate command

                var post = new Post(command.Title, command.Content, command.Slug, command.Tags, command.PublishDate);
                session.Store(post);

                var location = "/admin/posts/" + post.Id;
                return new Response()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("location", location);
            };
        }
    }
}