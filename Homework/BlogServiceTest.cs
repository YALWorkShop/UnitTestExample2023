using Homework.Models;
using NUnit.Framework;

namespace Homework
{
    public class BlogServiceTest
    {
        [Test]
        public void CreateBlog_create_blog_Success_with_Id()
        {
            var service = new BlogService();

            var createModel = new BlogCreateModel()
            {
                Name = "It's me",
                Introduction = "write something here..."
            };

            var act = service.CreateBlog(createModel);

            Assert.IsTrue(act.IsSuccess);
            Assert.IsNotNull(act.Result.Id);
        }
    }
}
