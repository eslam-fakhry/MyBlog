using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Utilities;

namespace MyBlog.Services
{
    public interface IPostData
    {
        PaginatedCollection<Post> GetPaginatedPosts(int page, int pageSize);
        Post GetBySlug(string slug);
    }
}
