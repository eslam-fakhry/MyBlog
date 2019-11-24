using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Models;
using MyBlog.Services;

namespace MyBlog.Pages
{
    public class DetailsModel : PageModel
    {
        private IPostData _postData;

        public Post Post { get; private set; }
        public DetailsModel(IPostData postData)
        {
            _postData = postData;
        }
        public IActionResult OnGet(string slug)
        {
            Post = _postData.GetBySlug(slug);

            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
