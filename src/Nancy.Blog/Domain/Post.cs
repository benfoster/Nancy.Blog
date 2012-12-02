using Fabrik.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nancy.Blog.Domain
{
    public class Post
    {
        public string Id { get; private set; }
        public DateTime PublishDate { get; private set; }
        public string Title { get; private set; }
        public string Slug { get; private set; }
        public string Content { get; private set; }
        public Tag[] Tags { get; private set; }

        public Post(string title, string content, string slug = null, IEnumerable<string> tags = null, DateTime? publishDate = null)
        {
            Update(title, content, slug, tags, publishDate ?? DateTime.UtcNow);
        }

        public void Update(string title, string content, string slug = null, IEnumerable<string> tags = null, DateTime? publishDate = null)
        {
            Ensure.Argument.NotNullOrEmpty(title, "title");
            Ensure.Argument.NotNullOrEmpty(content, "content");

            Title = title;
            Slug = slug.IsNotNullOrEmpty() ? slug.ToSlug() : Title.ToSlug();
            Content = content;

            if (publishDate.HasValue)
            {
                PublishDate = publishDate.Value;
            }

            if (tags.IsNotNullOrEmpty())
            {
                Tags = tags.Select(t => new Tag(t)).ToArray();
            }
        }

        public Post()
        {

        }
    }
}