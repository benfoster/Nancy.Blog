using Nancy.Blog.Domain;
using Nancy.Blog.Model;
using Raven.Client.Indexes;
using System.Linq;

namespace Nancy.Blog.Indexes
{
    public class Posts_Tags : AbstractIndexCreationTask<Post, TagCount>
    {
        public Posts_Tags()
        {
            Map = posts => from p in posts
                           from t in p.Tags
                           select new
                           {
                               Value = t.Value,
                               Slug = t.Slug,
                               Count = 1
                           };

            Reduce = results => from result in results
                                group result by result.Slug into g
                                select new
                                {
                                    Value = g.Select(x => x.Value).FirstOrDefault(),
                                    Slug = g.Select(x =>x.Slug).FirstOrDefault(),
                                    Count = g.Sum(x => x.Count)
                                };
        }
    }
}