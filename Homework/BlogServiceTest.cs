using ExpectedObjects;
using Homework.Models;
using NUnit.Framework;
using NSubstitute;

namespace Homework
{
    public class BlogServiceTest
    {
        FakeBlogService _service;
        IPostRepository _postRepository;

        [SetUp]
        public void SetUp()
        {
            _postRepository = Substitute.For<IPostRepository>();
            _service = new FakeBlogService(_postRepository);
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

        [Test]
        public async Task GetAllPosts_No_Post_Return_Empty_ListOfBlog()
        {
            GivenPostRepositoryGetAll(null);

            await GetAllPostsShouldBe(true, null, new List<Post>());
            await PostRepositoryGetAllShouldReceived(1);
        }

        [Test]
        public async Task GetAllPosts_Return_2_Posts()
        {
            var allPosts = new List<Post> { new Post { Id = "post1" }, new Post { Id = "post2" } };
            GivenPostRepositoryGetAll(allPosts);

            await GetAllPostsShouldBe(true, null, allPosts);
            await PostRepositoryGetAllShouldReceived(1);
        }

        [Test]
        public async Task CreatePost_TitleEmpty_Return_False_請輸入文章標題與內容()
        {
            var createModel = new PostCreateModel() { Title = "" };

            await CreatePostShouldBe(createModel, false, "請輸入文章標題與內容", null);
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

        private async Task GetAllPostsShouldBe(bool isSuccess, string errorMessage, List<Post> expectedPosts)
        {
            var actualResult = await _service.GetAllPosts();

            var expectedResult = new ServiceResult<List<Post>>
            {
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                Result = expectedPosts
            };

            Assert.IsTrue(expectedResult.ToExpectedObject().Equals(actualResult));
        }

        private async Task CreatePostShouldBe(PostCreateModel createModel ,bool isSuccess, string errorMessage, Post expectedPost)
        {
            var actualResult = await _service.CreatePost(createModel);

            var expectedResult = new ServiceResult<Post>
            {
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                Result = expectedPost
            };

            Assert.IsTrue(expectedResult.ToExpectedObject().Equals(actualResult));
        }

        private void GivenBlogId(string blogId)
        {
            _service.SetBlogId(blogId);
        }

        private void GivenPostRepositoryGetAll(List<Post> posts)
        {
            _postRepository.GetAll().Returns(Task.FromResult(posts));
        }

        private async Task PostRepositoryGetAllShouldReceived(int times)
        {
            await _postRepository.Received(times).GetAll();
        }
    }

    public class FakeBlogService : BlogService
    {
        private string _blogId;

        public FakeBlogService(IPostRepository postRepository) : base(postRepository)
        {
        }

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
