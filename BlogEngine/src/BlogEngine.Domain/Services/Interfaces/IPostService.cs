using BlogEngine.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Domain.Services.Interfaces
{
    public interface IPostService
    {
        Post AddPost(Post post);

        Post Find(int id);

        Post UpdatePost(Post post);
    }
}
