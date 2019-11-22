using System;
using System.IO;
using System.Linq;
using Castle.Core.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using MyBlog.Services;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MyBlogTests
{
    public class FileMarkdownGetterTest
    {
        [Fact]
        public void GetAll_ReturnsGeneratorOfPostString()
        {
            var envMock = new Mock<IHostEnvironment>();
            envMock.Setup(x => x.ContentRootPath)
                .Returns(() => Path.Combine(Directory.GetCurrentDirectory(), @"Stubs\"));
            var configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            configurationMock.Setup(x => x[It.IsAny<string>()])
                .Returns(() => "MarkdownFiles");

            var fileMarkdownGetter = new FileMarkdownGetter(envMock.Object, configurationMock.Object);

            Assert.Equal(2, fileMarkdownGetter.GetAll().ToList().Count);
        }

        [Theory]
        [InlineData("post1", "Post 1")]
        [InlineData("post2", "Post 2")]
        public void GetBySlug_ReturnsContentsOfTheRightFile(string slug, string title)
        {
            var envMock = new Mock<IHostEnvironment>();
            envMock.Setup(x => x.ContentRootPath)
                .Returns(() => Path.Combine(Directory.GetCurrentDirectory(), @"Stubs\"));
            var configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            configurationMock.Setup(x => x[It.IsAny<string>()])
                .Returns(() => "MarkdownFiles");

            var fileMarkdownGetter = new FileMarkdownGetter(envMock.Object, configurationMock.Object);

            Assert.Contains(title, fileMarkdownGetter.GetBySlug(slug), StringComparison.Ordinal);
        }

        [Fact]
        public void GetAll_ThrowsAnException_WhenTheDirectory_DoesNotExit()
        {
            var envMock = new Mock<IHostEnvironment>();
            envMock.Setup(x => x.ContentRootPath)
                .Returns(() => Path.Combine(Directory.GetCurrentDirectory(), @"Stubs\"));
            var configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            configurationMock.Setup(x => x[It.IsAny<string>()])
                .Returns(() => "NotExistedFolder");

            var fileMarkdownGetter = new FileMarkdownGetter(envMock.Object, configurationMock.Object);

            Assert.Throws<DirectoryNotFoundException>(() => fileMarkdownGetter.GetAll());
        }

        [Fact]
        public void GetBySlug_ThrowsAnException_WhenTheFile_DoesNotExit()
        {
            var envMock = new Mock<IHostEnvironment>();
            envMock.Setup(x => x.ContentRootPath)
                .Returns(() => Path.Combine(Directory.GetCurrentDirectory(), @"Stubs\"));
            var configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            configurationMock.Setup(x => x[It.IsAny<string>()])
                .Returns(() => "MarkdownFiles");

            var fileMarkdownGetter = new FileMarkdownGetter(envMock.Object, configurationMock.Object);

            Assert.Throws<FileNotFoundException>(() => fileMarkdownGetter.GetBySlug("not-exited-slug"));
        }
    }
}
