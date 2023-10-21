using Homework.Models;

namespace Homework
{
    public class BlogService
    {
        public ServiceResult<Blog> CreateBlog(BlogCreateModel blogCreateModel)
        {
            return new ServiceResult<Blog>
            {
                IsSuccess = true,
                Result = new Blog
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = blogCreateModel.Name,
                    Introduction = blogCreateModel.Introduction
                }
            };
        }

        public async Task<ServiceResult<List<Post>>> GetAllPosts()
        {
            var repository = new PostRepository();

            var allPosts = await repository.GetAll() ?? new List<Post>();

            return new ServiceResult<List<Post>>
            {
                IsSuccess = true,
                Result = allPosts
            };
        }

        public async Task<ServiceResult<Post>> CreatePost(PostCreateModel postCreateModel)
        {
            if (string.IsNullOrEmpty(postCreateModel.Title) || string.IsNullOrEmpty(postCreateModel.Content))
            {
                return new ServiceResult<Post>
                {
                    IsSuccess = false,
                    ErrorMessage = "請輸入文章標題與內容"
                };
            }

            if (postCreateModel.Content.Length <= 10)
            {
                return new ServiceResult<Post>
                {
                    IsSuccess = false,
                    ErrorMessage = "內容須超過 10 個字"
                };
            }

            var newPost = new Post()
            {
                Id = Guid.NewGuid().ToString("N"),
                Title = postCreateModel.Title,
                Content = postCreateModel.Content,
                CreatTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            var repository = new PostRepository();
            var createResult = await repository.Add(newPost);

            if (createResult == null)
            {
                return new ServiceResult<Post>
                {
                    IsSuccess = false,
                    ErrorMessage = "文章發佈失敗"
                };
            }

            return new ServiceResult<Post>
            {
                IsSuccess = true,
                Result = createResult
            };
        }

        public async Task<ServiceResult<Post>> UpdatePost(string id, PostUpdateModel postUpdateModel)
        {
            try
            {
                if (string.IsNullOrEmpty(postUpdateModel.Title) || string.IsNullOrEmpty(postUpdateModel.Content))
                {
                    return new ServiceResult<Post>
                    {
                        IsSuccess = false,
                        ErrorMessage = "請輸入文章標題與內容"
                    };
                }

                if (postUpdateModel.Content.Length <= 10)
                {
                    return new ServiceResult<Post>
                    {
                        IsSuccess = false,
                        ErrorMessage = "內容須超過 10 個字"
                    };
                }

                var repository = new PostRepository();
                var post = await repository.GetById(id);
                if (post == null)
                {
                    throw new Exception("查無資料");
                }

                post.Title = postUpdateModel.Title;
                post.Content = postUpdateModel.Content;
                post.UpdateTime = DateTime.Now;

                var updateResult = await repository.Update(post);

                return new ServiceResult<Post>
                {
                    IsSuccess = true,
                    Result = updateResult
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Post>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
