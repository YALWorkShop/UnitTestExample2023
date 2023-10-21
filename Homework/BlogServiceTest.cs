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
        DateTime fakeNow = new DateTime(2023, 10, 01, 10, 0, 0);

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

        [TestCase("")]
        [TestCase(null)]
        public async Task CreatePost_TitleEmptyOrNull_Return_False_請輸入文章標題與內容(string title)
        {
            var createModel = new PostCreateModel() { Title = title };

            await CreatePostShouldBe(createModel, false, "請輸入文章標題與內容", null);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task CreatePost_ContentEmptyOrNull_Return_False_請輸入文章標題與內容(string content)
        {
            var createModel = new PostCreateModel() { Title = "myTitle", Content = content };

            await CreatePostShouldBe(createModel, false, "請輸入文章標題與內容", null);
        }

        [Test]
        public async Task CreatePost_ContentLessThan10words_Return_False_內容須超過10個字()
        {
            var createModel = new PostCreateModel() { Title = "myTitle", Content = "10 words" };

            await CreatePostShouldBe(createModel, false, "內容須超過 10 個字", null);
        }

        [Test]
        public async Task CreatePost_PostRepositoryAddFail_Return_False_文章發佈失敗()
        {
            // wrong logic: should be GivenPostId
            GivenBlogId("post1");
            GivenNow(fakeNow);

            var createModel = new PostCreateModel() { Title = "myTitle", Content = "exceed 10 words" };

            var expectedAddedPost = new Post()
            {
                Id = "post1",
                Title = "myTitle",
                Content = "exceed 10 words",
                CreatTime = fakeNow,
                UpdateTime = fakeNow
            };

            // if setting wrong(or not set), postRepository.Add always return null
             _postRepository.Add(Arg.Is<Post>(p => p.ToExpectedObject().Equals(expectedAddedPost))).Returns(Task.FromResult<Post>(null));
            await CreatePostShouldBe(createModel, false, "文章發佈失敗", null);
            await _postRepository.Received(1).Add(Arg.Is<Post>(p => p.ToExpectedObject().Equals(expectedAddedPost)));
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

        private async Task CreatePostShouldBe(PostCreateModel createModel, bool isSuccess, string errorMessage, Post expectedPost)
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

        private void GivenPostId(string postId)
        {
            _service.SetPostId(postId);
        }

        private void GivenNow(DateTime now)
        {
            _service.SetNow(now);
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
        private string _postId;
        private DateTime _now;

        public FakeBlogService(IPostRepository postRepository) : base(postRepository)
        {
        }

        public void SetBlogId(string blogId)
        {
            _blogId = blogId;
        }

        public void SetPostId(string postId)
        {
            _postId = postId;
        }

        public void SetNow(DateTime now)
        {
            _now = now;
        }

        protected override string GetBlogId()
        {
            return _blogId;
        }

        protected override string GetPostId()
        {
            return _postId;
        }

        protected override DateTime GetNow()
        {
            return _now;
        }
    }
}
