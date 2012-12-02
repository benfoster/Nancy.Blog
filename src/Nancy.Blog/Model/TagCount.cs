using Nancy.Blog.Domain;

namespace Nancy.Blog.Model
{
    public class TagCount : Tag
    {
        public int Count { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}