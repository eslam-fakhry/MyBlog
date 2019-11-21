using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Services
{
    public class MarkdownPostData : IPostData
    {
        public IEnumerable<Post> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
