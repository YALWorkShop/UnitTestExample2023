﻿using ExpectedObjects;
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
        public void CreateBlog_create_blog_Success_with_Id()
        {
            var createModel = new BlogCreateModel()
            {
                Name = "It's me",
                Introduction = "write something here..."
            };

            GivenBlogId("blog1");

            var act = _service.CreateBlog(createModel);

            Assert.That(act.IsSuccess, Is.True);
            Assert.That(act.Result.Id, Is.EqualTo("blog1"));
        }

        [Test]
        public void CreateBlog_create_blog_Success_with_expected_blog()
        {
            var createModel = new BlogCreateModel()
            {
                Name = "It's me",
                Introduction = "write something here..."
            };

            GivenBlogId("blog1");

            var act = _service.CreateBlog(createModel);

            var expectedBlog = new Blog
            {
                Id = "blog1",
                Name = "It's me",
                Introduction = "write something here..."
            };

            Assert.That(act.IsSuccess, Is.True);
            Assert.That(act.ErrorMessage, Is.Null);
            Assert.IsTrue(expectedBlog.ToExpectedObject().Equals(act.Result));
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