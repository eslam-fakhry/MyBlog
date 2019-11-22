using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Services
{
    public interface IPostData
    {
        IEnumerable<Post> GetAllExcerpts();
        Post GetBySlug(string slug);
    }
}
