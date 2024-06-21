using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Jabbox.API.Models;
using AutoMapper;
using Jabbox.Data.Interfaces;
using Microsoft.Extensions.Options;

namespace Jabbox.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : BaseController
    {

        public PostController(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings) { }
        

        [HttpGet(Name = "GetPosts")]
        public async Task<IEnumerable<PostDTO>> Get([FromQuery] string userName)
        {
            Console.WriteLine(_appSettings.SiteName);
            return ProjectMap<PostDTO>(_unitOfWork.Posts.GetPosts(userName));

        }

        [HttpPut(Name = "AddPost"), Authorize]
        public async Task<PostDTO> Put(PostDTO post)
        {
            var newPost = _unitOfWork.Posts.AddPost(CurrentAccountName, post.Message);
            _unitOfWork.SaveChanges();
            return _mapper.Map<PostDTO>(newPost);
        }
    }
}