using Fabrik.Common;
using Nancy.Blog.Domain;
using Nancy.Blog.Indexes;
using Nancy.Blog.Model;
using Raven.Client;
using System;
using System.Linq;

namespace Nancy.Blog.Modules
{
    public class PostsModule : NancyModule
    {
        private const string PageRegex = "[1-9]+[0-9]*";
        private const string SlugRegex = "([a-z0-9])([a-z0-9-]+)*([a-z0-9])";
        private const string SlugWithSegmentsRegex = "(?!-)[a-z0-9-]+(?<!-)(/(?!-)[a-z0-9-]+(?<!-))*";
        private const int DefaultPageSize = 20;

        private readonly IDocumentSession session;
        
        public PostsModule(IDocumentSession session) : base("posts")
        {
            this.session = session;

            Get["/"] = p =>
            {
                var posts = session.Query<Post>()
                    .Where(x => x.PublishDate <= DateTime.UtcNow)
                    .OrderByDescending(x => x.PublishDate)
                    .Take(DefaultPageSize)
                    .ToList();

                return View["list", posts];
            };

            Get["/page/(?<page>{0})".FormatWith(PageRegex)] = p => {

                int pageSize = Request.Query.PageSize.HasValue ? Request.Query.PageSize : DefaultPageSize;
                int page = p.Page - 1;

                var posts = session.Query<Post>()
                    .Where(x => x.PublishDate <= DateTime.UtcNow)
                    .OrderByDescending(x => x.PublishDate)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                return View["list", posts];
            };

            Get["/tagged/(?<tag>{0})".FormatWith(SlugRegex)] = p =>
            {
                string tag = p.Tag;
                
                var posts = session.Query<Post>()
                    .Where(x => x.PublishDate <= DateTime.UtcNow && x.Tags.Any(t => t.Slug == tag))
                    .OrderByDescending(x => x.PublishDate)
                    .Take(DefaultPageSize)
                    .ToList();

                return View["list", posts];
            };
            
            Get["/tagged/(?<tag>{0})/page/(?<page>{1})".FormatWith(SlugRegex, PageRegex)] = p =>
            {
                var tag = p.Tag as string;
                int pageSize = Request.Query.PageSize.HasValue ? Request.Query.PageSize : DefaultPageSize;
                int page = p.Page - 1;

                var posts = session.Query<Post>()
                    .Where(x => x.PublishDate <= DateTime.UtcNow && x.Tags.Any(t => t.Slug == tag))
                    .OrderByDescending(x => x.PublishDate)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                return View["list", "posts"];
            };

            Get["/tags"] = p =>
            {
                var tags = session.Query<TagCount, Posts_Tags>()
                            .OrderBy(t => t.Value)
                            .ToList();

                return View["tags", tags];
            };
            
            // SlugRegexWithSegements is too greedy!
            Get["/(?<slug>{0})".FormatWith(SlugRegex)] = p => 
            {
                string slug = p.Slug;
                
                var post = session.Query<Post>()
                    .FirstOrDefault(x => x.Slug == slug);

                if (post == null)
                {
                    return new NotFoundResponse();
                }

                return View["Details", post];
            };
        }
    }
}