using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Jabbox.Data.Interfaces;
using Microsoft.Extensions.Options;

namespace Jabbox.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {

        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly AppSettings _appSettings;

        public BaseController(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        public string CurrentAccountName { 
            get { return this.HttpContext.User.Identity.Name; } 
        }


        public IEnumerable<TEntity> ProjectMap<TEntity>(IQueryable query)
        {
            return query.ProjectTo<TEntity>(_mapper.ConfigurationProvider).ToList();
        }
    }
}