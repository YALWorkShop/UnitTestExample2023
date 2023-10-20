using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework
{
    public interface IPostRepository
    {
        Task<Post> Add(Post post);
        Task<List<Post>> GetAll();
        Task<Post> GetById(string id);
        Task<Post> Update(Post post);
    }

    public class PostRepository : IPostRepository
    {
        public Task<Post> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Post> Add(Post post)
        {
            throw new NotImplementedException();
        }

        public Task<Post> Update(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
