using ExpectedObjects;
using Homework.Models;
using NUnit.Framework;

namespace Homework
{
    public class BlogServiceTest
    {
        FakeBlogService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new FakeBlogService();
        }

        [Test]
        public void CreateBlog_create_blog_Success_with_expected_blog()
        {
            var createModel = new BlogCreateModel()
            {
                Name = "It's me",
                Introduction = "write something here..."
            };

            var expectedBlog = new Blog
            {
                Id = "blog1",
                Name = "It's me",
                Introduction = "write something here..."
            };

            GivenBlogId("blog1");

            CreateBlogShouldBe(createModel, true, null, expectedBlog);
        }

        private void CreateBlogShouldBe(BlogCreateModel createModel, bool isSuccess, string errorMessage, Blog expectedBlog)
        {
            var actualResult = _service.CreateBlog(createModel);

            var expectedResult = new ServiceResult<Blog>
            {
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                Result = expectedBlog
            };

            Assert.IsTrue(expectedResult.ToExpectedObject().Equals(actualResult));
        }

        private void GivenBlogId(string blogId)
        {
            _service.SetBlogId(blogId);
        }
    }

    public class FakeBlogService : BlogService
    {
        private string _blogId;

        public void SetBlogId(string blogId)
        {
            _blogId = blogId;
        }

        protected override string GetBlogId()
        {
            return _blogId;
        }
    }
}
