using Moq;
using MyBlog.Services;
using System;
using System.Collections.Generic;
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

    }
}
