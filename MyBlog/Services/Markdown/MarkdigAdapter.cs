namespace MyBlog.Services.Markdown
{
    public class MarkdigAdapter : IMarkdownConverter
    {
        public string Convert(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown);
        }
    }
}
