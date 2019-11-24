using Moq;
using MyBlog.Models;
using MyBlog.Pages;
using MyBlog.Services;
using Xunit;

namespace MyBlogTests
{
    public class DetailsPageTest
    {
        [Fact]
        public void OnGet_populatesPostModel()
        {
            var post = new Post {Title = "Post 1"};

            var postData = new Mock<IPostData>();
            postData.Setup((x) => x.GetBySlug(It.IsAny<string>()))
                .Returns(() => post);

            var detailsModel = new DetailsModel(postData.Object);

            detailsModel.OnGet("post-1");

            Assert.IsAssignableFrom<Post>(detailsModel.Post);
            Assert.Equal("Post 1", detailsModel.Post.Title);
        }
    }
}
