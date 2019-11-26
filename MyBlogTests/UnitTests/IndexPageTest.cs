using Moq;
using System;
using System.Collections.Generic;
using MyBlog.Models;
using MyBlog.Pages;
using MyBlog.Services;
using MyBlog.Utilities;
using Xunit;

namespace MyBlogTests
{
    public class IndexPageTest
    {
        [Fact]
        public void OnGet_PopulatesThePageModel_WithAListOfPosts()
        {
            var pageIndex = 1;
            var pageSize = 2;
            var posts = new PaginatedCollection<Post>(new List<Post>
            {
                new Post
                {
                    Title = "Post 1"
                },
                new Post
                {
                    Title = "Post 2"
                }
            }, 4, pageIndex, pageSize);

            var postData = new Mock<IPostData>();
            postData.Setup((x) => x.GetPaginatedPosts(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => posts);
            var configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            configurationMock.Setup(x => x[It.IsAny<string>()])
                .Returns(() => "2");
            
            var indexModel = new IndexModel(postData.Object, configurationMock.Object);

            indexModel.OnGet(pageIndex);

            Assert.IsAssignableFrom<IEnumerable<Post>>(indexModel.Posts);
        }
    }
}
