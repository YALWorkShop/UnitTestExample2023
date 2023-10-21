using ExpectedObjects;
using Homework.Models;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

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
            var createModel = new PostCreateModel() { Title = "myTitle", Content = "exceed 10 words" };

            var expectedAddedPost = GeneratePost("post1", "myTitle", "exceed 10 words", fakeNow, fakeNow);

            GivenPostId("post1");
            GivenNow(fakeNow);
            GivenPostRepositoryAdd(expectedAddedPost, null);

            await CreatePostShouldBe(createModel, false, "文章發佈失敗", null);
            await PostRepositoryAddShouldReceived(expectedAddedPost, 1);
        }

        [Test]
        public async Task CreatePost_Success_Return_True_with_Post()
        {
            var createModel = new PostCreateModel() { Title = "myTitle", Content = "exceed 10 words" };

            var expectedAddedPost = GeneratePost("post1", "myTitle", "exceed 10 words", fakeNow, fakeNow);

            GivenPostId("post1");
            GivenNow(fakeNow);
            GivenPostRepositoryAdd(expectedAddedPost, expectedAddedPost);

            await CreatePostShouldBe(createModel, true, null, expectedAddedPost);
            await PostRepositoryAddShouldReceived(expectedAddedPost, 1);

            // make sure only call Add once
            await PostRepositoryAddWithAnyArgsShouldReceived(1);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task UpdatePost_TitleEmptyOrNull_Return_False_請輸入文章標題與內容(string title)
        {
            var postId = "post1";
            var updateModel = new PostUpdateModel() { Title = title };

            await UpdatePostShouldBe(postId, updateModel, false, "請輸入文章標題與內容", null);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task UpdatePost_ContentEmptyOrNull_Return_False_請輸入文章標題與內容(string content)
        {
            var postId = "post1";
            var updateModel = new PostUpdateModel() { Title = "myTitle", Content = content };

            await UpdatePostShouldBe(postId, updateModel, false, "請輸入文章標題與內容", null);
        }

        [Test]
        public async Task UpdatePost_ContentEmptyOrNull_Return_False_內容須超過10個字()
        {
            var postId = "post1";
            var updateModel = new PostUpdateModel() { Title = "myTitle", Content = "10words" };

            await UpdatePostShouldBe(postId, updateModel, false, "內容須超過 10 個字", null);
        }

        [Test]
        public async Task UpdatePost_Post_not_exist_Return_False_查無資料()
        {
            var postId = "post1";
            var updateModel = new PostUpdateModel() { Title = "myTitle", Content = "exceed 10 words" };

            GivenPostRepositoryGetById(postId, null);

            await UpdatePostShouldBe(postId, updateModel, false, "查無資料", null);
            await PostRepositoryGetByIdShouldReceived(postId, 1);
        }

        [Test]
        public async Task UpdatePost_PostRepositoryUpdate_fail_Return_False_exception_message()
        {
            var postId = "post1";
            var updateModel = new PostUpdateModel() { Title = "myTitle", Content = "exceed 10 words" };
            var updateException = new Exception("update fail");

            var originalPost = GeneratePost("post1", "old title", "old content is here", new DateTime(2023, 9, 30), new DateTime(2023, 9, 30));
            var updatedPost = GeneratePost("post1", "myTitle", "exceed 10 words", new DateTime(2023, 9, 30), fakeNow);

            GivenPostRepositoryGetById(postId, originalPost);
            GivenNow(fakeNow);
            GivenPostRepositoryUpdateThrow(updatedPost, updateException);

            await UpdatePostShouldBe(postId, updateModel, false, "update fail", null);
            await PostRepositoryGetByIdShouldReceived(postId, 1);
            await PostRepositoryUpdateShouldReceived(updatedPost, 1);
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

            expectedResult.ToExpectedObject().ShouldEqual(actualResult);
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

            expectedResult.ToExpectedObject().ShouldEqual(actualResult);
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

            expectedResult.ToExpectedObject().ShouldEqual(actualResult);
        }

        private async Task UpdatePostShouldBe(string id, PostUpdateModel updateModel, bool isSuccess, string errorMessage, Post expectedPost)
        {
            var actualResult = await _service.UpdatePost(id, updateModel);

            var expectedResult = new ServiceResult<Post>
            {
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                Result = expectedPost
            };

            expectedResult.ToExpectedObject().ShouldEqual(actualResult);
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

        private void GivenPostRepositoryAdd(Post addedPost, Post returnPost)
        {
            _postRepository.Add(Arg.Is<Post>(p => addedPost.ToExpectedObject().Equals(p))).Returns(Task.FromResult<Post>(returnPost));
        }

        private void GivenPostRepositoryGetById(string id, Post post)
        {
            _postRepository.GetById(id).Returns(Task.FromResult(post));
        }

        private void GivenPostRepositoryUpdateThrow(Post updatedPost, Exception exception)
        {
            _postRepository.Update(Arg.Is<Post>(p => updatedPost.ToExpectedObject().Equals(p))).Throws(exception);
        }

        private Post GeneratePost(string id, string title, string content, DateTime createTime, DateTime updateTime)
        {
            return new Post
            {
                Id = id,
                Title = title,
                Content = content,
                CreatTime = createTime,
                UpdateTime = updateTime
            };
        }

        private async Task PostRepositoryGetAllShouldReceived(int times)
        {
            await _postRepository.Received(times).GetAll();
        }

        private async Task PostRepositoryAddShouldReceived(Post post, int times)
        {
            await _postRepository.Received(times).Add(Arg.Is<Post>(p => post.ToExpectedObject().Equals(p)));
        }

        private async Task PostRepositoryAddWithAnyArgsShouldReceived(int times)
        {
            await _postRepository.Received(times).Add(Arg.Any<Post>());
        }

        private async Task PostRepositoryGetByIdShouldReceived(string id, int times)
        {
            await _postRepository.Received(times).GetById(id);
        }

        private async Task PostRepositoryUpdateShouldReceived(Post post, int times)
        {
            await _postRepository.Received(times).Update(Arg.Is<Post>(p => post.ToExpectedObject().Equals(p)));
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
