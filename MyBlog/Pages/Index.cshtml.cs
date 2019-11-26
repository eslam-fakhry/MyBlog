using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBlog.Models;
using MyBlog.Services;
using MyBlog.Utilities;

namespace MyBlog.Pages
{
    public class IndexModel : PageModel
    {
        private IPostData _postData;
        private int _pageSize;
        private const int DefaultPageSize = 8;

        public IndexModel(IPostData postData, IConfiguration config)
        {
            _postData = postData;
            if (!int.TryParse(config["PageSize"],out _pageSize))
            {
                _pageSize = DefaultPageSize;
            }
        }

        public PaginatedCollection<Post> Posts { get; private set; }

        public IActionResult OnGet(int? pageIndex)
        {
            Posts = _postData.GetPaginatedPosts(pageIndex ?? 1, _pageSize);
            
            if (Posts == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
