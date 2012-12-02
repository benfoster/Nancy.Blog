using Fabrik.Common;

namespace Nancy.Blog.Domain
{
    public class Tag
    {
        public string Value { get; private set; }
        public string Slug { get; private set; }

        public Tag(string value)
        {
            Ensure.Argument.NotNullOrEmpty(value, "value");
            Value = value;
            Slug = value.ToSlug();
        }

        public Tag()
        {

        }
    }
}