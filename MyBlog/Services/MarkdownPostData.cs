using Markdig;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyBlog.Utilities;

namespace MyBlog.Services
{
    public class MarkdownPostData : IPostData
    {
        private IMarkdownGetter _markdownGetter;

        public MarkdownPostData(IMarkdownGetter markdownGetter)
        {
            _markdownGetter = markdownGetter;
        }


        public PaginatedCollection<Post> GetPaginatedPosts(int page, int pageSize)
        {
            var markdowns = _markdownGetter.GetAll();
            var count = _markdownGetter.GetAll().Count();


            return new PaginatedCollection<Post>(
                markdowns.Select(PostFromMarkdown).Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                count,
                page,
                pageSize
            );
        }

        public Post GetBySlug(string slug)
        {
            var markdown = _markdownGetter.GetBySlug(slug);

            return PostFromMarkdown(markdown);
        }

        private Post PostFromMarkdown(string markdown)
        {
            var rx = new Regex(@"^---(?<metadata>.*)---(?<content>.*)$",
                RegexOptions.Singleline);
            Match ts = rx.Match(markdown);

            var metadata = ts.Groups["metadata"].Value.Trim();
            var content = ts.Groups["content"].Value.Trim();
            var result = Markdown.ToHtml(content);
            var metadataDictionary = GetMetadataDictionary(metadata);


            var publishDate = !string.IsNullOrEmpty(metadataDictionary.GetValueOrDefault("date"))
                ? DateTime.Parse(metadataDictionary.GetValueOrDefault("date"))
                : DateTime.Now;

            return new Post
            {
                Content = result,
                Tags = metadataDictionary.GetValueOrDefault("tags").Split(" "),
                Title = metadataDictionary.GetValueOrDefault("title"),
                Excerpt = metadataDictionary.GetValueOrDefault("excerpt"),
                PublishDate = publishDate
            };
        }

        private Dictionary<string, string> GetMetadataDictionary(string metadata)
        {
            Regex rx = new Regex(@"(?<name>[a-zA-Z-]*):(?<value>[^\n]*)",
                RegexOptions.Singleline);
            MatchCollection matches = rx.Matches(metadata);

            return GetDictionaryFromMetadataMatches(matches);
        }

        private static Dictionary<string, string> GetDictionaryFromMetadataMatches(MatchCollection matches)
        {
            var metadataDictionary = new Dictionary<string, string>();
            foreach (Match match in matches)
            {
                AddMatchItemToDictionary(match, metadataDictionary);
            }

            return metadataDictionary;
        }

        private static void AddMatchItemToDictionary(Match match, IDictionary<string, string> metadataDictionary)
        {
            var name = match.Groups["name"].Value.Trim();
            var value = match.Groups["value"].Value.Trim();
            metadataDictionary.Add(name, value);
        }
    }
}
