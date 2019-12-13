using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyBlog.Services.Markdown;
using MyBlog.Utilities;

namespace MyBlog.Services
{
    public class MarkdownPostData : IPostData
    {
        private IMarkdownGetter _markdownGetter;
        private readonly IMarkdownConverter _markdownConverter;

        public MarkdownPostData(IMarkdownGetter markdownGetter, IMarkdownConverter markdownConverter)
        {
            _markdownGetter = markdownGetter;
            _markdownConverter = markdownConverter;
        }


        public PaginatedCollection<Post> GetPaginatedPosts(int page, int pageSize)
        {
            try
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
            catch (DirectoryNotFoundException exception)
            {
                return null;
            }
        }

        public Post GetBySlug(string slug)
        {
            try
            {
                var markdown = _markdownGetter.GetBySlug(slug);
                return PostFromMarkdown(markdown);
            }
            catch (FileNotFoundException exception)
            {
                return null;
            }
            catch (DirectoryNotFoundException exception)
            {
                return null;
            }
        }

        private Post PostFromMarkdown(string markdown)
        {
            var rx = new Regex(@"^---(?<metadata>.*)---(?<content>.*)$",
                RegexOptions.Singleline);
            Match ts = rx.Match(markdown);

            var metadata = ts.Groups["metadata"].Value.Trim();
            var content = ts.Groups["content"].Value.Trim();
            var result = _markdownConverter.Convert(content);
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
                Slug = metadataDictionary.GetValueOrDefault("slug"),
                Image = metadataDictionary.GetValueOrDefault("image"),
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
