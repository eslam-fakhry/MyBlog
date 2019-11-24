using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MyBlog.Models;
using MyBlog.Services;
using MyBlog.Utilities;

namespace MyBlog.Pages
{
    public class IndexModel : PageModel
    {
        private IPostData _postData;

        public IndexModel(IPostData postData)
        {
            _postData = postData;
        }

        public PaginatedCollection<Post> Posts { get; private set; }

        public IActionResult OnGet(int? pageIndex)
        {
            const int pageSize = 2;
            
            Posts = _postData.GetPaginatedPosts(pageIndex ?? 1, pageSize);
            
            if (Posts == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
