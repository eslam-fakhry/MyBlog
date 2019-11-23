using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MyBlog.Services
{
    public class FileMarkdownGetter : IMarkdownGetter
    {
        private readonly string _markdownFolder;

        public FileMarkdownGetter(IHostEnvironment env, IConfiguration configuration)
        {
            _markdownFolder = Path.Combine(env.ContentRootPath, configuration["MarkdownFolder"]);
        }

        public IEnumerable<string> GetAll()
        {
            if (!Directory.Exists(_markdownFolder)) throw new DirectoryNotFoundException();
            foreach (var file in Directory.EnumerateFiles(_markdownFolder, "*.md"))
            {
                yield return File.ReadAllText(file);
            }
        }

        public string GetBySlug(string slug)
        {
            if (!Directory.Exists(_markdownFolder)) throw new DirectoryNotFoundException();
            var file = Directory.GetFiles(_markdownFolder, $"*{slug}.md").FirstOrDefault();
            if (string.IsNullOrWhiteSpace(file)) throw new FileNotFoundException();
            return File.ReadAllText(file);
        }
    }
}
