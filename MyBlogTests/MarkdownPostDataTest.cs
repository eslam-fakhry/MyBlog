using Moq;
using MyBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using MyBlog.Models;
using Xunit;

namespace MyBlogTests
{
    public class MarkdownPostDataTest
    {
        [Fact]
        public void Get_ReturnsTheRightPost()
        {
            var slug = "my-first-blog-post";
            var markdown = @"---
title:Game Development with C#
tags: c# .net game
date:1-may-2019
excerpt:I will tell you all about that game development
---
# heading 1";

            var markdownGetter = new Mock<IMarkdownGetter>();
            markdownGetter.Setup(x => x.GetBySlug(It.IsAny<string>())).Returns(() => markdown);
            var markdownPostData = new MarkdownPostData(markdownGetter.Object);

            var post = markdownPostData.GetBySlug(slug);

            Assert.Equal("<h1>heading 1</h1>", post.Content.Trim());
            Assert.Equal(new List<string> {"c#", ".net", "game"}, post.Tags);
            Assert.Equal(DateTime.Parse("1-may-2019"), post.PublishDate);
            Assert.Equal("I will tell you all about that game development", post.Excerpt);
            Assert.Equal("Game Development with C#", post.Title);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        [InlineData(1)]
        public void GetPaginatedPosted_ReturnsAListOfPosts_WithTheRequestedCount(int pageSize)
        {
            var markdowns = new List<string>
            {
                @"---
title:Game Development with C#
tags: c# .net game
date:5-may-2019
excerpt:I will tell you all about that game development
---
# heading 1",
                @"---
title:Whats New With ASP.NET Core 3.2
tags: c# .net core
date:4-may-2019
excerpt:The new additions to ASP.NET Core 3.2 are...
---
## heading 2",
                @"---
title:Another Post About Programming
tags: deploy clean javascript
date:3-may-2019
excerpt:excerpt text
---
# heading 1",
                @"---
title:How To Learn Coding
tags: software programming coding
date:2-may-2019
excerpt:Another excerpt text
---
## heading 2",
            };
            
            var markdownGetter = new Mock<IMarkdownGetter>();
            markdownGetter.Setup(x => x.GetAll()).Returns(() => markdowns);
            var markdownPostData = new MarkdownPostData(markdownGetter.Object);
            var posts = markdownPostData.GetPaginatedPosts(1, pageSize);
            Assert.IsAssignableFrom<IEnumerable<Post>>(posts);
            Assert.Equal(pageSize,posts.Count);
        }
        
        [Theory]
        [InlineData(1,"Game Development with C#")]
        [InlineData(2,"Another Post About Programming")]
        public void GetPaginatedPosted_ReturnsAListOfPosts_WithTheRightPosts(int pageIndex , string expectedTitle)
        {
            var markdowns = new List<string>
            {
                @"---
title:Game Development with C#
tags: c# .net game
date:5-may-2019
excerpt:I will tell you all about that game development
---
# heading 1",
                @"---
title:Whats New With ASP.NET Core 3.2
tags: c# .net core
date:4-may-2019
excerpt:The new additions to ASP.NET Core 3.2 are...
---
## heading 2",
                @"---
title:Another Post About Programming
tags: deploy clean javascript
date:3-may-2019
excerpt:excerpt text
---
# heading 1",
                @"---
title:How To Learn Coding
tags: software programming coding
date:2-may-2019
excerpt:Another excerpt text
---
## heading 2",
            };
            
            var markdownGetter = new Mock<IMarkdownGetter>();
            markdownGetter.Setup(x => x.GetAll()).Returns(() => markdowns);
            var markdownPostData = new MarkdownPostData(markdownGetter.Object);
            var posts = markdownPostData.GetPaginatedPosts(pageIndex, 2);
            Assert.Equal(expectedTitle,posts.First().Title);
        }
    }
}
