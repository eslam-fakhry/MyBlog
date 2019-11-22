using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class Post
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
