﻿using Markdig;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBlog.Services
{
    public class MarkdownPostData : IPostData
    {
        private IMarkdownGetter _markdownGetter;

        public MarkdownPostData(IMarkdownGetter markdownGetter)
        {
            _markdownGetter = markdownGetter;
        }
        public IEnumerable<Post> GetAllExcerpts()
        {
            throw new NotImplementedException();
        }

        public Post GetBySlug(string slug)
        {
            var markdown = _markdownGetter.GetBySlug(slug);

            // ^---.*\ntags:([^\n]*).*---(?<content>.*)$
            Regex rx = new Regex(@"^---(?<metadata>.*)---(?<content>.*)$",
                RegexOptions.Singleline);
            Match ts = rx.Match(markdown);

            var metadata = ts.Groups["metadata"].Value.Trim();
            var content = ts.Groups["content"].Value.Trim();
            var result = Markdown.ToHtml(content);
            var metadataDictionary = GetMetadataDictionary(metadata);


            var publishDate = !string.IsNullOrEmpty(metadataDictionary.GetValueOrDefault("date")) ? DateTime.Parse(metadataDictionary.GetValueOrDefault("date")) : DateTime.Now;

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
            var metadataDictionary = new Dictionary<string, string>();
            Regex rx = new Regex(@"(?<name>[a-zA-Z-]*):(?<value>[^\n]*)",
                RegexOptions.Singleline);
            MatchCollection matches = rx.Matches(metadata);

            foreach (Match match in matches)
            {
                var name = match.Groups["name"].Value.Trim();
                var value = match.Groups["value"].Value.Trim();
                metadataDictionary.Add(name, value);
            }

            return metadataDictionary;
        }
    }
}
