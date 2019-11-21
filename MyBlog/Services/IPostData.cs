using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Services
{
    interface IPostData
    {
        IEnumerable<Post> GetAll();
    }
}
