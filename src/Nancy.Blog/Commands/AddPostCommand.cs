using System;
using System.Collections.Generic;

namespace Nancy.Blog.Commands
{
    public class AddPostCommand
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public DateTime? PublishDate { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}